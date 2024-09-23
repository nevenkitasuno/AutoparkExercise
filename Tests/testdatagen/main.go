package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io"
	"math/rand"
	"net/http"
	"time"
)

const (
	countOfVehiclesToPost = 10
	baseURL               = "http://localhost:5237"
	enterpriseId          = "016790c2-cf54-4fd0-8d7e-2a154912f639"
	dateOfBirthYearFrom   = 1950
	dateOfBirthYearTo     = 2005
	manufactureYearFrom   = 1990
	manufactureYearTo     = 2020
	priceFrom             = 50000
	priceTo               = 5000000
	mileageFrom           = 50000
	mileageTo             = 500000
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

type UpsertVehicleDto struct {
	LicensePlate    string `json:"licensePlate"`
	Price           int    `json:"price"`
	ManufactureYear int    `json:"manufactureYear"`
	Mileage         int    `json:"mileage"`
	BrandId         int    `json:"brandId"`
	EnterpriseId    string `json:"enterpriseId"`
}

type GetVehicleDto struct {
	Id              string `json:"id"`
	LicensePlate    string `json:"licensePlate"`
	Price           int    `json:"price"`
	ManufactureYear int    `json:"manufactureYear"`
	Mileage         int    `json:"mileage"`
	BrandId         int    `json:"brandId"`
	EnterpriseId    string `json:"enterpriseId"`
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
	licensePlate[0] = rune(rand.Intn(10) + '0')
	for i := 1; i < 4; i++ {
		licensePlate[i] = letters[rand.Intn(len(letters))]
	}
	for i := 4; i < 6; i++ {
		licensePlate[i] = rune(rand.Intn(10) + '0')
	}
	return string(licensePlate)
}

func generateRandomValue(min, max int) int {
	return rand.Intn(max-min+1) + min
}

func postDriver(driver Driver, cookie *http.Cookie) error {
	body, _ := json.Marshal(driver)
	req, err := http.NewRequest("POST", baseURL+"/api/Driver", bytes.NewBuffer(body))
	if err != nil {
		return err
	}
	req.Header.Set("Content-Type", "application/json")

	req.AddCookie(cookie)

	client := &http.Client{}
	resp, err := client.Do(req)
	if err != nil {
		return err
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusCreated {
		return fmt.Errorf("failed to post driver: %s", resp.Status)
	}

	return nil
}

func postVehicle(vehicle UpsertVehicleDto, cookie *http.Cookie) (string, error) {
	body, _ := json.Marshal(vehicle)
	req, err := http.NewRequest("POST", baseURL+"/api/Vehicle", bytes.NewBuffer(body))
	if err != nil {
		return "", err
	}
	req.Header.Set("Content-Type", "application/json")

	req.AddCookie(cookie)

	client := &http.Client{}
	resp, err := client.Do(req)
	if err != nil {
		return "", err
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusCreated {
		body, _ := io.ReadAll(resp.Body)
		return "", fmt.Errorf("failed to post vehicle: %s, body: %s", resp.Status, string(body))
	}

	var createdVehicle GetVehicleDto
	if err := json.NewDecoder(resp.Body).Decode(&createdVehicle); err != nil {
		return "", err
	}

	return createdVehicle.Id, nil
}

func login() (*http.Cookie, error) {
	body, _ := json.Marshal(loginData)
	req, err := http.NewRequest("POST", baseURL+"/identity/login?useCookies=true", bytes.NewBuffer(body))
	if err != nil {
		return nil, err
	}
	req.Header.Set("Content-Type", "application/json")

	client := &http.Client{}
	resp, err := client.Do(req)
	if err != nil {
		return nil, err
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return nil, fmt.Errorf("login failed: %s", resp.Status)
	}

	var cookie *http.Cookie
	cookies := resp.Cookies()
	for _, c := range cookies {
		if c.Name == ".AspNetCore.Identity.Application" {
			cookie = c
			break
		}
	}

	return cookie, nil
}

func main() {
	rand.Seed(time.Now().UnixNano())

	cookie, err := login()
	if err != nil {
		fmt.Println("Error during login:", err)
		return
	}

	for i := 0; i < countOfVehiclesToPost; i++ {

		vehicle := UpsertVehicleDto{
			LicensePlate:    generateLicensePlate(),
			Price:           generateRandomValue(priceFrom, priceTo),
			ManufactureYear: generateRandomValue(manufactureYearFrom, manufactureYearTo),
			Mileage:         generateRandomValue(mileageFrom, mileageTo),
			BrandId:         2,
			EnterpriseId:    enterpriseId,
		}

		postedVehicleId, err := postVehicle(vehicle, cookie)
		if err != nil {
			fmt.Println("Error posting vehicle: ", err)
			continue
		}

		if i%10 == 0 {
			driver := Driver{
				FirstName:        firstNames[rand.Intn(len(firstNames))],
				Surname:          surnames[rand.Intn(len(surnames))],
				Patronymic:       patronymics[rand.Intn(len(patronymics))],
				DateOfBirth:      time.Date(generateRandomValue(dateOfBirthYearFrom, dateOfBirthYearTo), time.Month(rand.Intn(12)+1), rand.Intn(28)+1, 0, 0, 0, 0, time.UTC),
				Salary:           generateRandomValue(50000, 200000),
				EnterpriseId:     enterpriseId,
				CurrentVehicleId: postedVehicleId,
			}

			if err := postDriver(driver, cookie); err != nil {
				fmt.Println("Error posting driver: ", err)
			}
		}
	}
}
