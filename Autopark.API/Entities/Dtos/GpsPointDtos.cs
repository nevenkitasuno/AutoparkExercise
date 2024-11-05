using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Entities.Dtos
{
    public record GetGpsPointDto : UpsertGpsPointDto
    {
        public long Id { get; set; }
    };

    public record UpsertGpsPointDto
    {
        public required Guid VehicleId { get; set; }
        public required DateTime Timestamp { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
    };

    public record GetGpsPointWithoutVehicleIdDto
    {
        public long Id { get; set; }

        public required DateTime Timestamp { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
    }
}