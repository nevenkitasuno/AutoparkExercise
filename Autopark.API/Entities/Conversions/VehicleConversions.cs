using Autopark.API.Entities.Dtos.Vehicle;

namespace Autopark.API.Entities.Conversions
{
    public static class VehicleConversions
    {
        public static GetVehicleDto AsDto(this Vehicle vehicle, int enterpriseTimeZone = 0)
        {
            // var purchaseDateInEnterpriseTimezone = TimeZoneInfo.ConvertTime(vehicle.PurchaseDate, TimeZoneInfo.FindSystemTimeZoneById($"UTC{enterpriseTimeZone}"));
            var dtoffset = new DateTimeOffset(vehicle.PurchaseDate, TimeSpan.FromHours(enterpriseTimeZone)); 
            var purchaseDateInEnterpriseTimezone = dtoffset.UtcDateTime;  // Convert to UTC for consistency with other entities

            return new GetVehicleDto
            (
                vehicle.Id,
                vehicle.LicensePlate,
                vehicle.Price,
                vehicle.ManufactureYear,
                vehicle.Mileage,
                vehicle.BrandId,
                vehicle.EnterpriseId,
                purchaseDateInEnterpriseTimezone // Adjusted purchase date
            );
        }
    }
}