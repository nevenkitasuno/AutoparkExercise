namespace Autopark.API.Entities
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int ManufactureYear { get; set; }
        public int Mileage { get; set; }
        public required string LicensePlate { get; set; }
        public long BrandId { get; set; }
        public Brand? Brand { get; set; }
        public Guid? EnterpriseId { get; set; }
        public Enterprise? Enterprise { get; set; }

        // one to one
        public Driver? CurrentDriver { get; set; }

        // many to many
        public List<Driver> Drivers { get; set; } = new List<Driver>();
    }
}