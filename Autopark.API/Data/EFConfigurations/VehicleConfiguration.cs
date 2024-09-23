using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autopark.API.Data.EFConfigurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasOne(v => v.CurrentDriver)
                .WithOne(d => d.CurrentVehicle)
                .HasForeignKey<Driver>(d => d.CurrentVehicleId);

            builder.HasMany(v => v.Drivers)
                .WithMany(d => d.Vehicles);
        }
    }
}