using Autopark.API.Entities.Dtos.Vehicle;

namespace Autopark.API.Entities.Conversions
{
    public static class VehicleConversions
    {
        public static GetVehicleDto AsDto(this Vehicle vehicle, int enterpriseTimeZone = 0)
{
    // Ensure that the purchase date is in UTC
    DateTime purchaseDateUtc = vehicle.PurchaseDate.ToUniversalTime();

    // Apply time zone only if enterpriseTimeZone is provided and it's not UTC (0 offset)
    if (enterpriseTimeZone != 0)
    {
        // Get the offset for the enterprise's time zone
        var timeZoneOffset = TimeSpan.FromHours(enterpriseTimeZone);

        // Check if the purchaseDate is already in UTC (i.e., its offset should be 0)
        if (purchaseDateUtc.Kind == DateTimeKind.Utc)
        {
            // If the date is already UTC, just leave it as is
            purchaseDateUtc = new DateTimeOffset(purchaseDateUtc, TimeSpan.Zero).UtcDateTime;
        }
        else
        {
            // Otherwise, adjust to the enterprise's time zone
            var enterpriseDateTimeOffset = new DateTimeOffset(purchaseDateUtc, timeZoneOffset);
            purchaseDateUtc = enterpriseDateTimeOffset.UtcDateTime;  // Convert back to UTC after adjustment
        }
    }

    // Return the DTO with the purchase date in UTC
    return new GetVehicleDto
    (
        vehicle.Id,
        vehicle.LicensePlate,
        vehicle.Price,
        vehicle.ManufactureYear,
        vehicle.Mileage,
        vehicle.BrandId,
        vehicle.EnterpriseId,
        purchaseDateUtc // The adjusted purchase date (in UTC)
    );
}

    }
}