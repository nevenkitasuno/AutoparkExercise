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
} from '@chakra-ui/react';
import axios from 'axios';

type Vehicle = {
  id: string;
  licensePlate: string | null;
  price: number;
  manufactureYear: number;
  mileage: number;
  brandId: number;
  enterpriseId: string | null;
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
  });

  const [editingVehicle, setEditingVehicle] = useState<Vehicle | null>(null);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);

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

  const addVehicle = async (): Promise<void> => {
    const response = await axios.post('http://localhost:5237/api/Vehicle', { ...newVehicle, enterpriseId: selectedEnterpriseId }, { withCredentials: true });
    setVehicles([...vehicles, response.data]);
    resetNewVehicle();
  };

  const updateVehicle = async (id: string): Promise<void> => {
    if (editingVehicle) {
      await axios.put(`http://localhost:5237/api/Vehicle/${id}`, editingVehicle);
      setVehicles(vehicles.map(vehicle => (vehicle.id === id ? editingVehicle : vehicle)));
      setEditingVehicle(null);
    }
  };

  const deleteVehicle = async (id: string): Promise<void> => {
    await axios.delete(`http://localhost:5237/api/Vehicle/${id}`);
    setVehicles(vehicles.filter(vehicle => vehicle.id !== id));
  };

  const resetNewVehicle = () => {
    setNewVehicle({
      id: '',
      licensePlate: null,
      price: 0,
      manufactureYear: new Date().getFullYear(),
      mileage: 0,
      brandId: 0,
      enterpriseId: null,
    });
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
                <Th className="px-6 py-3 text-left">Brand</Th>
                <Th className="px-6 py-3 text-left">Actions</Th>
              </Tr>
            </Thead>
            <Tbody>
              <Tr className="border-b hover:bg-gray-100">
                <Td className="px-6 py-4">
                  <Input
                    value={newVehicle.licensePlate || ''}
                    onChange={(e) => setNewVehicle({ ...newVehicle, licensePlate: e.target.value || null })}
                    placeholder="License Plate"
                  />
                </Td>
                <Td className="px-6 py-4">
                  <Input
                    type="number"
                    value={newVehicle.price}
                    onChange={(e) => setNewVehicle({ ...newVehicle, price: Number(e.target.value) })}
                    placeholder="Price"
                  />
                </Td>
                <Td className="px-6 py-4">
                  <Input
                    type="number"
                    value={newVehicle.manufactureYear}
                    onChange={(e) => setNewVehicle({ ...newVehicle, manufactureYear: Number(e.target.value) })}
                    placeholder="Manufacture Year"
                  />
                </Td>
                <Td className="px-6 py-4">
                  <Input
                    type="number"
                    value={newVehicle.mileage}
                    onChange={(e) => setNewVehicle({ ...newVehicle, mileage: Number(e.target.value) })}
                    placeholder="Mileage"
                  />
                </Td>
                <Td className="px-6 py-4">
                  <Select
                    placeholder="Select Brand"
                    onChange={(e) => setNewVehicle({ ...newVehicle, brandId: Number(e.target.value) })}
                  >
                    {brands.map((brand) => (
                      <option key={brand.id} value={brand.id}>
                        {`${brand.manufacturerCompany} ${brand.modelName}`}
                      </option>
                    ))}
                  </Select>
                </Td>
                <Td className="px-6 py-4">
                  <Button onClick={addVehicle} colorScheme="blue">Add Vehicle</Button>
                </Td>
              </Tr>
              {vehicles.map((vehicle) => (
                <Tr key={vehicle.id} className="border-b hover:bg-gray-100">
                  <Td className="px-6 py-4">
                    {editingVehicle?.id === vehicle.id ? (
                      <Input
                        value={editingVehicle.licensePlate || ''}
                        onChange={(e) => setEditingVehicle({ ...editingVehicle, licensePlate: e.target.value || null })}
                        placeholder="License Plate"
                      />
                    ) : (
                      vehicle.licensePlate
                    )}
                  </Td>
                  <Td className="px-6 py-4">
                    {editingVehicle?.id === vehicle.id ? (
                      <Input
                        type="number"
                        value={editingVehicle.price}
                        onChange={(e) => setEditingVehicle({ ...editingVehicle, price: Number(e.target.value) })}
                        placeholder="Price"
                      />
                    ) : (
                      vehicle.price
                    )}
                  </Td>
                  <Td className="px-6 py-4">
                    {editingVehicle?.id === vehicle.id ? (
                      <Input
                        type="number"
                        value={editingVehicle.manufactureYear}
                        onChange={(e) => setEditingVehicle({ ...editingVehicle, manufactureYear: Number(e.target.value) })}
                        placeholder="Manufacture Year"
                      />
                    ) : (
                      vehicle.manufactureYear
                    )}
                  </Td>
                  <Td className="px-6 py-4">
                    {editingVehicle?.id === vehicle.id ? (
                      <Input
                        type="number"
                        value={editingVehicle.mileage}
                        onChange={(e) => setEditingVehicle({ ...editingVehicle, mileage: Number(e.target.value) })}
                        placeholder="Mileage"
                      />
                    ) : (
                      vehicle.mileage
                    )}
                  </Td>
                  <Td className="px-6 py-4">
                    {editingVehicle?.id === vehicle.id ? (
                      <Select
                        value={editingVehicle.brandId}
                        onChange={(e) => setEditingVehicle({ ...editingVehicle, brandId: Number(e.target.value) })}
                      >
                        {brands.map((brand) => (
                          <option key={brand.id} value={brand.id}>
                            {`${brand.manufacturerCompany} ${brand.modelName}`}
                          </option>
                        ))}
                      </Select>
                    ) : (
                      brands.find(brand => brand.id === vehicle.brandId)?.manufacturerCompany + ' ' +
                      brands.find(brand => brand.id === vehicle.brandId)?.modelName
                    )}
                  </Td>
                  <Td className="px-6 py-4">
                    <HStack spacing={2}>
                      {editingVehicle?.id === vehicle.id ? (
                        <>
                          <Button onClick={() => updateVehicle(vehicle.id)} colorScheme="green">Save</Button>
                          <Button onClick={() => setEditingVehicle(null)} colorScheme="red">Cancel</Button>
                        </>
                      ) : (
                        <Button onClick={() => setEditingVehicle(vehicle)}>Edit</Button>
                      )}
                      <Button onClick={() => deleteVehicle(vehicle.id)} colorScheme="red">Delete</Button>
                    </HStack>
                  </Td>
                </Tr>
              ))}
            </Tbody>
          </Table>

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
