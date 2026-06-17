using HealthCare.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace HealthCare.Api.Data
{
    public class RoleSeeder
    {
        public static async Task SeedRoleAsync(RoleManager<IdentityRole>roleManager)
        {
            string[] roles = { "Admin", "Patient", "Doctor" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))

                    await roleManager.CreateAsync(new IdentityRole(role));
            }

        }

        
    }
}

