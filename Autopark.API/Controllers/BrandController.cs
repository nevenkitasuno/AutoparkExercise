using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Data;
using Autopark.API.Entities;
using Autopark.API.Dtos.Brand;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autopark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly AutoparkDbContext _context;

        public BrandController(AutoparkDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<List<Brand>>> GetAllBrandsAsync()
        {
            var brands = await _context.Brands.AsNoTracking().ToListAsync();
            return Ok(brands.Select(brand => brand.Id));
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetBrandAsync))] // TODO Get rid
        public async Task<ActionResult<Brand>> GetBrandAsync(Guid id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) return NotFound();
            return brand;
        }

        [HttpPost]
        public async Task<ActionResult> AddBrandAsync(UpsertBrandDto upsertBrandDto)
        {
            var brand = new Brand
            {
                ManufacturerCompany = upsertBrandDto.ManufacturerCompany,
                ModelName = upsertBrandDto.ModelName,
                EngineDisplacementLiters = upsertBrandDto.EngineDisplacementLiters,
                VehicleType = upsertBrandDto.VehicleType,
                FuelTankCapacityLiters = upsertBrandDto.FuelTankCapacityLiters,
                SeatsCount = upsertBrandDto.SeatsCount,
                LiftWeightCapacityKg = upsertBrandDto.LiftWeightCapacityKg
            };

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBrandAsync), new { id = brand.Id }, brand);
        }
    }
}