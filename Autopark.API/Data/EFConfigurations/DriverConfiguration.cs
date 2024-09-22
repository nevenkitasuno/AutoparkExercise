using Autopark.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autopark.API.Data.EFConfigurations
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.HasOne(d => d.CurrentVehicle)
               .WithOne(v => v.CurrentDriver)
               .HasForeignKey<Vehicle>(v => v.CurrentDriverId);

            builder.HasMany(d => d.Vehicles)
               .WithMany(v => v.Drivers);
        }
    }
}