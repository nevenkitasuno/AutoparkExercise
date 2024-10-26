using System.Security.Claims;
using System.Threading.Tasks;
using Autopark.API.Data;
using Autopark.API.Entities;
using Autopark.API.Entities.Dtos;
using Autopark.API.Entities.Dtos.GpsPoint;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

namespace Autopark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(AuthenticationSchemes = "Identity.Application")]
    public class GpsPointController : ControllerBase
    {
        private readonly AutoparkDbContext _context;

        public GpsPointController(AutoparkDbContext context) { _context = context; }

        [HttpPost]
        public async Task AddGpsPointsAsync(IEnumerable<UpsertGpsPointDto> gpsPointsDto)
        {
            var gpsPoints = gpsPointsDto.Select(gpsPointDto => new GpsPoint
            {
                VehicleId = gpsPointDto.VehicleId,
                Timestamp = gpsPointDto.Timestamp,
                point = new Point(gpsPointDto.Longitude, gpsPointDto.Latitude)
            });

            await _context.GpsPoints.AddRangeAsync(gpsPoints);
            await _context.SaveChangesAsync();
        }

        [HttpPost("GetTrack")]
        public async Task<IActionResult> GetGpsTrack(TrackRequestDto reqParams)
        {
            var query = _context.GpsPoints.AsQueryable();

            if (reqParams.VehicleId != null)
            {
                query = query.Where(p => p.VehicleId == reqParams.VehicleId);
            }

            if (reqParams.From != null)
            {
                query = query.Where(p => p.Timestamp >= reqParams.From);
            }

            if (reqParams.To != null)
            {
                query = query.Where(p => p.Timestamp <= reqParams.To);
            }

            if (reqParams.ReturnGeoJson)
            {
                var geoJsonFeatures = query.Select(p => new
                {
                    type = "Feature",
                    geometry = new
                    {
                        type = "Point",
                        coordinates = new[] { p.point.Y, p.point.X }
                    },
                    properties = new
                    {
                        Id = p.Id,
                        Timestamp = p.Timestamp
                    }
                }).ToList();

                var geoJsonResult = new
                {
                    type = "FeatureCollection",
                    features = geoJsonFeatures
                };

                return Ok(geoJsonResult);
            }

            var result = query.Select(p => new
            {
                p.Id,
                p.Timestamp,
                Latitude = p.point.Y,
                Longitude = p.point.X
            }).ToList();

            return Ok(result);
        }
    }
}