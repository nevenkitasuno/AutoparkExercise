import { useState } from "react";
import { Select, Input, Button, Box, FormControl, FormLabel, useToast, Table, Thead, Tbody, Tr, Th, Td, TableCaption } from "@chakra-ui/react";

const ReportForm = () => {
  const [reportType, setReportType] = useState("Пробег автомобиля за период");
  const [period, setPeriod] = useState(1); // 1 = Month, 0 = Day
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [licensePlate, setLicensePlate] = useState("");
  const [loading, setLoading] = useState(false);
  const [reportData, setReportData] = useState<any>(null);

  const toast = useToast();

  const handleSubmit = async () => {
    if (!startDate || !endDate || !licensePlate) {
      toast({
        title: "Error",
        description: "Please fill in all fields.",
        status: "error",
        duration: 4000,
        isClosable: true,
      });
      return;
    }

    const requestBody = {
      Period: period,
      Start: startDate,
      End: endDate,
      LicensePlate: licensePlate,
    };

    setLoading(true);
    try {
      const response = await fetch("http://localhost:5237/api/VehicleMileageReport", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(requestBody),
      });

      if (response.ok) {
        const data = await response.json();
        setReportData(data);
        toast({
          title: "Success",
          description: "Report generated successfully.",
          status: "success",
          duration: 4000,
          isClosable: true,
        });
      } else {
        toast({
          title: "Error",
          description: "Failed to fetch the report. Please try again.",
          status: "error",
          duration: 4000,
          isClosable: true,
        });
      }
    } catch (error) {
      toast({
        title: "Error",
        description: "An error occurred while fetching the report.",
        status: "error",
        duration: 4000,
        isClosable: true,
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box p={5} borderWidth={1} borderRadius="md" boxShadow="md">
      <FormControl id="report-type" mb={4}>
        <FormLabel>Choose Report Type</FormLabel>
        <Select value={reportType} onChange={(e) => setReportType(e.target.value)}>
          <option value="Пробег автомобиля за период">Пробег автомобиля за период</option>
          {/* Add more report types if needed */}
        </Select>
      </FormControl>

      {reportType === "Пробег автомобиля за период" && (
        <>
          <FormControl id="period" mb={4}>
            <FormLabel>Period</FormLabel>
            <Select value={period} onChange={(e) => setPeriod(Number(e.target.value))}>
              <option value={1}>Month</option>
              <option value={0}>Day</option>
            </Select>
          </FormControl>

          <FormControl id="start-date" mb={4}>
            <FormLabel>Start Date</FormLabel>
            <Input
              type="datetime-local"
              value={startDate}
              onChange={(e) => setStartDate(e.target.value)}
            />
          </FormControl>

          <FormControl id="end-date" mb={4}>
            <FormLabel>End Date</FormLabel>
            <Input
              type="datetime-local"
              value={endDate}
              onChange={(e) => setEndDate(e.target.value)}
            />
          </FormControl>

          <FormControl id="license-plate" mb={4}>
            <FormLabel>License Plate</FormLabel>
            <Input
              type="text"
              value={licensePlate}
              onChange={(e) => setLicensePlate(e.target.value)}
            />
          </FormControl>

          <Button colorScheme="teal" isLoading={loading} onClick={handleSubmit}>
            Generate Report
          </Button>
        </>
      )}

      {reportData && (
        <Box mt={4}>
          <h3 className="text-xl font-semibold mb-2">Report for {reportData.name}</h3>

          {/* Displaying the report in a table */}
          <Table variant="simple">
            <TableCaption>{`Mileage Report for ${reportData.name}`}</TableCaption>
            <Thead>
              <Tr>
                <Th>Date</Th>
                <Th>Mileage</Th>
              </Tr>
            </Thead>
            <Tbody>
              {Object.entries(reportData.result).map(([date, mileage]) => (
                <Tr key={date}>
                  <Td>{date}</Td>
                  <Td>{mileage as number} km</Td>
                </Tr>
              ))}
            </Tbody>
          </Table>
        </Box>
      )}
    </Box>
  );
};

export default ReportForm;
