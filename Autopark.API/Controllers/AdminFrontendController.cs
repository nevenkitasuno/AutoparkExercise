using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Data;
using Autopark.API.Dtos.AdminFrontend;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autopark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminFrontendController : ControllerBase
    {
        private readonly AutoparkDbContext _context;
        public AdminFrontendController(AutoparkDbContext context) { _context = context; }

        public async Task<ActionResult<List<GetVehicleWithManufacturerAndModelNameDto>>> GetAllVehiclesWithManufacturerAndModelNamesAsync()
        {
            var vehiclesWithBrands = _context.Vehicles.Join(_context.Brands,
                v => v.BrandId,
                b => b.Id,
                (v, b) => new GetVehicleWithManufacturerAndModelNameDto
                (
                    v.Id,
                    v.LicensePlate,
                    v.Price,
                    v.ManufactureYear,
                    v.Mileage,
                    b.ManufacturerCompany,
                    b.ModelName
                ));

            return Ok(vehiclesWithBrands);
        }
    }
}