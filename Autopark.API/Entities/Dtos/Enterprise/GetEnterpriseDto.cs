using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Entities.Dtos.Enterprise
{
    public record GetEnterpriseDto
    (
        [Required] Guid Id,
        [Required] string Name,
        [Required] string City,
        int TimeZone
    );
}