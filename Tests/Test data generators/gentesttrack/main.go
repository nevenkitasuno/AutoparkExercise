package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"math/rand"
	"net/http"
	"time"
)

type UpsertGpsPointDto struct {
	VehicleId Guid      `json:"vehicleId"`
	Timestamp  time.Time `json:"timestamp"`
	Latitude   float64   `json:"latitude"`
	Longitude  float64   `json:"longitude"`
}

type Guid string

type RouteRequest struct {
	Coordinates [][]float64 `json:"coordinates"`
	Profile     string      `json:"profile"`
}

type RouteResponse struct {
	Routes []struct {
		Geometry struct {
			Coordinates [][][2]float64 `json:"coordinates"`
		} `json:"geometry"`
	} `json:"routes"`
}

func getRoute(start, end [2]float64) ([][][2]float64, error) {
	client := &http.Client{}
	reqBody := RouteRequest{
		Coordinates: [][]float64{{start[0], start[1]}, {end[0], end[1]}},
		Profile:     "driving-car",
	}

	body, err := json.Marshal(reqBody)
	if err != nil {
		return nil, err
	}

	req, err := http.NewRequest("POST", "https://api.openrouteservice.org/v2/directions/driving-car", bytes.NewBuffer(body))
	if err != nil {
		return nil, err
	}
	req.Header.Set("Authorization", "YOUR_API_KEY") // Replace with your OpenRouteService API key
	req.Header.Set("Content-Type", "application/json")

	resp, err := client.Do(req)
	if err != nil {
		return nil, err
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return nil, fmt.Errorf("error fetching route: %s", resp.Status)
	}

	body, _ = ioutil.ReadAll(resp.Body)

	var routeResponse RouteResponse
	if err := json.Unmarshal(body, &routeResponse); err != nil {
		return nil, err
	}

	return routeResponse.Routes[0].Geometry.Coordinates, nil
}

func postGpsPoint(point UpsertGpsPointDto) error {
	client := &http.Client{}
	url := "http://localhost:5237/api/GpsPoint"

	body, err := json.Marshal(point)
	if err != nil {
		return err
	}

	req, err := http.NewRequest("POST", url, bytes.NewBuffer(body))
	if err != nil {
		return err
	}
	req.Header.Set("Content-Type", "application/json")

	resp, err := client.Do(req)
	if err != nil {
		return err
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		bodyResp, _ := ioutil.ReadAll(resp.Body)
		return fmt.Errorf("error posting GPS point: %s, response: %s", resp.Status, string(bodyResp))
	}

	return nil
}

func generateTrack(vehicleId Guid, routeCoords [][][2]float64, interval time.Duration) {
	startTime := time.Now()

	for _, segment := range routeCoords {
		for i := 0; i < len(segment)-1; i++ {
			start := segment[i]
			end := segment[i+1]

			// Simulate movement along the segment
			steps := 10 // Number of points to generate along the segment
			for j := 0; j <= steps; j++ {
				factor := float64(j) / float64(steps)
				lat := start[1]*(1-factor) + end[1]*factor
				lng := start[0]*(1-factor) + end[0]*factor

				point := UpsertGpsPointDto{
					VehicleId: vehicleId,
					Timestamp: startTime.Add(time.Duration(j) * interval),
					Latitude:  lat,
					Longitude: lng,
				}

				// Post the point to the API
				if err := postGpsPoint(point); err != nil {
					fmt.Println("Error posting point:", err)
				} else {
					fmt.Printf("Recorded and posted point: %+v\n", point)
				}

				time.Sleep(interval)
			}
		}
	}
}

func main() {
	vehicleId := Guid("123e4567-e89b-12d3-a456-426614174000")
	start := [2]float64{8.681495, 49.41461} // Example: starting coordinates
	end := [2]float64{8.686507, 49.41943}   // Example: ending coordinates

	routeCoords, err := getRoute(start, end)
	if err != nil {
		fmt.Println("Error fetching route:", err)
		return
	}

	interval := 10 * time.Second // 10 seconds
	generateTrack(vehicleId, routeCoords, interval)
}
