namespace Autopark.API.Entities.Dtos
{
    public record GetVehicleMileageReportDto : UpsertVehicleMileageReportDto
    {
        public long Id { get; set; }
    };

    public record UpsertVehicleMileageReportDto
    {
        public required string Name { get; set; }
        public required ReportPeriod Period { get; set; }
        public required string Type { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public required Dictionary<DateTime, int> Result { get; set; }
        public required Guid VehicleId { get; set; }
    };
}
