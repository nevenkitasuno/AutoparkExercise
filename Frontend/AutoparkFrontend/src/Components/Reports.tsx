import { useState, useEffect } from "react";
import axios from "axios";
import {
    Box,
    Button,
    FormControl,
    FormLabel,
    Select,
    Input,
    useToast,
    Stack,
    Table,
    Thead,
    Tbody,
    Tr,
    Th,
    Td,
    TableCaption,
    Accordion,
    AccordionItem,
    AccordionButton,
    AccordionPanel,
    AccordionIcon,
    Text,
} from "@chakra-ui/react";
import { DateRangePicker } from "react-date-range";
import { addDays, format } from "date-fns";
import "react-date-range/dist/styles.css";
import "react-date-range/dist/theme/default.css";

enum ReportPeriod {
    Day = "Day",
    Month = "Month",
}

const Reports = () => {
    const toast = useToast();
    const [reports, setReports] = useState<any[]>([]);
    const [licensePlate, setLicensePlate] = useState<string>(""); // State for licensePlate input
    const [selectedStartDate, setSelectedStartDate] = useState<Date>(new Date());
    const [selectedEndDate, setSelectedEndDate] = useState<Date>(addDays(new Date(), 7)); // Default range of 1 week
    const [period, setPeriod] = useState<ReportPeriod>(ReportPeriod.Day);
    const [reportType, setReportType] = useState<string>("");

    // Fetch reports based on the selected filters and licensePlate
    const fetchReports = async () => {
        if (!licensePlate) {
            toast({
                title: "License plate is required",
                description: "Please enter a valid license plate.",
                status: "error",
                duration: 5000,
                isClosable: true,
            });
            return;
        }

        try {
            const response = await axios.get(`http://localhost:5237/api/VehicleMileageReport/${licensePlate}`, {
                params: {
                    type: reportType,
                    startDate: selectedStartDate.toISOString(), // Convert to ISO string (UTC)
                    endDate: selectedEndDate.toISOString(), // Convert to ISO string (UTC)
                    period: period,
                },
            });
            setReports(response.data);
        } catch (error) {
            toast({
                title: "Error fetching reports",
                description: "There was an error fetching the reports.",
                status: "error",
                duration: 5000,
                isClosable: true,
            });
        }
    };

    // Fetch reports when the component mounts or when filters change
    useEffect(() => {
        if (licensePlate) {
            fetchReports();
        }
    }, [selectedStartDate, selectedEndDate, period, reportType, licensePlate]);

    // Helper function to render Result (Dictionary<DateTime, int>)
    const renderResult = (result: { [key: string]: number }) => {
        return Object.entries(result).map(([date, mileage]) => (
            <Tr key={date}>
                <Td>{format(new Date(date), "yyyy-MM-dd")}</Td>
                <Td>{mileage}</Td>
            </Tr>
        ));
    };

    return (
        <Box p={4} bg="white" rounded="lg" shadow="md">
            <Stack spacing={4}>
                {/* License Plate Input */}
                <FormControl>
                    <FormLabel>Enter License Plate</FormLabel>
                    <Input
                        type="text"
                        value={licensePlate}
                        onChange={(e) => setLicensePlate(e.target.value)}
                        placeholder="Enter License Plate"
                    />
                </FormControl>

                {/* Report Type Select */}
                <FormControl>
                    <FormLabel>Select Report Type</FormLabel>
                    <Select
                        placeholder="Select report type"
                        value={reportType}
                        onChange={(e) => setReportType(e.target.value)}
                    >
                        <option value="Пробег автомобиля за период">Пробег автомобиля за период</option>
                        <option value="Other">Other</option>
                    </Select>
                </FormControl>

                {/* Period Select */}
                <FormControl>
                    <FormLabel>Select Period</FormLabel>
                    <Select
                        value={period}
                        onChange={(e) => setPeriod(e.target.value as ReportPeriod)}
                    >
                        <option value={ReportPeriod.Day}>Day</option>
                        <option value={ReportPeriod.Month}>Month</option>
                    </Select>
                </FormControl>

                {/* Date Range Picker */}
                <FormControl>
                    <FormLabel>Select Date Range</FormLabel>
                    <DateRangePicker
                        ranges={[
                            {
                                startDate: selectedStartDate,
                                endDate: selectedEndDate,
                                key: "selection",
                            },
                        ]}
                        onChange={(ranges) => {
                            const { startDate, endDate } = ranges.selection;
                            if (startDate instanceof Date && !isNaN(startDate.getTime())) {
                                setSelectedStartDate(startDate);
                            }
                            if (endDate instanceof Date && !isNaN(endDate.getTime())) {
                                setSelectedEndDate(endDate);
                            }
                        }}
                    />
                </FormControl>

                <Button onClick={fetchReports} colorScheme="teal">
                    Fetch Reports
                </Button>

                {/* Accordion for clickable report rows */}
                <Accordion allowToggle mt={6}>
                    {reports.map((report) => (
                        <AccordionItem key={report.id}>
                            <h2>
                                <AccordionButton>
                                    <Box flex="1" textAlign="left">
                                        <Text color="black" fontWeight="bold">
                                            {report.name}
                                        </Text>
                                        <Text color="black" fontSize="sm">
                                            {report.type} ({report.period === ReportPeriod.Day ? "Day" : "Month"})
                                        </Text>
                                        <Text color="black" fontSize="sm">
                                            {format(new Date(report.start), "yyyy-MM-dd")} - {format(new Date(report.end), "yyyy-MM-dd")}
                                        </Text>
                                    </Box>
                                    <AccordionIcon />
                                </AccordionButton>
                            </h2>
                            <AccordionPanel pb={4}>
                                <Table variant="simple">
                                    <TableCaption>Results</TableCaption>
                                    <Thead>
                                        <Tr>
                                            <Th>Date</Th>
                                            <Th>Mileage</Th>
                                        </Tr>
                                    </Thead>
                                    <Tbody>
                                        {renderResult(report.result)}
                                    </Tbody>
                                </Table>
                            </AccordionPanel>
                        </AccordionItem>
                    ))}
                </Accordion>
            </Stack>
        </Box>
    );
};

export default Reports;
