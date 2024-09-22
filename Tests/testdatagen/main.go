package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"math/rand"
	"net/http"
	"time"
)

const (
	countOfVehiclesToPost = 100
	baseURL               = "http://localhost:5237"
	enterpriseId          = "3fa85f64-5717-4562-b3fc-2c963f66afa6"
	dateOfBirthYearFrom   = 1900
	dateOfBirthYearTo     = 2020
	manufactureYearFrom   = 1900
	manufactureYearTo     = 2020
	priceFrom             = 50000
	priceTo               = 2000000
	mileageFrom           = 500000
	mileageTo             = 200000
)

var loginData = map[string]string{
	"email":    "vasilyPetrovich@mail.ry",
	"password": "SuperParol123",
}

type Driver struct {
	FirstName        string    `json:"firstName"`
	Surname          string    `json:"surname"`
	Patronymic       string    `json:"patronymic"`
	DateOfBirth      time.Time `json:"dateOfBirth"`
	Salary           int       `json:"salary"`
	EnterpriseId     string    `json:"enterpriseId"`
	CurrentVehicleId string    `json:"currentVehicleId,omitempty"`
}

type Vehicle struct {
	LicensePlate    string `json:"licensePlate"`
	Price           int    `json:"price"`
	ManufactureYear int    `json:"manufactureYear"`
	Mileage         int    `json:"mileage"`
	BrandId         int    `json:"brandId"`
	EnterpriseId    string `json:"enterpriseId"`
	CurrentDriverId string `json:"currentDriverId,omitempty"`
}

type LoginResponse struct {
	TokenType    string `json:"tokenType"`
	AccessToken  string `json:"accessToken"`
	ExpiresIn    int    `json:"expiresIn"`
	RefreshToken string `json:"refreshToken"`
}

var firstNames = []string{"Сергей", "Алексей", "Иван", "Дмитрий", "Николай"}
var surnames = []string{"Иванов", "Петров", "Сидоров", "Кузнецов", "Смирнов"}
var patronymics = []string{"Сергеевич", "Алексеевич", "Иванович", "Дмитриевич", "Николаевич"}

var letters = []rune{'А', 'В', 'Е', 'К', 'М', 'Н', 'О', 'Р', 'С', 'Т', 'У', 'Х'}

func generateLicensePlate() string {
	licensePlate := make([]rune, 6)
	for i := 0; i < 3; i++ {
		licensePlate[i] = letters[rand.Intn(len(letters))]
	}
	for i := 3; i < 6; i++ {
		licensePlate[i] = rune(rand.Intn(10) + '0')
	}
	return string(licensePlate)
}

func generateRandomValue(min, max int) int {
	return rand.Intn(max-min+1) + min
}

func postDriver(driver Driver, token string) (string, error) {
	body, _ := json.Marshal(driver)
	req, err := http.NewRequest("POST", baseURL+"/api/Driver", bytes.NewBuffer(body))
	if err != nil {
		return "", err
	}
	req.Header.Set("Content-Type", "application/json")
	req.Header.Set("Authorization", "Bearer "+token)

	client := &http.Client{}
	resp, err := client.Do(req)
	if err != nil {
		return "", err
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return "", fmt.Errorf("failed to post driver: %s", resp.Status)
	}

	var createdDriver Driver
	if err := json.NewDecoder(resp.Body).Decode(&createdDriver); err != nil {
		return "", err
	}

	return createdDriver.CurrentVehicleId, nil
}

func postVehicle(vehicle Vehicle, token string) error {
	body, _ := json.Marshal(vehicle)
	req, err := http.NewRequest("POST", baseURL+"/api/Vehicle", bytes.NewBuffer(body))
	if err != nil {
		return err
	}
	req.Header.Set("Content-Type", "application/json")
	req.Header.Set("Authorization", "Bearer "+token)

	client := &http.Client{}
	resp, err := client.Do(req)
	if err != nil {
		return err
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return fmt.Errorf("failed to post vehicle: %s", resp.Status)
	}

	return nil
}

func login() (string, error) {
	body, _ := json.Marshal(loginData)
	req, err := http.NewRequest("POST", baseURL+"/identity/login", bytes.NewBuffer(body))
	if err != nil {
		return "", err
	}
	req.Header.Set("Content-Type", "application/json")

	client := &http.Client{}
	resp, err := client.Do(req)
	if err != nil {
		return "", err
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return "", fmt.Errorf("login failed: %s", resp.Status)
	}

	var response LoginResponse
	if err := json.NewDecoder(resp.Body).Decode(&response); err != nil {
		return "", err
	}

	return response.AccessToken, nil
}

func main() {
	rand.Seed(time.Now().UnixNano())

	token, err := login()
	if err != nil {
		fmt.Println("Error during login:", err)
		return
	}

	var currentDriverId string

	for i := 0; i < countOfVehiclesToPost; i++ {
		driver := Driver{
			FirstName:    firstNames[rand.Intn(len(firstNames))],
			Surname:      surnames[rand.Intn(len(surnames))],
			Patronymic:   patronymics[rand.Intn(len(patronymics))],
			DateOfBirth:  time.Date(generateRandomValue(dateOfBirthYearFrom, dateOfBirthYearTo), time.Month(rand.Intn(12)+1), rand.Intn(28)+1, 0, 0, 0, 0, time.UTC),
			Salary:       generateRandomValue(50000, 200000),
			EnterpriseId: enterpriseId,
		}

		if i%10 == 0 {
			driver.CurrentVehicleId = currentDriverId // Every 10th driver has a valid vehicle ID
		} else {
			driver.CurrentVehicleId = "" // Otherwise, the vehicle ID is empty
		}

		driverId, err := postDriver(driver, token)
		if err != nil {
			fmt.Println("Error posting driver:", err)
			continue
		}

		vehicle := Vehicle{
			LicensePlate:    generateLicensePlate(),
			Price:           generateRandomValue(priceFrom, priceTo),
			ManufactureYear: generateRandomValue(manufactureYearFrom, manufactureYearTo),
			Mileage:         generateRandomValue(mileageFrom, mileageTo),
			BrandId:         2,
			EnterpriseId:    enterpriseId,
		}

		if i%10 == 0 {
			vehicle.CurrentDriverId = driverId // Every 10th vehicle has a valid driver ID
		} else {
			vehicle.CurrentDriverId = "" // Otherwise, the driver ID is empty
		}

		if err := postVehicle(vehicle, token); err != nil {
			fmt.Println("Error posting vehicle:", err)
		}
	}
}
