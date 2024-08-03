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
        [Range(1, double.PositiveInfinity, ErrorMessage = "Only positive number allowed")] decimal Price,
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")] int Year,
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")] int Mileage
    );
}