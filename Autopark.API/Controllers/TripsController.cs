using Autopark.API.Data;
using Autopark.API.Entities;
using Autopark.API.Entities.Dtos;
using Autopark.API.Entities.Dtos.GpsPoint;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autopark.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly AutoparkDbContext _context;

        public TripsController(AutoparkDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Trip>> PostTrip([FromBody] UpsertTripDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            var trip = new Trip
            {
                VehicleId = dto.VehicleId,
                Start = dto.StartTimestamp,
                End = dto.EndTimestamp
            };

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrip), new { id = trip.Id }, trip);
        }

        [HttpPost("gps-points")]
        public async Task<ActionResult<IEnumerable<GpsPoint>>> GetGpsPointsInRange(TrackRequestDto reqParams)
        {
            // Retrieve trips that match the criteria
            var trips = await _context.Trips
                .Where(t => t.Start >= reqParams.From
                             && t.End <= reqParams.To
                             && t.VehicleId == reqParams.VehicleId)
                .ToListAsync();

            if (!trips.Any())
            {
                return Ok(new List<GpsPoint>());
            }

            // Load all GPS points for the vehicle
            var gpsPoints = await _context.GpsPoints
                .Where(g => g.VehicleId == reqParams.VehicleId)
                .ToListAsync();

            // Filter GPS points based on the trip time ranges in memory
            var filteredGpsPoints = gpsPoints
                .Where(g => trips.Any(t => g.Timestamp >= t.Start && g.Timestamp <= t.End))
                .Select(p => new
                {
                    p.Id,
                    p.Timestamp,
                    Latitude = p.point.Y,
                    Longitude = p.point.X
                })
                .ToList();

            if (reqParams.ReturnGeoJson)
            {
                var geoJsonFeatures = filteredGpsPoints.Select(p => new
                {
                    type = "Feature",
                    geometry = new
                    {
                        type = "Point",
                        coordinates = new[] { p.Longitude, p.Latitude }
                    },
                    properties = new
                    {
                        Id = p.Id,
                        Timestamp = p.Timestamp
                    }
                });

                var geoJsonResult = new
                {
                    type = "FeatureCollection",
                    features = geoJsonFeatures
                };

                return Ok(geoJsonResult);
            }

            return Ok(filteredGpsPoints);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Trip>> GetTrip(long id)
        {
            var trip = await _context.Trips.FindAsync(id);

            if (trip == null)
            {
                return NotFound();
            }

            return trip;
        }
    }
}
