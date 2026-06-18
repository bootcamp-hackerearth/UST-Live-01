using HealthAxis.API.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace HealthAxis.API.Data
{
    public static class AdminSeeder
    {
        public static async Task SeedAdmin(UserManager<ApplicationUser> userManager)
        {
            const string adminEmail = "admin@healthaxis.com";
            const string adminPassword = "Admin@123";

            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin != null)
            {
                return;
            }

            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                MustChangePassword = false
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}