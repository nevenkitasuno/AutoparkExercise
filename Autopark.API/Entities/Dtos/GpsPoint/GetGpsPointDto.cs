using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Entities.Dtos.GpsPoint
{
    public record GetGpsPointDto : UpsertGpsPointDto
    {
        public long Id { get; set; }

        public GetGpsPointDto AsDto(Entities.GpsPoint gpsPoint)
        {
            return new GetGpsPointDto
            {
                Id = gpsPoint.Id,
                VehicleId = gpsPoint.VehicleId,
                Timestamp = gpsPoint.Timestamp,
                Latitude = gpsPoint.point.Y,
                Longitude = gpsPoint.point.X
            };
        }
    }
}