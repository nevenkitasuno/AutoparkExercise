using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Autopark.API.Data
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using AutoparkDbContext context = scope.ServiceProvider.GetRequiredService<AutoparkDbContext>();

            context.Database.Migrate();
        }

    }
}