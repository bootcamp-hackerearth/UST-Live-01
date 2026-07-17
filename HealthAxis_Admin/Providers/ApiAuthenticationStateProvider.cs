using HealthAxis_Admin.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace HealthAxis_Admin.Providers
{
    public sealed class ApiAuthenticationStateProvider
        : AuthenticationStateProvider
    {
        private const string AdminRole = "Admin";
        private const string JwtAuthenticationType = "jwt";

        private readonly TokenService _tokenService;

        public ApiAuthenticationStateProvider(
            TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public override async Task<AuthenticationState>
            GetAuthenticationStateAsync()
        {
            var token =
                await _tokenService.GetAccessTokenAsync();

            if (string.IsNullOrWhiteSpace(token) ||
                IsTokenExpired(token))
            {
                await _tokenService.ClearTokensAsync();

                return CreateAnonymousState();
            }

            var identity = new ClaimsIdentity(
                ParseClaimsFromJwt(token),
                JwtAuthenticationType);

            return new AuthenticationState(
                new ClaimsPrincipal(identity));
        }

        public void NotifyUserAuthenticated()
        {
            NotifyAuthenticationStateChanged(
                GetAuthenticationStateAsync());
        }

        public void NotifyUserLoggedOut()
        {
            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    CreateAnonymousState()));
        }

        public static bool IsAdminToken(string token)
        {
            var claims =
                ParseClaimsFromJwt(token);

            return claims.Any(claim =>
                claim.Type == ClaimTypes.Role &&
                claim.Value.Equals(
                    AdminRole,
                    StringComparison.OrdinalIgnoreCase));
        }

        public static List<Claim> ParseClaimsFromJwt(
            string token)
        {
            var claims = new List<Claim>();

            var payload =
                GetJwtPayload(token);

            if (string.IsNullOrWhiteSpace(payload))
            {
                return claims;
            }

            var jsonBytes =
                Convert.FromBase64String(payload);

            var keyValuePairs =
                JsonSerializer.Deserialize<
                    Dictionary<string, JsonElement>>(
                        jsonBytes);

            if (keyValuePairs is null)
            {
                return claims;
            }

            foreach (var pair in keyValuePairs)
            {
                AddClaim(
                    claims,
                    pair.Key,
                    pair.Value);
            }

            return claims;
        }

        private static AuthenticationState
            CreateAnonymousState()
        {
            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity()));
        }

        private static void AddClaim(
            List<Claim> claims,
            string claimType,
            JsonElement claimValue)
        {
            var normalizedClaimType =
                NormalizeClaimType(claimType);

            if (claimValue.ValueKind ==
                JsonValueKind.Array)
            {
                foreach (
                    var value in
                    claimValue.EnumerateArray())
                {
                    claims.Add(
                        new Claim(
                            normalizedClaimType,
                            value.ToString()));
                }

                return;
            }

            claims.Add(
                new Claim(
                    normalizedClaimType,
                    claimValue.ToString()));
        }

        private static string NormalizeClaimType(
            string claimType)
        {
            return claimType switch
            {
                "role" => ClaimTypes.Role,
                "Role" => ClaimTypes.Role,

                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                    => ClaimTypes.Role,

                "email" => ClaimTypes.Email,
                "Email" => ClaimTypes.Email,

                "sub" => ClaimTypes.NameIdentifier,
                "nameid" => ClaimTypes.NameIdentifier,

                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                    => ClaimTypes.NameIdentifier,

                _ => claimType
            };
        }

        private static string GetJwtPayload(
            string token)
        {
            var parts = token.Split('.');

            if (parts.Length < 2)
            {
                return string.Empty;
            }

            var payload = parts[1]
                .Replace('-', '+')
                .Replace('_', '/');

            var padding =
                payload.Length % 4;

            if (padding > 0)
            {
                payload = payload.PadRight(
                    payload.Length + 4 - padding,
                    '=');
            }

            return payload;
        }

        private static bool IsTokenExpired(
            string token)
        {
            var claims =
                ParseClaimsFromJwt(token);

            var expiryClaim =
                claims.FirstOrDefault(
                    claim => claim.Type == "exp");

            if (expiryClaim is null)
            {
                return true;
            }

            if (!long.TryParse(
                    expiryClaim.Value,
                    out var expirySeconds))
            {
                return true;
            }

            var expiryDate =
                DateTimeOffset.FromUnixTimeSeconds(
                    expirySeconds);

            return expiryDate <=
                DateTimeOffset.UtcNow;
        }
    }
}

