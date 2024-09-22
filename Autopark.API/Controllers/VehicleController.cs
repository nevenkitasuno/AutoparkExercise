using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autopark.API.Data;
using Autopark.API.Entities.Dtos.Driver;
using Autopark.API.Entities.Dtos.Vehicle;
using Autopark.API.Entities;
using Autopark.API.Entities.Conversions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autopark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    public class VehicleController : ControllerBase
    {
        private readonly AutoparkDbContext _context;

        public VehicleController(AutoparkDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<List<GetVehicleDto>>> GetAllVehiclesAsync()
        {
            var loggedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var enterpriseIds = await Manager.GetEnterpriseIdsAsync(_context, loggedUserId);

            var vehicles = await _context.Vehicles
                .Where(vehicle => vehicle.EnterpriseId.HasValue && enterpriseIds.Contains(vehicle.EnterpriseId.Value))
                // .Include("Drivers")
                .Select(vehicle => vehicle.AsDto()).ToListAsync();

            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetVehicleAsync))] // TODO How to get rid of it? In FreeCodeCamp course works without it
        public async Task<ActionResult<GetVehicleDto>> GetVehicleAsync(Guid id)
        {
            var vehicle = await _context.Vehicles.Include(v => v.Drivers).FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null) return NotFound();

            var getVehicleDto = vehicle.AsDto();

            return getVehicleDto;
        }

        [HttpPost]
        public async Task<ActionResult> AddVehicleAsync(UpsertVehicleDto upsertVehicleDto)
        {
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                Price = upsertVehicleDto.Price,
                ManufactureYear = upsertVehicleDto.ManufactureYear,
                Mileage = upsertVehicleDto.Mileage,
                LicensePlate = upsertVehicleDto.LicensePlate,
                BrandId = upsertVehicleDto.BrandId,
                EnterpriseId = upsertVehicleDto.EnterpriseId,
                CurrentDriverId = upsertVehicleDto.CurrentDrvierId
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicleAsync), new { id = vehicle.Id }, vehicle.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicleAsync(Guid id, UpsertVehicleDto upsertVehicleDto)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();

            vehicle.Price = upsertVehicleDto.Price;
            vehicle.ManufactureYear = upsertVehicleDto.ManufactureYear;
            vehicle.Mileage = upsertVehicleDto.Mileage;
            vehicle.LicensePlate = upsertVehicleDto.LicensePlate;
            vehicle.BrandId = upsertVehicleDto.BrandId;
            vehicle.EnterpriseId = upsertVehicleDto.EnterpriseId;
            vehicle.CurrentDriverId = upsertVehicleDto.CurrentDrvierId;

            // _context.Entry(vehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleAsync(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/drivers")]
        public async Task<ActionResult<List<GetDriverDto>>> GetDriversAsync(Guid id)
        {
            var drivers = await _context.Drivers
                .Where(driver => driver.Vehicles.Select(vehicle => vehicle.Id).Contains(id))
                .Select(driver => driver.AsDto()).ToListAsync();
            return Ok(drivers);
        }

        [HttpPut("{id}/drivers/{driverId}")]
        public async Task<IActionResult> AddDriverAsync(Guid id, Guid driverId)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();

            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver == null) return NotFound();

            vehicle.Drivers.Add(driver);
            driver.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}