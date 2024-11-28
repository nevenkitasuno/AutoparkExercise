namespace Autopark.API.Entities.Dtos
{

    public record RequestReportDto
    {
        public required ReportPeriod Period { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
    };

    public record RequestVehicleMileageReportDto : RequestReportDto
    {
        public required string LicensePlate { get; set; }
    }

    public record GetVehicleMileageReportDto
    {
        public required string Name { get; set; }
        public required Dictionary<DateTime, int> Result { get; set; }
    };
}