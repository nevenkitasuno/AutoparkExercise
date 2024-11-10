import React, { useEffect, useState } from 'react';
import { MapContainer, TileLayer, GeoJSON } from 'react-leaflet';
import axios from 'axios';
import { LatLngBounds, LatLngBoundsExpression, LatLng } from 'leaflet';

interface TrackProps {
  vehicleId: string;
  from: string;
  to: string;
  trips: any[]; // Pass selected trips
}

// Type definitions for GeoJSON
interface GeoJSONFeature {
  type: "Feature";
  geometry: {
    type: string;
    coordinates: number[];
  };
  properties: { [key: string]: any };
}

interface GeoJSONFeatureCollection {
  type: "FeatureCollection";
  features: GeoJSONFeature[];
}

const MapWithTrack: React.FC<TrackProps> = ({ vehicleId, from, to, trips }) => {
  const [geoJsonData, setGeoJsonData] = useState<GeoJSONFeatureCollection | null>(null);
  const [mapBounds, setMapBounds] = useState<LatLngBounds | null>(null);

  // Calculate the map bounds based on selected trips
  const calculateMapBounds = (trips: any[]) => {
    const latitudes: number[] = [];
    const longitudes: number[] = [];

    trips.forEach((trip) => {
      if (trip.startPoint) {
        latitudes.push(trip.startPoint.latitude);
        longitudes.push(trip.startPoint.longitude);
      }
      if (trip.endPoint) {
        latitudes.push(trip.endPoint.latitude);
        longitudes.push(trip.endPoint.longitude);
      }
    });

    if (latitudes.length > 0 && longitudes.length > 0) {
      const northEast = new LatLng(Math.max(...latitudes), Math.max(...longitudes));
      const southWest = new LatLng(Math.min(...latitudes), Math.min(...longitudes));
      return new LatLngBounds(southWest, northEast);
    }
    return null; // If no coordinates, return null
  };

  // Fetch the GPS track data when the vehicleId, trips, or date range changes
  useEffect(() => {
    const fetchGpsTrack = async () => {
      console.log('Fetching GPS track data...');
      try {
        if (trips.length === 0) {
          console.log('No trips selected');
          setGeoJsonData(null);
          setMapBounds(null);
          return;
        }

        // Initialize an empty GeoJSON feature collection with proper type
        let combinedGeoJson: GeoJSONFeatureCollection = {
          type: "FeatureCollection",
          features: [],
        };

        // Loop through each trip and append its GeoJSON data
        for (const trip of trips) {
          console.log(`Fetching GeoJSON for trip: ${trip.tripId}`);

          // Extract the 'from' and 'to' timestamps from the trip data
          const tripFrom = trip.startPoint?.timestamp;
          const tripTo = trip.endPoint?.timestamp;

          if (tripFrom && tripTo) {
            // Make the API request for the current trip using its own 'from' and 'to' values
            const response = await axios.post('http://localhost:5237/api/GpsPoint/GetTrack', {
              vehicleId,   // Vehicle ID
              from: tripFrom,  // Start timestamp for the trip
              to: tripTo,      // End timestamp for the trip
              returnGeoJson: true,  // Request GeoJSON data
            });

            // Append the fetched GeoJSON features to the combined collection
            if (response.data && response.data.features) {
              // Convert points to LineString if needed
              const lineStringFeature: GeoJSONFeature = {
                type: "Feature",
                geometry: {
                  type: "LineString",
                  coordinates: response.data.features.map((feature: GeoJSONFeature) => feature.geometry.coordinates),
                },
                properties: {}, // Add any relevant properties here
              };
              combinedGeoJson.features.push(lineStringFeature);
            }
          }
        }

        console.log('Combined GeoJSON:', combinedGeoJson);

        // Set the combined GeoJSON data
        setGeoJsonData(combinedGeoJson);

        // Calculate map bounds based on the trips
        const bounds = calculateMapBounds(trips);
        console.log('Map bounds:', bounds);
        setMapBounds(bounds);
      } catch (error) {
        console.error('Error fetching GPS track:', error);
      }
    };

    fetchGpsTrack();
  }, [vehicleId, trips]); // Dependencies (vehicleId and trips)

  if (!geoJsonData) return <div>Loading map...</div>;

  // Use mapBounds if available, otherwise default to center
  const bounds: LatLngBoundsExpression | undefined = mapBounds ? mapBounds : undefined;

  return (
    <MapContainer
      center={bounds ? bounds.getCenter() : [51.505, -0.09]} // Center on bounds
      zoom={13}
      style={{ height: '400px', width: '100%' }}
      bounds={bounds}
    >
      <TileLayer url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" />
      <GeoJSON data={geoJsonData} />
    </MapContainer>
  );
};

export default MapWithTrack;