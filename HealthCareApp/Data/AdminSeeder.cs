using Microsoft.AspNetCore.Identity;

namespace HealthCareApp.Data
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            string adminRole = "Admin";

            string? adminEmail = configuration["SeedAdmin:Email"];

            string? adminPassword = configuration["SeedAdmin:Password"];

            if (string.IsNullOrWhiteSpace(adminEmail) ||
                string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new InvalidOperationException(
                    "Seed admin credentials are missing. Please configure SeedAdmin:Email and SeedAdmin:Password.");
            }

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Failed to seed admin user: {errors}");
                }
            }
        }
    }
}