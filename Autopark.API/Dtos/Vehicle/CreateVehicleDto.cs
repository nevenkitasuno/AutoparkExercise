using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Dtos.Vehicle
{
    public record CreateVehicleDto
    (
        [Required] string LicensePlate,
        [Range(0, double.PositiveInfinity, ErrorMessage = "Only non-negative number allowed")] decimal Price,
        [Range(Options.MinManufactureYear, Options.MaxManufactureYear, ErrorMessage = "Required valid year")] int ManufactureYear,
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")] int Mileage
    );
}