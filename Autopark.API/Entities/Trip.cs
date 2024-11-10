using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Entities.Dtos;

namespace Autopark.API.Entities
{
    public class Trip
    {
        public long Id { get; set; }
        public required Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public GetTripDto AsDto() {
            return new GetTripDto
            {
                Id = Id,
                VehicleId = VehicleId,
                StartTimestamp = Start,
                EndTimestamp = End
            };
        }
    }
}