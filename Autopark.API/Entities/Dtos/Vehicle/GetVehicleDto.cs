using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Entities.Dtos.Vehicle
{
    public record GetVehicleDto
    (
        [Required] Guid Id,
        [Required] string LicensePlate,
        [Range(0, double.PositiveInfinity, ErrorMessage = "Only non-negative number allowed")] decimal Price,
        [Range(Options.MinManufactureYear, Options.MaxManufactureYear, ErrorMessage = "Required valid year")] int ManufactureYear,
        [Range(0, int.MaxValue, ErrorMessage = "Only non-negative number allowed")] int Mileage,
        [Range(0, long.MaxValue, ErrorMessage = "Only non-negative number allowed")] long BrandId,
        Guid? EnterpriseId,
        DateTime PurchaseDate
    );
}