using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Entities;

namespace Autopark.API.Entities.Dtos.Brand
{
    public record UpsertBrandDto
    (
        [Required] string ManufacturerCompany,
        [Required] string ModelName,
        [Range(0, double.PositiveInfinity, ErrorMessage = "Only non-negative number allowed")] decimal EngineDisplacementLiters,
        VehicleTypes VehicleType,
        [Range(0, int.MaxValue, ErrorMessage = "Only non-negative number allowed")] int FuelTankCapacityLiters,
        [Range(0, int.MaxValue, ErrorMessage = "Only non-negative number allowed")] int SeatsCount,
        [Range(0, int.MaxValue, ErrorMessage = "Only non-negative number allowed")] int LiftWeightCapacityKg
    );
}