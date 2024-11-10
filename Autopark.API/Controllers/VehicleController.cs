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
                .Where(vehicle => vehicle.EnterpriseId.HasValue && enterpriseIds.Contains(vehicle.EnterpriseId.Value));

            var totalCount = await vehiclesQuery.CountAsync();
            var vehicles = await vehiclesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Fetch the timezone for the first vehicle's enterprise if they are all from the same enterprise
            var enterpriseTimeZone = await _context.Enterprises
                .Where(e => e.Id == vehicles.First().EnterpriseId)
                .Select(e => e.TimeZone)
                .FirstOrDefaultAsync();

            var pagedResult = new PagedResult<GetVehicleDto>
            {
                Items = vehicles.Select(vehicle => vehicle.AsDto(enterpriseTimeZone)).ToList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetVehicleDto>> GetVehicleAsync(Guid id)
        {
            var vehicle = await _context.Vehicles.Include(v => v.Drivers).FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null) return NotFound();

            var enterpriseTimeZone = await _context.Enterprises
                .Where(e => e.Id == vehicle.EnterpriseId)
                .Select(e => e.TimeZone)
                .FirstOrDefaultAsync();

            var getVehicleDto = vehicle.AsDto(enterpriseTimeZone); // Use the updated AsDto method

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
                PurchaseDate = upsertVehicleDto.PurchaseDate
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicleAsync), new { id = vehicle.Id });
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
            vehicle.PurchaseDate = upsertVehicleDto.PurchaseDate;

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

        private async Task<bool> IsAuthorizedUpsertAsync(UpsertVehicleDto upsertVehicleDto)
        {
            var loggedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var enterpriseIds = await Manager.GetEnterpriseIdsAsync(_context, loggedUserId);
            return enterpriseIds.Contains(upsertVehicleDto.EnterpriseId.Value);
        }

        // GET /api/vehicles/{id}/trips?startDate=2024-01-01T00:00:00Z&endDate=2024-01-31T23:59:59Z
        [AllowAnonymous]
        [HttpGet("{id}/trips")]
        public async Task<IActionResult> GetTripsByVehicleId(Guid id, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            // If startDate or endDate are provided, ensure they're in UTC
            if (startDate.HasValue && startDate.Value.Kind != DateTimeKind.Utc)
            {
                startDate = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
            }

            if (endDate.HasValue && endDate.Value.Kind != DateTimeKind.Utc)
            {
                endDate = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
            }

            // Build the initial query to get trips by vehicle ID
            var query = _context.Trips.AsQueryable();

            // Apply start date filter if provided
            if (startDate.HasValue)
            {
                query = query.Where(t => t.Start >= startDate.Value);
            }

            // Apply end date filter if provided
            if (endDate.HasValue)
            {
                query = query.Where(t => t.End <= endDate.Value);
            }

            // Fetch trips from the database
            var trips = await query.Where(t => t.VehicleId == id).ToListAsync();

            if (!trips.Any())
            {
                return Ok(new List<TripInfoDto>());
            }

            var res = new List<TripInfoDto>();

            var token = "API_KEY";
            var api = new Dadata.SuggestClientAsync(token);

            foreach (Trip trip in trips)
            {
                var tripInfoDto = new TripInfoDto();

                tripInfoDto.TripId = trip.Id;

                var gpsPoints = await _context.GpsPoints
                    .Where(g => g.VehicleId == id && g.Timestamp >= trip.Start && g.Timestamp <= trip.End)
                    .OrderBy(p => p.Timestamp)
                    .ToListAsync();

                if (gpsPoints.Any())
                {
                    tripInfoDto.StartPoint = gpsPoints.FirstOrDefault().AsDtoWithoutVehicleId();
                    var response = await api.Geolocate(lat: tripInfoDto.StartPoint.Latitude, lon: tripInfoDto.StartPoint.Longitude, radius_meters: 50);
                    if (response.suggestions.Any()) tripInfoDto.StartPointAddress = response.suggestions[0].value;

                    tripInfoDto.EndPoint = gpsPoints.LastOrDefault().AsDtoWithoutVehicleId();
                    response = await api.Geolocate(lat: tripInfoDto.EndPoint.Latitude, lon: tripInfoDto.EndPoint.Longitude, radius_meters: 50);
                    if (response.suggestions.Any()) tripInfoDto.EndPointAddress = response.suggestions[0].value;
                }

                res.Add(tripInfoDto);
            }

            return Ok(res);
        }
    }
}