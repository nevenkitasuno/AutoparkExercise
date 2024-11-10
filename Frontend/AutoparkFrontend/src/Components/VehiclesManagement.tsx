import { useEffect, useState } from 'react';
import {
  Button,
  Center,
  Container,
  HStack,
  Heading,
  Input,
  VStack,
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  Select,
  Collapse,
} from '@chakra-ui/react';
import axios from 'axios';
import DrillDownVehicleDetails from './DrillDownVehicleDetails'; // Import the empty component

type Vehicle = {
  id: string;
  licensePlate: string | null;
  price: number;
  manufactureYear: number;
  mileage: number;
  brandId: number;
  enterpriseId: string | null;
  purchaseDate: string; // Adjusted purchase date
};

type Enterprise = {
  id: string;
  name: string;
  city: string;
};

type Brand = {
  id: number;
  manufacturerCompany: string;
  modelName: string;
};

type PagedResult<T> = {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
};

function VehicleManagement() {
  const [enterprises, setEnterprises] = useState<Enterprise[]>([]);
  const [brands, setBrands] = useState<Brand[]>([]);
  const [selectedEnterpriseId, setSelectedEnterpriseId] = useState<string | null>(null);
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [newVehicle, setNewVehicle] = useState<Vehicle>({
    id: '',
    licensePlate: null,
    price: 0,
    manufactureYear: new Date().getFullYear(),
    mileage: 0,
    brandId: 0,
    enterpriseId: null,
    purchaseDate: '', // Add this field
  });
  const [editingVehicle, setEditingVehicle] = useState<Vehicle | null>(null);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [expandedVehicleId, setExpandedVehicleId] = useState<string | null>(null); // Track expanded vehicle

  useEffect(() => {
    fetchEnterprises();
    fetchBrands();
  }, []);

  useEffect(() => {
    if (selectedEnterpriseId) {
      fetchVehicles();
    }
  }, [selectedEnterpriseId, pageNumber, pageSize]);

  const fetchEnterprises = async (): Promise<void> => {
    const response = await axios.get<Enterprise[]>('http://localhost:5237/api/Enterprise', { withCredentials: true });
    setEnterprises(response.data);
  };

  const fetchBrands = async (): Promise<void> => {
    const response = await axios.get<Brand[]>('http://localhost:5237/api/Brand', { withCredentials: true });
    setBrands(response.data);
  };

  const fetchVehicles = async (): Promise<void> => {
    const response = await axios.get<PagedResult<Vehicle>>(
      `http://localhost:5237/api/Vehicle?enterpriseId=${selectedEnterpriseId}&pageNumber=${pageNumber}&pageSize=${pageSize}`,
      { withCredentials: true }
    );
    setVehicles(response.data.items);
    setTotalCount(response.data.totalCount);
  };

  const deleteVehicle = async (id: string): Promise<void> => {
    await axios.delete(`http://localhost:5237/api/Vehicle/${id}`, { withCredentials: true });
    setVehicles(vehicles.filter(vehicle => vehicle.id !== id));
  };

  const toggleVehicleDetails = (vehicleId: string) => {
    setExpandedVehicleId(prev => (prev === vehicleId ? null : vehicleId));
  };

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <Container maxW="container.md" p={5}>
      <Center>
        <Heading mb={5}>Vehicle Management</Heading>
      </Center>

      {/* Enterprise Selection Table */}
      <VStack spacing={4} mb={10}>
        <Table className="min-w-full bg-white shadow-md rounded-lg overflow-hidden">
          <Thead>
            <Tr className="bg-gray-200 text-gray-700">
              <Th className="px-6 py-3 text-left">Enterprise Name</Th>
              <Th className="px-6 py-3 text-left">City</Th>
              <Th className="px-6 py-3 text-left">Actions</Th>
            </Tr>
          </Thead>
          <Tbody>
            {enterprises.map(enterprise => (
              <Tr key={enterprise.id} className="border-b hover:bg-gray-100">
                <Td className="px-6 py-4">{enterprise.name}</Td>
                <Td className="px-6 py-4">{enterprise.city}</Td>
                <Td className="px-6 py-4">
                  <Button onClick={() => setSelectedEnterpriseId(enterprise.id)} colorScheme="blue">Select</Button>
                </Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </VStack>

      {/* Vehicles Table */}
      {selectedEnterpriseId && (
        <VStack spacing={4}>
          <Table className="min-w-full bg-white shadow-md rounded-lg overflow-hidden">
            <Thead>
              <Tr className="bg-gray-200 text-gray-700">
                <Th className="px-6 py-3 text-left">License Plate</Th>
                <Th className="px-6 py-3 text-left">Price</Th>
                <Th className="px-6 py-3 text-left">Manufacture Year</Th>
                <Th className="px-6 py-3 text-left">Mileage</Th>
                <Th className="px-6 py-3 text-left">Purchase Date (Client Timezone)</Th>
                <Th className="px-6 py-3 text-left">Brand</Th>
                <Th className="px-6 py-3 text-left">Actions</Th>
              </Tr>
            </Thead>
            <Tbody>
              {vehicles.map((vehicle) => {
                const purchaseDate = new Date(vehicle.purchaseDate); // Convert to Date object
                const clientTimezonePurchaseDate = purchaseDate.toLocaleString(); // Display in client's timezone

                return (
                  <Tr key={vehicle.id} className="border-b hover:bg-gray-100">
                    <Td className="px-6 py-4">{vehicle.licensePlate}</Td>
                    <Td className="px-6 py-4">{vehicle.price}</Td>
                    <Td className="px-6 py-4">{vehicle.manufactureYear}</Td>
                    <Td className="px-6 py-4">{vehicle.mileage}</Td>
                    <Td className="px-6 py-4">{clientTimezonePurchaseDate}</Td>
                    <Td className="px-6 py-4">{brands.find(brand => brand.id === vehicle.brandId)?.manufacturerCompany}</Td>
                    <Td className="px-6 py-4">
                      <HStack spacing={2}>
                        <Button onClick={() => toggleVehicleDetails(vehicle.id)} colorScheme="blue">
                          {expandedVehicleId === vehicle.id ? 'Hide Details' : 'Show Details'}
                        </Button>
                        <Button onClick={() => setEditingVehicle(vehicle)} colorScheme="yellow">Edit</Button>
                        <Button onClick={() => deleteVehicle(vehicle.id)} colorScheme="red">Delete</Button>
                      </HStack>
                    </Td>
                  </Tr>
                );
              })}
            </Tbody>
          </Table>

          {/* Vehicle Details (Collapsible Section) */}
          {vehicles.map((vehicle) => (
            <Collapse key={vehicle.id} in={expandedVehicleId === vehicle.id}>
              <DrillDownVehicleDetails vehicleId={vehicle.id} /> {/* Pass the vehicle's ID */}
            </Collapse>
          ))}

          {/* Pagination Controls */}
          <HStack spacing={4} mt={4}>
            <Button
              onClick={() => setPageNumber(prev => Math.max(prev - 1, 1))}
              isDisabled={pageNumber === 1}
            >
              Previous
            </Button>
            <Button
              onClick={() => setPageNumber(prev => Math.min(prev + 1, totalPages))}
              isDisabled={pageNumber === totalPages}
            >
              Next
            </Button>
            <Select
              value={pageSize}
              onChange={(e) => {
                setPageSize(Number(e.target.value));
                setPageNumber(1); // Reset to first page when page size changes
              }}
            >
              <option value={10}>10</option>
              <option value={20}>20</option>
              <option value={30}>30</option>
            </Select>
          </HStack>
        </VStack>
      )}
    </Container>
  );
}

export default VehicleManagement;
