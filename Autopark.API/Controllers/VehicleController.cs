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
using Autopark.API.Entities.Dtos;

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
        public async Task<ActionResult<PagedResult<GetVehicleDto>>> GetAllVehiclesAsync(int pageNumber = 1, int pageSize = 10)
        {
            var loggedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var enterpriseIds = await Manager.GetEnterpriseIdsAsync(_context, loggedUserId);

            var vehiclesQuery = _context.Vehicles
                .Where(vehicle => vehicle.EnterpriseId.HasValue && enterpriseIds.Contains(vehicle.EnterpriseId.Value))
                .Select(vehicle => vehicle.AsDto());

            var totalCount = await vehiclesQuery.CountAsync();
            var vehicles = await vehiclesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedResult = new PagedResult<GetVehicleDto>
            {
                Items = vehicles,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Ok(pagedResult);
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
            bool isAuthorized = await IsAuthorizedUpsertAsync(upsertVehicleDto);
            if (!isAuthorized) return Unauthorized();
            
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                Price = upsertVehicleDto.Price,
                ManufactureYear = upsertVehicleDto.ManufactureYear,
                Mileage = upsertVehicleDto.Mileage,
                LicensePlate = upsertVehicleDto.LicensePlate,
                BrandId = upsertVehicleDto.BrandId,
                EnterpriseId = upsertVehicleDto.EnterpriseId,
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicleAsync), new { id = vehicle.Id }, vehicle.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicleAsync(Guid id, UpsertVehicleDto upsertVehicleDto)
        {
            bool isAuthorized = await IsAuthorizedUpsertAsync(upsertVehicleDto);
            if (!isAuthorized) return Unauthorized();

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();

            vehicle.Price = upsertVehicleDto.Price;
            vehicle.ManufactureYear = upsertVehicleDto.ManufactureYear;
            vehicle.Mileage = upsertVehicleDto.Mileage;
            vehicle.LicensePlate = upsertVehicleDto.LicensePlate;
            vehicle.BrandId = upsertVehicleDto.BrandId;
            vehicle.EnterpriseId = upsertVehicleDto.EnterpriseId;

            // _context.Entry(vehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleAsync(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();

            var loggedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var enterpriseIds = await Manager.GetEnterpriseIdsAsync(_context, loggedUserId);
            if (!enterpriseIds.Contains(vehicle.EnterpriseId.Value)) return Unauthorized();

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

        private async Task<bool> IsAuthorizedUpsertAsync(UpsertVehicleDto upsertVehicleDto) {
            var loggedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var enterpriseIds = await Manager.GetEnterpriseIdsAsync(_context, loggedUserId);
            return enterpriseIds.Contains(upsertVehicleDto.EnterpriseId.Value);
        }
    }
}