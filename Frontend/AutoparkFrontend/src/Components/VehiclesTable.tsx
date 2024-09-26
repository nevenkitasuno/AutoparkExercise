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
} from '@chakra-ui/react';
import axios from 'axios';
import Cookies from 'js-cookie';

type Vehicle = {
  id: string;
  licensePlate: string | null;
  price: number;
  manufactureYear: number;
  mileage: number;
  brandId: number;
  enterpriseId: string | null;
};

function VehiclesTable() {
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

  useEffect(() => {
    const fetchVehicles = async (): Promise<void> => {
      const response = await axios.get<Vehicle[]>('http://localhost:5237/api/Vehicle', { withCredentials: true });
      setVehicles(response.data);
      // const response = await fetch('http://localhost:5237/api/Vehicle', {
      //   method: 'GET',
      //   headers: {
      //     'Content-Type': 'application/json',
      //   },
      //   credentials: 'include',
      // });
    };

    fetchVehicles();
  }, []);

  const addVehicle = async (): Promise<void> => {
    const response = await axios.post('http://localhost:5237/api/Vehicle', newVehicle, { withCredentials: true });
    setVehicles([...vehicles, response.data]); // Assuming API returns the created vehicle
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

  return (
    <Container maxW="container.md" p={5}>
      <Center>
        <Heading mb={5}>Vehicle Management</Heading>
      </Center>

      <VStack spacing={4}>
        <Table className="min-w-full bg-white shadow-md rounded-lg overflow-hidden">
          <Thead>
            <Tr className="bg-gray-200 text-gray-700">
              <Th className="px-6 py-3 text-left">License Plate</Th>
              <Th className="px-6 py-3 text-left">Price</Th>
              <Th className="px-6 py-3 text-left">Manufacture Year</Th>
              <Th className="px-6 py-3 text-left">Mileage</Th>
              <Th className="px-6 py-3 text-left">Brand ID</Th>
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
                <Input
                  type="number"
                  value={newVehicle.brandId}
                  onChange={(e) => setNewVehicle({ ...newVehicle, brandId: Number(e.target.value) })}
                  placeholder="Brand ID"
                />
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
                    <Input
                      type="number"
                      value={editingVehicle.brandId}
                      onChange={(e) => setEditingVehicle({ ...editingVehicle, brandId: Number(e.target.value) })}
                      placeholder="Brand ID"
                    />
                  ) : (
                    vehicle.brandId
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
      </VStack>
    </Container>
  );
}

export default VehiclesTable;
