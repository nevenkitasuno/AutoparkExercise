using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Entities.Dtos;
using NetTopologySuite.Geometries;

namespace Autopark.API.Entities
{
    public class GpsPoint
    {
        public long Id { get; set; }
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime Timestamp { get; set; }
        public Point point { get; set; }

        public GetGpsPointDto AsDto()
        {
            return new GetGpsPointDto
            {
                Id = Id,
                VehicleId = VehicleId,
                Timestamp = Timestamp,
                Latitude = point.Y,
                Longitude = point.X
            };
        }

        public GetGpsPointWithoutVehicleIdDto AsDtoWithoutVehicleId()
        {
            return new GetGpsPointWithoutVehicleIdDto
            {
                Id = Id,
                Timestamp = Timestamp,
                Latitude = point.Y,
                Longitude = point.X
            };
        }
    }   
}