using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Dtos.Enterprise
{
    public record GetEnterpriseDto
    (
        [Required] Guid Id,
        [Required] string City,
        [Required] string Name
    );
}