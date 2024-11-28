using Autopark.API.Data;
using Autopark.API.Entities;
using Autopark.API.Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleMileageReportController : ControllerBase
    {
        private readonly AutoparkDbContext _context;

        public VehicleMileageReportController(AutoparkDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult<GetVehicleMileageReportDto> GetVehicleMileageReport(RequestVehicleMileageReportDto request)
        {
            if (request == null)
                return BadRequest("Request cannot be null.");

            // Create the result dictionary to store the date and mileage
            var result = new Dictionary<DateTime, int>();

            // Logic based on the ReportPeriod (Day or Month)
            if (request.Period == ReportPeriod.Day)
            {
                // Generate a daily mileage report based on the given start and end dates
                var currentDate = request.Start.Date;
                while (currentDate <= request.End.Date)
                {
                    // Sample value for mileage (you can adjust the logic here)
                    result[currentDate] = new Random().Next(50, 100); // Sample mileage value

                    currentDate = currentDate.AddDays(1); // Move to the next day
                }
            }
            else if (request.Period == ReportPeriod.Month)
            {
                // Generate a monthly mileage report based on the start and end dates
                var currentMonth = request.Start.Date;
                while (currentMonth <= request.End.Date)
                {
                    // Sample value for mileage (30 times a random value)
                    var mileage = new Random().Next(50, 100) * 30; // Sample mileage multiplied by 30 for the month

                    // Set the result for the first day of each month
                    result[new DateTime(currentMonth.Year, currentMonth.Month, 1)] = mileage;

                    // Move to the next month
                    currentMonth = currentMonth.AddMonths(1);
                }
            }

            // Return the response with the mileage report
            var report = new GetVehicleMileageReportDto
            {
                Name = request.LicensePlate,
                Result = result
            };

            return Ok(report);
        }
    }
}
