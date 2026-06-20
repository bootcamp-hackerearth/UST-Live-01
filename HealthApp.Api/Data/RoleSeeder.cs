using HealthApp.Api.Constants;
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
            await SeedRolesAsync(roleManager);

            var adminEmail = GetRequiredConfigurationValue(
                configuration,
                "AdminUser:Email",
                "Admin email is not configured.");

            var adminPassword = GetRequiredConfigurationValue(
                configuration,
                "AdminUser:Password",
                "Admin password is not configured.");

            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                await CreateAdminUserAsync(
                    userManager,
                    adminEmail,
                    adminPassword);

                return;
            }

            await EnsureAdminRoleAsync(
                userManager,
                existingAdmin,
                "Failed to assign Admin role to existing user.");
        }

        private static async Task SeedRolesAsync(
            RoleManager<IdentityRole> roleManager)
        {
            string[] roles =
            {
                RoleConstants.Admin,
                RoleConstants.Patient,
                RoleConstants.Doctor
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static string GetRequiredConfigurationValue(
            IConfiguration configuration,
            string key,
            string errorMessage)
        {
            var value = configuration[key];

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(errorMessage);
            }

            return value;
        }

        private static async Task CreateAdminUserAsync(
            UserManager<ApplicationUser> userManager,
            string adminEmail,
            string adminPassword)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                PatientId = null,
                DoctorId = null
            };

            var createResult = await userManager.CreateAsync(
                adminUser,
                adminPassword);

            ThrowIfFailed(
                createResult,
                "Failed to create admin user.");

            await EnsureAdminRoleAsync(
                userManager,
                adminUser,
                "Failed to assign Admin role to admin user.");
        }

        private static async Task EnsureAdminRoleAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationUser user,
            string errorMessage)
        {
            var isAdmin = await userManager.IsInRoleAsync(
                user,
                RoleConstants.Admin);

            if (isAdmin)
            {
                return;
            }

            var roleResult = await userManager.AddToRoleAsync(
                user,
                RoleConstants.Admin);

            ThrowIfFailed(
                roleResult,
                errorMessage);
        }

        private static void ThrowIfFailed(
            IdentityResult result,
            string errorMessage)
        {
            if (result.Succeeded)
            {
                return;
            }

            var errors = string.Join(
                " ",
                result.Errors.Select(error => error.Description));

            throw new InvalidOperationException(
                $"{errorMessage} {errors}");
        }
    }
}