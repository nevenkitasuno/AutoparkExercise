using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Autopark.API.Entities
{
    public class Manager : IdentityUser
    {
        public List<Enterprise> Enterprises { get; set; } = new List<Enterprise>();
    }
}