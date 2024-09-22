using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Data.Dtos.Brand;

namespace Autopark.API.Entities.Conversions
{
    public static class BrandConversions
    {
        public static GetBrandDto AsDto(this Brand brand) {
            return new GetBrandDto
            (
                brand.Id,
                brand.ManufacturerCompany,
                brand.ModelName,
                brand.EngineDisplacementLiters,
                brand.VehicleType,
                brand.FuelTankCapacityLiters,
                brand.SeatsCount,
                brand.LiftWeightCapacityKg
            );
        }
    }
}