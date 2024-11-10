import React, { useEffect, useState } from 'react';
import { Button, HStack, VStack, Checkbox, Table, Thead, Tbody, Tr, Th, Td } from '@chakra-ui/react';
import MapWithTrack from './MapWithTrack'; // import the Map component
import axios from 'axios';

interface Trip {
  tripId: number;
  startPoint: { timestamp: string; latitude: number; longitude: number } | null;
  endPoint: { timestamp: string; latitude: number; longitude: number } | null;
  startPointAddress: string;
  endPointAddress: string;
}

interface DrillDownVehicleDetailsProps {
  vehicleId: string;
}

const DrillDownVehicleDetails: React.FC<DrillDownVehicleDetailsProps> = ({ vehicleId }) => {
  const [trips, setTrips] = useState<Trip[]>([]);
  const [startDate, setStartDate] = useState<string>('');
  const [endDate, setEndDate] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);
  const [selectedTrips, setSelectedTrips] = useState<Set<number>>(new Set()); // Track selected trips

  useEffect(() => {
    // Fetch trips when vehicleId or date range changes
    if (vehicleId) {
      fetchTrips();
    }
  }, [vehicleId, startDate, endDate]);

  const fetchTrips = async () => {
    if (!vehicleId) return;

    setLoading(true);

    try {
      let url = `http://localhost:5237/api/Vehicle/${vehicleId}/trips?`;

      if (startDate) {
        url += `startDate=${startDate}&`;
      }

      if (endDate) {
        url += `endDate=${endDate}`;
      }

      const response = await axios.get(url, { withCredentials: true });

      setTrips(response.data);
    } catch (error) {
      console.error('Error fetching trips:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleStartDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setStartDate(e.target.value);
  };

  const handleEndDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEndDate(e.target.value);
  };

  const toggleTripSelection = (tripId: number) => {
    const updatedSelection = new Set(selectedTrips);
    if (updatedSelection.has(tripId)) {
      updatedSelection.delete(tripId);
    } else {
      updatedSelection.add(tripId);
    }
    setSelectedTrips(updatedSelection);
  };

  const renderTimestamp = (timestamp: string | null) => {
    if (timestamp) {
      return new Date(timestamp).toLocaleString();
    }
    return 'Timestamp not available';
  };

  const selectedTripsArray = Array.from(selectedTrips);

  // Filter trips based on the selected trips
  const filteredTrips = trips.filter((trip) => selectedTrips.has(trip.tripId));

  // Find the earliest start time and the latest end time for the selected trips
  const getMapDateRange = () => {
    if (filteredTrips.length === 0) return { from: '', to: '' };

    const sortedByStart = filteredTrips.sort(
      (a, b) => new Date(a.startPoint?.timestamp || '').getTime() - new Date(b.startPoint?.timestamp || '').getTime()
    );
    const sortedByEnd = filteredTrips.sort(
      (a, b) => new Date(b.endPoint?.timestamp || '').getTime() - new Date(a.endPoint?.timestamp || '').getTime()
    );

    const from = sortedByStart[0].startPoint?.timestamp || '';
    const to = sortedByEnd[0].endPoint?.timestamp || '';

    return { from, to };
  };

  const { from, to } = getMapDateRange();

  return (
    <VStack spacing={4}>
      <Table variant="simple">
        <Thead>
          <Tr>
            <Th>Select</Th>
            <Th>Trip ID</Th>
            <Th>Start Point</Th>
            <Th>End Point</Th>
            <Th>Start Address</Th>
            <Th>End Address</Th>
          </Tr>
        </Thead>
        <Tbody>
          {trips.map((trip) => (
            <Tr key={trip.tripId}>
              <Td>
                <Checkbox
                  isChecked={selectedTrips.has(trip.tripId)}
                  onChange={() => toggleTripSelection(trip.tripId)}
                />
              </Td>
              <Td>{trip.tripId}</Td>
              <Td>
                {trip.startPoint && trip.startPoint.timestamp
                  ? new Date(trip.startPoint.timestamp).toLocaleString()
                  : 'Timestamp not available'}
              </Td>
              <Td>
                {trip.endPoint && trip.endPoint.timestamp
                  ? new Date(trip.endPoint.timestamp).toLocaleString()
                  : 'Timestamp not available'}
              </Td>
              <Td>{trip.startPointAddress}</Td>
              <Td>{trip.endPointAddress}</Td>
            </Tr>
          ))}
        </Tbody>
      </Table>

      {/* Pass all selected trips to MapWithTrack */}
      {selectedTripsArray.length > 0 && (
        <MapWithTrack
          key={selectedTripsArray.join('-')}  // Force re-render by changing key
          vehicleId={vehicleId}
          from={new Date(from).toISOString()}
          to={new Date(to).toISOString()}
          trips={filteredTrips} // Pass the filtered trips to the MapWithTrack component
        />
      )}
    </VStack>
  );
};

export default DrillDownVehicleDetails;
