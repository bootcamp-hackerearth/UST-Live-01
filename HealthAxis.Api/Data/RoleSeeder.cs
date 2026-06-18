using HealthAxisCore_Api.Models;
using Microsoft.AspNetCore.Identity;

namespace HealthAxisCore_Api.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Patient", "Doctor" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!result.Succeeded)
                    {
                        throw new FormatException(string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }

        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var email = "admin@healthcare.com";
            var password = "Admin@123";
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    PhoneNumber = "9999999999",
                    IsActive = true
                };
                var createResult = await userManager.CreateAsync(user, password);
                if (!createResult.Succeeded)
                {
                    throw new FormatException(string.Join(", ", createResult.Errors.Select(e => e.Description)));
                }
            }
            if (!await userManager.IsInRoleAsync(user, "Admin"))
            {
                var roleResult = await userManager.AddToRoleAsync(user, "Admin");
                if (!roleResult.Succeeded)
                {
                    throw new FormatException(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
