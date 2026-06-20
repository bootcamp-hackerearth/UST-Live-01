using HealthAxisCore_Api.Models;
using Microsoft.AspNetCore.Identity;

namespace HealthAxisCore_Api.Data
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var adminEmail = "admin@healthaxis.com";
            var adminPassword = "Admin@123";

            
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Role = "Admin",
                    ReferenceId = null,
                    IsFirstLogin = false,
                    TemporaryPassword = null,
                    CreatedDate = DateTime.Now
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (!result.Succeeded)
                {
                    return;
                }
            }

            
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
