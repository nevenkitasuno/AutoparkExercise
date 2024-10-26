using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Entities.Dtos
{
    public record TrackRequestDto
    {
        public Guid VehicleId {get; set; }
        public DateTime? From {get; set; }
        public DateTime? To {get; set; }
        public bool ReturnGeoJson {get; set; } = false;
    }
}