using HealthAxisApplicn.Models;
using Microsoft.AspNetCore.Identity;

namespace HealthAxisApplicn.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Doctor", "Patient" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task SeedAdminAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "theadmin@healthaxis.com";
            string adminPassword = "Admin@123";

            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin != null)
                return;

            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Admin creation failed: {errors}");
            }

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}