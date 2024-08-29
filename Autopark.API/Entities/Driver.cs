using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autopark.API.Entities
{
    public class Driver
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string Surname { get; set; }
        public string? Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal Salary { get; set; }
        public Guid EnterpriseId { get; set; }
        public Enterprise? Enterprise { get; set; }
        
        // one to one
        public Guid? CurrentVehicleId { get; set; }
        public Vehicle? CurrentVehicle { get; set; }
        // many to many
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}