using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Data;
using Autopark.API.Dtos.Vehicle;
using Autopark.API.Entities;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autopark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly AutoparkDbContext _context;

        public VehicleController(AutoparkDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Vehicle>>> GetAllVehiclesAsync()
        {
            var vehicles = await _context.Vehicles.AsNoTracking().ToListAsync();
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetVehicleAsync))] // TODO How to get rid of it? In FreeCodeCamp course works without it
        public async Task<ActionResult<Vehicle>> GetVehicleAsync(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
                return NotFound();

            return vehicle;
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
                LicensePlate = upsertVehicleDto.LicensePlate
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicleAsync), new { id = vehicle.Id }, vehicle);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVehicleAsync(Guid id, UpsertVehicleDto upsertVehicleDto)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();

            vehicle.Price = upsertVehicleDto.Price;
            vehicle.ManufactureYear = upsertVehicleDto.ManufactureYear;
            vehicle.Mileage = upsertVehicleDto.Mileage;
            vehicle.LicensePlate = upsertVehicleDto.LicensePlate;

            // _context.Entry(vehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteVehicleAsync(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}