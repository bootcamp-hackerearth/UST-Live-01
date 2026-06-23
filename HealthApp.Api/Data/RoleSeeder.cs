using Microsoft.AspNetCore.Identity;

namespace HealthApp.Api.Data
{
    public static class RoleSeeder
    {
        public static async Task seedroleAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] role = { "Admin", "User", "Doctor" };
            foreach (var roleName in role)
            {
                if(!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
