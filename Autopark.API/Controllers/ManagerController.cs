using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autopark.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    public class ManagerController : ControllerBase
    {
        private readonly AutoparkDbContext _context;

        public ManagerController(AutoparkDbContext context) { _context = context; }

        [HttpPut("{id}/enterprises/{enterpriseId}")]
        public async Task<IActionResult> AddDriverAsync(string id, Guid enterpriseId)
        {
            var manager = await _context.Users.FindAsync(id);
            if (manager == null) return NotFound();

            var enterprise = await _context.Enterprises.FindAsync(enterpriseId);
            if (enterprise == null) return NotFound();

            manager.Enterprises.Add(enterprise);
            enterprise.Managers.Add(manager);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("StaticLoggedUser")]
        public async Task<ActionResult<String>> GetLoggedUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(userId);
        }
    }
}