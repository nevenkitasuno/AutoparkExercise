namespace Autopark.API.Entities.Dtos
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
    public record TripInfoDto
    {
        public GetTripDto Trip { get; set; }
        public GetGpsPointWithoutVehicleIdDto StartPoint { get; set; }
        public string StartPointAddress { get; set; }
        public GetGpsPointWithoutVehicleIdDto EndPoint { get; set; }
        public string EndPointAddress { get; set; }
    }
}