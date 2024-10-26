using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Entities.Dtos.GpsPoint
{
    public record GetGpsPointDto : UpsertGpsPointDto
    {
        public long Id { get; set; }
    }
}