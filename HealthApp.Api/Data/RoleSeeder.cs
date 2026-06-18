using HealthApp.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace HealthApp.Api.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAndAdminAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            string[] roles = { "Admin", "Patient", "Doctor" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = configuration["AdminUser:Email"];
            var adminPassword = configuration["AdminUser:Password"];

            if (string.IsNullOrWhiteSpace(adminEmail))
            {
                throw new InvalidOperationException("Admin email is not configured.");
            }

            if (string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new InvalidOperationException("Admin password is not configured.");
            }

            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PatientId = null,
                    DoctorId = null
                };

                var createResult = await userManager.CreateAsync(adminUser, adminPassword);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(" ", createResult.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Failed to create admin user. {errors}");
                }

                var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(" ", roleResult.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Failed to assign Admin role to admin user. {errors}");
                }
            }
            else
            {
                var isAdmin = await userManager.IsInRoleAsync(existingAdmin, "Admin");

                if (!isAdmin)
                {
                    var roleResult = await userManager.AddToRoleAsync(existingAdmin, "Admin");

                    if (!roleResult.Succeeded)
                    {
                        var errors = string.Join(" ", roleResult.Errors.Select(error => error.Description));

                        throw new InvalidOperationException(
                            $"Failed to assign Admin role to existing user. {errors}");
                    }
                }
            }
        }
    }
}