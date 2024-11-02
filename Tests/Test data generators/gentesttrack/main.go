package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io"
	"net/http"
	"time"
)

type UpsertGpsPointDto struct {
	VehicleId Guid      `json:"vehicleId"`
	Timestamp time.Time `json:"timestamp"`
	Latitude  float64   `json:"latitude"`
	Longitude float64   `json:"longitude"`
}

type Guid string

type RouteRequest struct {
	Coordinates [][]float64 `json:"coordinates"`
	Profile     string      `json:"profile"`
}

type RouteResponse struct {
	Routes []struct {
		Summary struct {
			Distance float64 `json:"distance"`
			Duration float64 `json:"duration"`
		} `json:"summary"`
		Segments []struct {
			Distance float64 `json:"distance"`
			Duration float64 `json:"duration"`
			Steps    []struct {
				Distance    float64 `json:"distance"`
				Duration    float64 `json:"duration"`
				Type        int     `json:"type"`
				Instruction string  `json:"instruction"`
				Name        string  `json:"name"`
				WayPoints   []int   `json:"way_points"`
			} `json:"steps"`
		} `json:"segments"`
		Geometry string `json:"geometry"` // Keep this as a string for encoded polyline
	} `json:"routes"`
}

func getRoute(start, end [2]float64) ([][]float64, error) {
	client := &http.Client{}
	reqBody := RouteRequest{
		Coordinates: [][]float64{{start[0], start[1]}, {end[0], end[1]}},
		Profile:     "driving-car",
	}

	body, err := json.Marshal(reqBody)
	if err != nil {
		return nil, fmt.Errorf("getRoute: error marshaling request body: %w", err)
	}

	req, err := http.NewRequest("POST", "https://api.openrouteservice.org/v2/directions/driving-car", bytes.NewBuffer(body))
	if err != nil {
		return nil, fmt.Errorf("getRoute: error creating new request: %w", err)
	}
	req.Header.Set("Authorization", "YOUR_API_KEY") // Replace with your OpenRouteService API key
	req.Header.Set("Content-Type", "application/json")

	resp, err := client.Do(req)
	if err != nil {
		return nil, fmt.Errorf("getRoute: error sending request: %w", err)
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		body, _ := io.ReadAll(resp.Body)
		return nil, fmt.Errorf("getRoute: error fetching route: %s, response: %s", resp.Status, string(body))
	}

	body, _ = io.ReadAll(resp.Body)
	var routeResponse RouteResponse
	if err := json.Unmarshal(body, &routeResponse); err != nil {
		return nil, fmt.Errorf("getRoute: error unmarshaling response: %w", err)
	}

	if len(routeResponse.Routes) > 0 {
		geometry := routeResponse.Routes[0].Geometry
		coordinates := DecodePolyline(geometry)
		return coordinates, nil
	}

	return nil, fmt.Errorf("getRoute: no routes found")
}

// DecodePolyline decodes a Google Maps encoded polyline into a slice of latitude/longitude pairs.
func DecodePolyline(encoded string) [][]float64 {
	var coords [][]float64
	var lat, lng int
	for i := 0; i < len(encoded); {
		var shift, result int
		for {
			b := int(encoded[i]) - 63
			i++
			result |= (b & 0x1f) << shift
			shift += 5
			if b < 0x20 {
				break
			}
		}
		dlat := (result >> 1) ^ -(result & 1)
		lat += dlat

		shift, result = 0, 0
		for {
			b := int(encoded[i]) - 63
			i++
			result |= (b & 0x1f) << shift
			shift += 5
			if b < 0x20 {
				break
			}
		}
		dlng := (result >> 1) ^ -(result & 1)
		lng += dlng

		coords = append(coords, []float64{float64(lat) / 1e5, float64(lng) / 1e5})
	}
	return coords
}

func postGpsPoint(points []UpsertGpsPointDto) error {
	client := &http.Client{}
	url := "http://localhost:5237/api/GpsPoint"

	body, err := json.Marshal(points) // Directly marshal the slice of points
	if err != nil {
		return fmt.Errorf("postGpsPoint: error marshaling point: %w", err)
	}

	req, err := http.NewRequest("POST", url, bytes.NewBuffer(body))
	if err != nil {
		return fmt.Errorf("postGpsPoint: error creating new request: %w", err)
	}
	req.Header.Set("Content-Type", "application/json")

	resp, err := client.Do(req)
	if err != nil {
		return fmt.Errorf("postGpsPoint: error sending request: %w", err)
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		bodyResp, _ := io.ReadAll(resp.Body)
		return fmt.Errorf("postGpsPoint: error posting GPS point: %s, response: %s", resp.Status, string(bodyResp))
	}

	return nil
}

func generateTrack(vehicleId Guid, routeCoords [][]float64, interval time.Duration) {
	startTime := time.Now()
	var points []UpsertGpsPointDto // Collect points to send in a single request

	for _, coord := range routeCoords {
		point := UpsertGpsPointDto{
			VehicleId: vehicleId,
			Timestamp: startTime.UTC(), // Ensure the timestamp is in UTC
			Latitude:  coord[1],
			Longitude: coord[0],
		}

		points = append(points, point)      // Collecting points
		startTime = startTime.Add(interval) // Update the timestamp
		// time.Sleep(interval)
	}

	// Post all points to the API
	if err := postGpsPoint(points); err != nil {
		fmt.Println("Error posting points:", err)
	} else {
		fmt.Println("Successfully posted all GPS points.")
	}
}

func main() {
	vehicleId := Guid("36ceafb2-0ac5-42b1-81fb-18681b0db151")
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
