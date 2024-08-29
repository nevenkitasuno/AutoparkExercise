using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Dtos.AdminFrontend
{
    public record GetVehicleWithManufacturerAndModelNameDto
    (
        [Required] Guid Id,
        [Required] string LicensePlate,
        [Range(0, double.PositiveInfinity, ErrorMessage = "Only non-negative number allowed")] decimal Price,
        [Range(Options.MinManufactureYear, Options.MaxManufactureYear, ErrorMessage = "Required valid year")] int ManufactureYear,
        [Range(0, int.MaxValue, ErrorMessage = "Only non-negative number allowed")] int Mileage,
        [Required] string ManufacturerCompany,
        [Required] string ModelName
    );
}