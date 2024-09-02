using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Entities;

namespace Autopark.API.Dtos.Brand
{
    public record GetBrandDto
    (
        [Required] long Id, // maybe there is a way to inherit Upsert record and just add id?
        [Required] string ManufacturerCompany,
        [Required] string ModelName,
        [Range(0, double.PositiveInfinity, ErrorMessage = "Only non-negative number allowed")] decimal EngineDisplacementLiters,
        VehicleTypes VehicleType,
        [Range(0, int.MaxValue, ErrorMessage = "Only non-negative number allowed")] int FuelTankCapacityLiters,
        [Range(0, int.MaxValue, ErrorMessage = "Only non-negative number allowed")] int SeatsCount,
        [Range(0, int.MaxValue, ErrorMessage = "Only non-negative number allowed")] int LiftWeightCapacityKg
    );
}