using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autopark.API.Entities.Dtos;
using Autopark.API.Entities;
using System.Security.Claims;
using Autopark.API.Data;

namespace Autopark.API.Controllers
{
    // [Authorize(AuthenticationSchemes = "Identity.Application")]
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleMileageReportController : ControllerBase
    {
        private readonly AutoparkDbContext _context;

        public VehicleMileageReportController(AutoparkDbContext context)
        {
            _context = context;
        }

        // GET: api/VehicleMileageReport/{vehicleId}
        [HttpGet("{licensePlate}")]
        public async Task<ActionResult<List<GetVehicleMileageReportDto>>> GetVehicleMileageReportsAsync(
            string licensePlate,
            [FromQuery] string type,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] ReportPeriod period)
        {
            var vehicle = await _context.Vehicles
                .Where(v => v.LicensePlate == licensePlate)
                .FirstOrDefaultAsync();
            // Fetch reports for the specified vehicle and within the given date range
            var reports = await _context.VehicleMileageReports
                .Where(report => report.VehicleId == vehicle.Id)
                .Where(report => report.Type == type)
                .Where(report => report.Start >= startDate && report.End <= endDate)
                .Where(report => report.Period == period)
                .Select(report => new GetVehicleMileageReportDto
                {
                    Id = report.Id,
                    Name = report.Name,
                    Period = report.Period,
                    Type = report.Type,
                    Start = report.Start,
                    End = report.End,
                    Result = report.Result,
                    VehicleId = report.VehicleId
                })
                .ToListAsync();

            if (reports.Count == 0)
            {
                return NotFound("No mileage reports found for the specified criteria.");
            }

            return Ok(reports);
        }

        // POST: api/VehicleMileageReport
        [HttpPost]
        public async Task<ActionResult<GetVehicleMileageReportDto>> CreateVehicleMileageReportAsync(
            UpsertVehicleMileageReportDto newReport)
        {
            // Check if the vehicle exists (optional, based on the application needs)
            var vehicle = await _context.Vehicles.FindAsync(newReport.VehicleId);
            if (vehicle == null)
            {
                return NotFound("Vehicle not found.");
            }

            // Create the new mileage report
            var report = new VehicleMileageReport
            {
                Name = newReport.Name,
                Period = newReport.Period,
                Type = newReport.Type,
                Start = newReport.Start,
                End = newReport.End,
                Result = newReport.Result, // Assuming this is a Dictionary<DateTime, int>
                VehicleId = newReport.VehicleId
            };

            _context.VehicleMileageReports.Add(report);
            await _context.SaveChangesAsync();

            // Return the created report with its ID
            return CreatedAtAction(nameof(GetVehicleMileageReportsAsync), new { vehicleId = report.VehicleId },
                new GetVehicleMileageReportDto
                {
                    Id = report.Id,
                    Name = report.Name,
                    Period = report.Period,
                    Type = report.Type,
                    Start = report.Start,
                    End = report.End,
                    Result = report.Result,
                    VehicleId = report.VehicleId
                });
        }

        // PUT: api/VehicleMileageReport/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicleMileageReportAsync(long id, UpsertVehicleMileageReportDto updatedReport)
        {
            var report = await _context.VehicleMileageReports.FindAsync(id);
            if (report == null)
            {
                return NotFound("Mileage report not found.");
            }

            // Update the report's fields
            report.Name = updatedReport.Name;
            report.Period = updatedReport.Period;
            report.Type = updatedReport.Type;
            report.Start = updatedReport.Start;
            report.End = updatedReport.End;
            report.Result = updatedReport.Result;

            await _context.SaveChangesAsync();

            return NoContent(); // No content to return after successful update
        }

        // DELETE: api/VehicleMileageReport/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleMileageReportAsync(long id)
        {
            var report = await _context.VehicleMileageReports.FindAsync(id);
            if (report == null)
            {
                return NotFound("Mileage report not found.");
            }

            _context.VehicleMileageReports.Remove(report);
            await _context.SaveChangesAsync();

            return NoContent(); // No content to return after successful deletion
        }

        [HttpPost("GenerateSampleReport/{vehicleId}")]
        public async Task<ActionResult<GetVehicleMileageReportDto>> GenerateSampleReportAsync(Guid vehicleId)
        {
            // Check if the vehicle exists
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null)
            {
                return NotFound("Vehicle not found.");
            }

            // Create random sample data for the report
            var random = new Random();
            var startDate = DateTime.Now.AddDays(-30).ToUniversalTime(); // Start 30 days ago (convert to UTC)
            var endDate = DateTime.Now.ToUniversalTime(); // End today (convert to UTC)

            // Create a random report name
            var reportName = "Sample Report " + random.Next(1, 1000).ToString();

            // Randomly choose the report type
            var reportTypes = new[] { "Пробег автомобиля за период" };
            var reportType = reportTypes[random.Next(reportTypes.Length)];

            // Randomly choose the report period (Day or Month)
            var reportPeriod = random.Next(2) == 0 ? ReportPeriod.Day : ReportPeriod.Month;

            // Generate random result data (a dictionary of DateTime -> int values)
            var result = new Dictionary<DateTime, int>();
            for (int i = 0; i < 7; i++) // Generate mileage for the past 7 days
            {
                var day = startDate.AddDays(i);
                result[day] = random.Next(50, 300); // Random mileage between 50 and 300
            }

            // Create the new mileage report with the generated data
            var newReport = new UpsertVehicleMileageReportDto
            {
                VehicleId = vehicleId,
                Name = reportName,
                Period = reportPeriod,
                Type = reportType,
                Start = startDate,
                End = endDate,
                Result = result // Pass the random result dictionary
            };

            // Create the new report entity
            var report = new VehicleMileageReport
            {
                Name = newReport.Name,
                Period = newReport.Period,
                Type = newReport.Type,
                Start = newReport.Start,
                End = newReport.End,
                Result = newReport.Result,
                VehicleId = newReport.VehicleId
            };

            // Save the new report in the database
            _context.VehicleMileageReports.Add(report);
            await _context.SaveChangesAsync();

            // Return the created report with its ID
            return CreatedAtAction(nameof(GetVehicleMileageReportsAsync), new { vehicleId = report.VehicleId },
                new GetVehicleMileageReportDto
                {
                    Id = report.Id,
                    Name = report.Name,
                    Period = report.Period,
                    Type = report.Type,
                    Start = report.Start,
                    End = report.End,
                    Result = report.Result,
                    VehicleId = report.VehicleId
                });
        }
    }
}
