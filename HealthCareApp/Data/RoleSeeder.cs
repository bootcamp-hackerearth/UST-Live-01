using Microsoft.AspNetCore.Identity;

namespace HealthCareApp.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new[] { "Admin", "Doctor", "Patient" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
