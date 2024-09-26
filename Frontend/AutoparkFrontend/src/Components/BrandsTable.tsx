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

type Brand = {
  id: number;
  manufacturerCompany: string;
  modelName: string;
  engineDisplacementLiters: number;
  fuelTankCapacityLiters: number;
  seatsCount: number;
  liftWeightCapacityKg: number;
};

function BrandsTable() {
  const [brands, setBrands] = useState<Brand[]>([]);
  const [newBrand, setNewBrand] = useState<Brand>({
    id: 0,
    manufacturerCompany: '',
    modelName: '',
    engineDisplacementLiters: 0,
    fuelTankCapacityLiters: 0,
    seatsCount: 0,
    liftWeightCapacityKg: 0,
  });
  
  const [editingBrand, setEditingBrand] = useState<Brand | null>(null);

  useEffect(() => {
    const fetchBrands = async (): Promise<void> => {
      const response = await axios.get<Brand[]>('http://localhost:5237/api/Brand');
      setBrands(response.data);
    };

    fetchBrands();
  }, []);

  const addBrand = async (): Promise<void> => {
    const response = await axios.post('http://localhost:5237/api/Brand', newBrand);
    setBrands([...brands, response.data]); // Assuming API returns the created brand
    resetNewBrand();
  };

  const updateBrand = async (id: number): Promise<void> => {
    if (editingBrand) {
      await axios.put(`http://localhost:5237/api/Brand/${id}`, editingBrand);
      setBrands(brands.map(brand => (brand.id === id ? editingBrand : brand)));
      setEditingBrand(null);
    }
  };

  const deleteBrand = async (id: number): Promise<void> => {
    await axios.delete(`http://localhost:5237/api/Brand/${id}`);
    setBrands(brands.filter(brand => brand.id !== id));
  };

  const resetNewBrand = () => {
    setNewBrand({
      id: 0,
      manufacturerCompany: '',
      modelName: '',
      engineDisplacementLiters: 0,
      fuelTankCapacityLiters: 0,
      seatsCount: 0,
      liftWeightCapacityKg: 0,
    });
  };

  return (
    <Container maxW="container.md" p={5}>
      <Center>
        <Heading mb={5}>Simple Autopark App</Heading>
      </Center>

      <VStack spacing={4}>
        <Table className="min-w-full bg-white shadow-md rounded-lg overflow-hidden">
          <Thead>
            <Tr className="bg-gray-200 text-gray-700">
              <Th className="px-6 py-3 text-left">Manufacturer Company</Th>
              <Th className="px-6 py-3 text-left">Model Name</Th>
              <Th className="px-6 py-3 text-left">Engine Displacement (L)</Th>
              <Th className="px-6 py-3 text-left">Fuel Tank Capacity (L)</Th>
              <Th className="px-6 py-3 text-left">Seats Count</Th>
              <Th className="px-6 py-3 text-left">Lift Weight Capacity (kg)</Th>
              <Th className="px-6 py-3 text-left">Actions</Th>
            </Tr>
          </Thead>
          <Tbody>
            <Tr className="border-b hover:bg-gray-100">
              <Td className="px-6 py-4">
                <Input
                  value={newBrand.manufacturerCompany}
                  onChange={(e) => setNewBrand({ ...newBrand, manufacturerCompany: e.target.value })}
                  placeholder="Manufacturer Company"
                />
              </Td>
              <Td className="px-6 py-4">
                <Input
                  value={newBrand.modelName}
                  onChange={(e) => setNewBrand({ ...newBrand, modelName: e.target.value })}
                  placeholder="Model Name"
                />
              </Td>
              <Td className="px-6 py-4">
                <Input
                  type="number"
                  value={newBrand.engineDisplacementLiters}
                  onChange={(e) => setNewBrand({ ...newBrand, engineDisplacementLiters: Number(e.target.value) })}
                  placeholder="Engine Displacement (liters)"
                />
              </Td>
              <Td className="px-6 py-4">
                <Input
                  type="number"
                  value={newBrand.fuelTankCapacityLiters}
                  onChange={(e) => setNewBrand({ ...newBrand, fuelTankCapacityLiters: Number(e.target.value) })}
                  placeholder="Fuel Tank Capacity (liters)"
                />
              </Td>
              <Td className="px-6 py-4">
                <Input
                  type="number"
                  value={newBrand.seatsCount}
                  onChange={(e) => setNewBrand({ ...newBrand, seatsCount: Number(e.target.value) })}
                  placeholder="Seats Count"
                />
              </Td>
              <Td className="px-6 py-4">
                <Input
                  type="number"
                  value={newBrand.liftWeightCapacityKg}
                  onChange={(e) => setNewBrand({ ...newBrand, liftWeightCapacityKg: Number(e.target.value) })}
                  placeholder="Lift Weight Capacity (kg)"
                />
              </Td>
              <Td className="px-6 py-4">
                <Button onClick={addBrand} colorScheme="blue">Add Brand</Button>
              </Td>
            </Tr>
            {brands.map((brand) => (
              <Tr key={brand.id} className="border-b hover:bg-gray-100">
                <Td className="px-6 py-4">
                  {editingBrand?.id === brand.id ? (
                    <Input
                      value={editingBrand.manufacturerCompany}
                      onChange={(e) => setEditingBrand({ ...editingBrand, manufacturerCompany: e.target.value })}
                      placeholder="Manufacturer Company"
                    />
                  ) : (
                    brand.manufacturerCompany
                  )}
                </Td>
                <Td className="px-6 py-4">
                  {editingBrand?.id === brand.id ? (
                    <Input
                      value={editingBrand.modelName}
                      onChange={(e) => setEditingBrand({ ...editingBrand, modelName: e.target.value })}
                      placeholder="Model Name"
                    />
                  ) : (
                    brand.modelName
                  )}
                </Td>
                <Td className="px-6 py-4">
                  {editingBrand?.id === brand.id ? (
                    <Input
                      type="number"
                      value={editingBrand.engineDisplacementLiters}
                      onChange={(e) => setEditingBrand({ ...editingBrand, engineDisplacementLiters: Number(e.target.value) })}
                      placeholder="Engine Displacement (liters)"
                    />
                  ) : (
                    brand.engineDisplacementLiters
                  )}
                </Td>
                <Td className="px-6 py-4">
                  {editingBrand?.id === brand.id ? (
                    <Input
                      type="number"
                      value={editingBrand.fuelTankCapacityLiters}
                      onChange={(e) => setEditingBrand({ ...editingBrand, fuelTankCapacityLiters: Number(e.target.value) })}
                      placeholder="Fuel Tank Capacity (liters)"
                    />
                  ) : (
                    brand.fuelTankCapacityLiters
                  )}
                </Td>
                <Td className="px-6 py-4">
                  {editingBrand?.id === brand.id ? (
                    <Input
                      type="number"
                      value={editingBrand.seatsCount}
                      onChange={(e) => setEditingBrand({ ...editingBrand, seatsCount: Number(e.target.value) })}
                      placeholder="Seats Count"
                    />
                  ) : (
                    brand.seatsCount
                  )}
                </Td>
                <Td className="px-6 py-4">
                  {editingBrand?.id === brand.id ? (
                    <Input
                      type="number"
                      value={editingBrand.liftWeightCapacityKg}
                      onChange={(e) => setEditingBrand({ ...editingBrand, liftWeightCapacityKg: Number(e.target.value) })}
                      placeholder="Lift Weight Capacity (kg)"
                    />
                  ) : (
                    brand.liftWeightCapacityKg
                  )}
                </Td>
                <Td className="px-6 py-4">
                  <HStack spacing={2}>
                    {editingBrand?.id === brand.id ? (
                      <>
                        <Button onClick={() => updateBrand(brand.id)} colorScheme="green">Save</Button>
                        <Button onClick={() => setEditingBrand(null)} colorScheme="red">Cancel</Button>
                      </>
                    ) : (
                      <Button onClick={() => setEditingBrand(brand)}>Edit</Button>
                    )}
                    <Button onClick={() => deleteBrand(brand.id)} colorScheme="red">Delete</Button>
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

export default BrandsTable;
