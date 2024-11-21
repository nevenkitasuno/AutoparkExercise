using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Entities
{
    public class VehicleMileageReport : ReportBase<int>
    {
        public Guid VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public VehicleMileageReport()
        {
            Type = "Пробег автомобиля за период";
        }
    }
}