using Autopark.API.Data;
using Autopark.API.Dtos.Driver;
using Autopark.API.Dtos.Vehicle;
using Autopark.API.Entities;
using Autopark.API.Entities.Conversions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autopark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly AutoparkDbContext _context;

        public DriverController(AutoparkDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<List<Driver>>> GetAllDriversAsync()
        {
            var drivers = await _context.Drivers.AsNoTracking().ToListAsync();
            return Ok(drivers.Select(driver => driver.Id));
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetDriverAsync))] // TODO How to get rid of it? In FreeCodeCamp course works without it
        public async Task<ActionResult<GetDriverDto>> GetDriverAsync(Guid id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null) return NotFound();

            var getDriverDto = new GetDriverDto
            (
                driver.Id,
                driver.FirstName,
                driver.Surname,
                driver.Patronymic,
                driver.DateOfBirth,
                driver.Salary,
                driver.EnterpriseId,
                driver.CurrentVehicleId
            );

            return Ok(getDriverDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddDriverAsync(UpsertDriverDto upsertDriverDto)
        {
            var driver = new Driver
            {
                FirstName = upsertDriverDto.FirstName,
                Surname = upsertDriverDto.Surname,
                Patronymic = upsertDriverDto.Patronymic,
                DateOfBirth = upsertDriverDto.DateOfBirth,
                Salary = upsertDriverDto.Salary,
                EnterpriseId = upsertDriverDto.EnterpriseId,
                CurrentVehicleId = upsertDriverDto.CurrentVehicleId
            };
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDriverAsync), new { id = driver.Id }, driver);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriverAsync(Guid id, UpsertDriverDto upsertDriverDto)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null) return NotFound();

            driver.FirstName = upsertDriverDto.FirstName;
            driver.Surname = upsertDriverDto.Surname;
            driver.Patronymic = upsertDriverDto.Patronymic;
            driver.DateOfBirth = upsertDriverDto.DateOfBirth;
            driver.Salary = upsertDriverDto.Salary;
            driver.EnterpriseId = upsertDriverDto.EnterpriseId;
            driver.CurrentVehicleId = upsertDriverDto.CurrentVehicleId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverAsync(Guid id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null) return NotFound();

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/vehicles")]
        public async Task<ActionResult<List<GetVehicleDto>>> GetDriversAsync(Guid id)
        {
            var vehicles = await _context.Vehicles
                .Where(vehicles => vehicles.Drivers.Select(driver => driver.Id).Contains(id))
                .Select(vehicle => vehicle.AsGetDto()).ToListAsync();
            return Ok(vehicles);
        }
    }
}