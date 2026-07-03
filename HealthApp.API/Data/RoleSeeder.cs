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
            if (await roleManager.RoleExistsAsync(role))
            {
                continue;
            }

            var result = await roleManager.CreateAsync(new IdentityRole(role));

            if (!result.Succeeded)
            {
                ThrowIdentityOperationFailure(
                    $"Failed to create role '{role}'",
                    result.Errors);
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
            await EnsureAdminRoleAsync(userManager, existingAdmin);
            return;
        }

        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "System Admin",
            EmailConfirmed = true,
            MustChangePassword = false,
            CreatedDate = DateTime.UtcNow
        };

        var createResult = await userManager.CreateAsync(adminUser, adminPassword);

        if (!createResult.Succeeded)
        {
            ThrowIdentityOperationFailure(
                "Failed to create admin user",
                createResult.Errors);
        }

        var addRoleResult = await userManager.AddToRoleAsync(adminUser, Roles.Admin);

        if (!addRoleResult.Succeeded)
        {
            ThrowIdentityOperationFailure(
                "Failed to assign Admin role",
                addRoleResult.Errors);
        }
    }

    private static async Task EnsureAdminRoleAsync(
        UserManager<ApplicationUser> userManager,
        ApplicationUser adminUser)
    {
        if (await userManager.IsInRoleAsync(adminUser, Roles.Admin))
        {
            return;
        }

        var roleResult = await userManager.AddToRoleAsync(adminUser, Roles.Admin);

        if (!roleResult.Succeeded)
        {
            ThrowIdentityOperationFailure(
                "Failed to assign Admin role to existing admin user",
                roleResult.Errors);
        }
    }

    private static void ThrowIdentityOperationFailure(
        string message,
        IEnumerable<IdentityError> errors)
    {
        var errorMessage = string.Join(
            ", ",
            errors.Select(error => error.Description));

        throw new InvalidOperationException($"{message}: {errorMessage}");
    }
}