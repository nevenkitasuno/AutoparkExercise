using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Entities
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int ManufactureYear { get; set; }
        public int Mileage { get; set; }
        public required string LicensePlate { get; set; }
        public long BrandId { get; set; }
        public Brand? Brand { get; set; }
    }
}