using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Entities
{
    public class Trip
    {
        public long Id { get; set; }
        public required Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}