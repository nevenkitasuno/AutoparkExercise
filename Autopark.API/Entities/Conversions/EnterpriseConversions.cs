using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Entities.Dtos.Enterprise;

namespace Autopark.API.Entities.Conversions
{
    public static class EnterpriseConversions
    {
        public static GetEnterpriseDto AsDto(this Enterprise enterprise) {
            return new GetEnterpriseDto
            (
                enterprise.Id,
                enterprise.Name,
                enterprise.City
            );
        }
    }
}