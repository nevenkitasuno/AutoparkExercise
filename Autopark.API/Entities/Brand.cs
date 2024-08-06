using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Entities
{
    public class Brand
    {
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public long Id { get; set; }
        public long Id { get; set; }
        public required string ManufacturerCompany { get; set; }
        public required string ModelName { get; set; }
        public decimal EngineDisplacementLiters { get; set; }
        public VehicleTypes VehicleType { get; set; }
        public int FuelTankCapacityLiters { get; set; }
        public int SeatsCount { get; set; }
        public int LiftWeightCapacityKg { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}