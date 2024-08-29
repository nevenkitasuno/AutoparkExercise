using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Data;
using Autopark.API.Dtos.Enterprise;
using Autopark.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autopark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnterpriseController : ControllerBase
    {
        private readonly AutoparkDbContext _context;

        public EnterpriseController(AutoparkDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<List<Enterprise>>> GetAllEnterprisesAsync()
        {
            var enterprises = await _context.Enterprises.AsNoTracking().ToListAsync();
            return Ok(enterprises); // TODO return only IDs
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Enterprise>> GetEnterpriseAsync(Guid id)
        {
            var enterprise = await _context.Enterprises.FindAsync(id);
            if (enterprise == null) return NotFound();
            return enterprise;
        }

        [HttpPost]
        public async Task<ActionResult> AddEnterpriseAsync(UpsertEnterpriseDto upsertEnterpriseDto)
        {
            var enterprise = new Enterprise
            {
                Name = upsertEnterpriseDto.Name,
                City = upsertEnterpriseDto.City
            };
            _context.Enterprises.Add(enterprise);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEnterpriseAsync), new { id = enterprise.Id }, enterprise);
        }
    }
}