using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}