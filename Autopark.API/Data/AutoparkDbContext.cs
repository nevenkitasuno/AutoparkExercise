using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Data.EFConfigurations;
using Autopark.API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Autopark.API.Data
{
    public class AutoparkDbContext : IdentityDbContext<Manager>
    {
        public AutoparkDbContext(DbContextOptions<AutoparkDbContext> options) : base(options){}
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Enterprise> Enterprises { get; set; }
        public DbSet<GpsPoint> GpsPoints { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<VehicleMileageReport> VehicleMileageReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");

            modelBuilder.ApplyConfiguration(new DriverConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleConfiguration());

            modelBuilder.HasDefaultSchema("identity");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VehicleMileageReport>()
                .Property(b => b.Result)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Dictionary<DateTime, int>>(v));
        }
    }
}