using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Dtos.Driver
{
    public record UpsertDriverDto
    (
        [Required] string FirstName,
        [Required] string Surname,
        [Required] string Patronymic,
        [Range(Options.MinDriverBirthYear, Options.MaxDriverBirthYear, ErrorMessage = "Required valid year")] [Required] DateTime DateOfBirth,
        [Range(0, double.PositiveInfinity, ErrorMessage = "Only non-negative number allowed")] [Required] decimal Salary,
        Guid EnterpriseId,
        Guid CurrentVehicleId
    );
}