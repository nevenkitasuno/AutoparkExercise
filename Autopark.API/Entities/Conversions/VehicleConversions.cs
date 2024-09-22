using Autopark.API.Data.Dtos.Vehicle;

namespace Autopark.API.Entities.Conversions
{
    public static class VehicleConversions
    {
        public static GetVehicleDto AsDto(this Vehicle vehicle) {
            return new GetVehicleDto
            (
                vehicle.Id,
                vehicle.LicensePlate,
                vehicle.Price,
                vehicle.ManufactureYear,
                vehicle.Mileage,
                vehicle.BrandId,
                vehicle.EnterpriseId,
                vehicle.CurrentDriverId
                // vehicle.Drivers.Select(driver => driver.Id).ToList()
            );
        }
    }
}