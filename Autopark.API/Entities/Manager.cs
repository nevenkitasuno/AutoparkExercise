using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autopark.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Autopark.API.Entities
{
    public class Manager : IdentityUser
    {
        public List<Enterprise> Enterprises { get; set; } = new List<Enterprise>();

        public static async Task<List<Guid>> GetEnterpriseIdsAsync(AutoparkDbContext context, string loggedUserId)
        {
            var enterpriseIds = await context.Users
                    .Where(manager => manager.Id == loggedUserId)
                    .SelectMany(manager => manager.Enterprises)
                    .Select(enterprise => enterprise.Id)
                    .ToListAsync();

            return enterpriseIds;
        }
    }
}