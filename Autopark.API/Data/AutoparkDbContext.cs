using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autopark.API.Data
{
    public class AutoparkDbContext : DbContext
    {
        public AutoparkDbContext(DbContextOptions<AutoparkDbContext> options) : base(options){}
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Brand> Brands { get; set; }
    }
}