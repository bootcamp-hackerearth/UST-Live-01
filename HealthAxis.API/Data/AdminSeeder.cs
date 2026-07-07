using Microsoft.AspNetCore.Identity;

namespace HealthAxis.API.Data
{
    public static class AdminSeeder
    {

        private const string Admin = "Admin";
        public static async Task SeedAdminAsync(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@healthaxis.com";
            string adminPassword = "Admin@123";

            if (!await roleManager.RoleExistsAsync(Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Admin));
            }

            var existingAdmin =  await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync( adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Admin);
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync( existingAdmin, Admin))
                {
                    await userManager.AddToRoleAsync(existingAdmin, Admin);
                }
            }
        }
    }
}