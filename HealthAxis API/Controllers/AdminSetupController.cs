using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxis.API.Controllers
{
    [Route("api/setup")]
    [ApiController]
    [AllowAnonymous]
    public class AdminSetupController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public AdminSetupController(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _configuration = configuration;
            _environment = environment;
        }

        [HttpPost("admin")]
        public async Task<IActionResult> CreateAdmin(
            [FromHeader(Name = "X-Setup-Key")] string setupKey)
        {
            if (!_environment.IsDevelopment())
            {
                return NotFound();
            }

            string? configuredSetupKey =
                _configuration["AdminSeed:SetupKey"];

            if (string.IsNullOrWhiteSpace(configuredSetupKey) ||
                setupKey != configuredSetupKey)
            {
                return Unauthorized(new
                {
                    message = "Invalid setup key."
                });
            }

            string adminEmail =
                _configuration["AdminSeed:Email"]
                ?? "admin@healthaxis.com";

            string adminPassword =
                _configuration["AdminSeed:Password"]
                ?? "Admin@12345";

            IdentityUser? adminUser =
                await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                IdentityResult createResult =
                    await _userManager.CreateAsync(
                        adminUser,
                        adminPassword);

                if (!createResult.Succeeded)
                {
                    string errors = string.Join(
                        ", ",
                        createResult.Errors.Select(error => error.Description));

                    return BadRequest(new
                    {
                        message = errors
                    });
                }
            }

            IList<Claim> claims =
                await _userManager.GetClaimsAsync(adminUser);

            bool hasAdminRoleClaim =
                claims.Any(claim =>
                    claim.Type == ClaimTypes.Role &&
                    claim.Value == "Admin");

            if (!hasAdminRoleClaim)
            {
                await _userManager.AddClaimAsync(
                    adminUser,
                    new Claim(ClaimTypes.Role, "Admin"));
            }

            bool hasReferenceIdClaim =
                claims.Any(claim =>
                    claim.Type == "ReferenceId");

            if (!hasReferenceIdClaim)
            {
                await _userManager.AddClaimAsync(
                    adminUser,
                    new Claim("ReferenceId", "0"));
            }

            return Ok(new
            {
                message = "Admin user created successfully.",
                email = adminEmail
            });
        }
    }
}
