using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Data;
using Autopark.API.Dtos.Driver;
using Autopark.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autopark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly AutoparkDbContext _context;

        public DriverController(AutoparkDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<List<Driver>>> GetAllDriverssAsync()
        {
            var vehicles = await _context.Drivers.AsNoTracking().ToListAsync();
            return Ok(vehicles); // TODO return only IDs
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetDriverAsync))] // TODO How to get rid of it? In FreeCodeCamp course works without it
        public async Task<ActionResult<Driver>> GetDriverAsync(Guid id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null) return NotFound();
            return driver;
        }

        [HttpPost]
        public async Task<ActionResult> AddDriverAsync(UpsertDriverDto upsertDriverDto)
        {
            var driver = new Driver
            {
                FirstName = upsertDriverDto.FirstName,
                Surname = upsertDriverDto.Surname,
                Patronymic = upsertDriverDto.Patronymic,
                DateOfBirth = upsertDriverDto.DateOfBirth,
                Salary = upsertDriverDto.Salary,
                EnterpriseId = upsertDriverDto.EnterpriseId
            };
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDriverAsync), new { id = driver.Id }, driver);
        }
    }
}