using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Entities.Dtos.GpsPoint
{
    public record UpsertGpsPointDto
    {
        public required Guid VehicleId {get; set; }
        public required DateTime Timestamp {get; set; }
        public required double Latitude {get; set; }
        public required double Longitude {get; set; }
    };
}