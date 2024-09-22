using Autopark.API.Data.Dtos.Driver;

namespace Autopark.API.Entities.Conversions
{
    public static class DriverConversions
    {
        public static GetDriverDto AsDto(this Driver driver) {
            return new GetDriverDto
            (
                driver.Id,
                driver.FirstName,
                driver.Surname,
                driver.Patronymic,
                driver.DateOfBirth,
                driver.Salary,
                driver.EnterpriseId,
                driver.CurrentVehicleId
            );
        }
    }
}