using HealthAxisHealth.API.Configurations;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HealthAxis_Health.API.Helpers
{
    public class JwtTokenGenerator :
        IJwtTokenGenerator
    {

        #region Fields

        private readonly JwtSettings _jwtSettings;

        #endregion

        #region Constructor

        public JwtTokenGenerator(
            IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        #endregion

        #region Methods

        public string GenerateToken(
            User user)
        {
            var claims =
                new List<Claim>
                {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        user.UserId.ToString()),

                    new Claim(
                        ClaimTypes.Email,
                        user.Email),

                    new Claim(
                        ClaimTypes.Role,
                        user.Role.ToString())
                };

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _jwtSettings.SecretKey));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(
                        _jwtSettings.ExpiryMinutes),
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes =
                new byte[64];

            using var rng =
                RandomNumberGenerator.Create();

            rng.GetBytes(
                randomBytes);

            return Convert.ToBase64String(
                randomBytes);
        }

        #endregion
    }
}
