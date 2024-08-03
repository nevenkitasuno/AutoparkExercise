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
        public async Task<ActionResult> AddVehicleAsync(CreateVehicleDto createVehicleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                Price = createVehicleDto.Price,
                Year = createVehicleDto.Year,
                Mileage = createVehicleDto.Mileage,
                LicensePlate = createVehicleDto.LicensePlate
            };
            
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicleAsync), new {id = vehicle.Id}, vehicle);
        }

        // [HttpPost]
        // public async Task<ActionResult> PostAsync([FromBody] CreateVehicleDto createItemDto)
        // {
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     var vehicle = new VehicleEntity
        //     {
        //         Id = Guid.NewGuid(),
        //         LicensePlate = createItemDto.LicensePlate,
        //         Price = createItemDto.Price,
        //         Year = createItemDto.Year,
        //         Mileage = createItemDto.Mileage
        //     };

        //     db.Vehicle.Add(vehicle);
        //     await db.SaveChangesAsync();

        //     return CreatedAtAction(nameof(GetByIdAsync), new { id = vehicle.Id }, vehicle);
        // }
    }
}