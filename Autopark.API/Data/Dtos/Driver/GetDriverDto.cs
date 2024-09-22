using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Data.Dtos.Driver
{
    public record GetDriverDto
    (
        [Required] Guid Id,
        [Required] string FirstName,
        [Required] string Surname,
        string? Patronymic,
        [Range(typeof(DateTime),
            Options.MinDriverBirthDate,
            Options.MaxDriverBirthDate,
            ErrorMessage = "Value for birth date {0} must be between {1} and {2}")]
            [Required] DateTime DateOfBirth,
        [Range(0, double.PositiveInfinity, ErrorMessage = "Only non-negative number allowed")] [Required] decimal Salary,
        Guid? EnterpriseId,
        Guid? CurrentVehicleId
    );
}