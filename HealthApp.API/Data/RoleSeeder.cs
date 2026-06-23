
using HealthApp.API.Identity;
using HealthApp.Shared.Constants;
using Microsoft.AspNetCore.Identity;

namespace HealthApp.API.Data;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles =
        {
            Roles.Patient,
            Roles.Doctor,
            Roles.Admin
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));

                if (!result.Succeeded)
                {
                    throw new Exception(
                        $"Failed to create role '{role}': " +
                        string.Join(", ", result.Errors.Select(e => e.Description))
                    );
                }
            }
        }
    }

    public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
    {
        const string adminEmail = "admin@healthapp.com";
        const string adminPassword = "Admin@123";

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin is not null)
        {
            if (!await userManager.IsInRoleAsync(existingAdmin, Roles.Admin))
            {
                var roleResult = await userManager.AddToRoleAsync(existingAdmin, Roles.Admin);

                if (!roleResult.Succeeded)
                {
                    throw new Exception(
                        "Failed to assign Admin role to existing admin user: " +
                        string.Join(", ", roleResult.Errors.Select(e => e.Description))
                    );
                }
            }

            return;
        }

        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "System Admin",
            EmailConfirmed = true,
            CreatedDate = DateTime.UtcNow
        };

        var createResult = await userManager.CreateAsync(adminUser, adminPassword);

        if (!createResult.Succeeded)
        {
            throw new Exception(
                "Failed to create admin user: " +
                string.Join(", ", createResult.Errors.Select(e => e.Description))
            );
        }

        var addRoleResult = await userManager.AddToRoleAsync(adminUser, Roles.Admin);

        if (!addRoleResult.Succeeded)
        {
            throw new Exception(
                "Failed to assign Admin role: " +
                string.Join(", ", addRoleResult.Errors.Select(e => e.Description))
            );
        }
    }
}
