using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autopark.API.Data;
using Autopark.API.Entities.Dtos.Enterprise;
using Autopark.API.Entities.Dtos.Vehicle;
using Autopark.API.Entities;
using Autopark.API.Entities.Conversions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autopark.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [Route("api/[controller]")]
    [ApiController]
    public class EnterpriseController : ControllerBase
    {
        private readonly AutoparkDbContext _context;

        public EnterpriseController(AutoparkDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<List<GetEnterpriseDto>>> GetAllEnterprisesAsync()
        {
            var loggedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var enterprises = await _context.Enterprises
                // .Where(enterprise => loggedManager.Enterprises.Select(ent => ent.Id).Contains(enterprise.Id))
                .Where(enterprise => enterprise.Managers.Select(manager => manager.Id).Contains(loggedUserId))
                .Select(enterprise => enterprise.AsDto()).ToListAsync();

            return Ok(enterprises);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetEnterpriseAsync))] // TODO How to get rid of it? In FreeCodeCamp course works without it
        public async Task<ActionResult<GetEnterpriseDto>> GetEnterpriseAsync(Guid id)
        {
            var enterprise = await _context.Enterprises.FindAsync(id);
            if (enterprise == null) return NotFound();

            var getEnterpriseDto = new GetEnterpriseDto
            (
                enterprise.Id,
                enterprise.Name,
                enterprise.City,
                enterprise.TimeZone
            );

            return getEnterpriseDto;
        }

        [HttpPost]
        public async Task<ActionResult> AddEnterpriseAsync(UpsertEnterpriseDto upsertEnterpriseDto)
        {
            var enterprise = new Enterprise
            {
                Name = upsertEnterpriseDto.Name,
                City = upsertEnterpriseDto.City,
                TimeZone = upsertEnterpriseDto.TimeZone
            };
            _context.Enterprises.Add(enterprise);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEnterpriseAsync), new { id = enterprise.Id }, enterprise.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnterpriseAsync(Guid id, UpsertEnterpriseDto upsertEnterpriseDto)
        {
            var enterprise = await _context.Enterprises.FindAsync(id);
            if (enterprise == null) return NotFound();

            enterprise.Name = upsertEnterpriseDto.Name;
            enterprise.City = upsertEnterpriseDto.City;
            enterprise.TimeZone = upsertEnterpriseDto.TimeZone;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnterpriseAsync(Guid id)
        {
            var enterprise = await _context.Enterprises.FindAsync(id);
            if (enterprise == null) return NotFound();

            _context.Enterprises.Remove(enterprise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/vehicles")]
        public async Task<ActionResult<List<GetVehicleDto>>> GetVehiclesAsync(Guid id)
        {
            var vehicles = await _context.Vehicles.Where(vehicle => vehicle.EnterpriseId == id).ToListAsync();
            return Ok(vehicles);
        }
    }
}