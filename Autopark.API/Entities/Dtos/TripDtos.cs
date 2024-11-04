namespace Autopark.API.Entities.Dtos.GpsPoint
{
    public record UpsertTripDto
    {
        public required Guid VehicleId { get; set; }
        public required DateTime StartTimestamp { get; set; }
        public required DateTime EndTimestamp { get; set; }
    }
    public record GetTripDto : UpsertTripDto
    {
        public long Id { get; set; }
    }
}