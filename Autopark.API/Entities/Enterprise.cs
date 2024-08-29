using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Entities
{
    public class Enterprise
    {
        public Guid Id { get; set; }
        public required string City { get; set; }
        public required string Name { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public List<Driver> Drivers { get; set; } = new List<Driver>();
    }
}