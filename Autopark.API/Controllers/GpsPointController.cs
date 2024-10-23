using System.Security.Claims;
using System.Threading.Tasks;
using Autopark.API.Data;
using Autopark.API.Entities;
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
    }
}