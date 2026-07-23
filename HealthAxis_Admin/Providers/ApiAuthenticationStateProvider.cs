using HealthAxis_Admin.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace HealthAxis_Admin.Providers
{
    public sealed class ApiAuthenticationStateProvider
        : AuthenticationStateProvider
    {
        private const string AdminRole = "Admin";
        private const string JwtAuthenticationType = "jwt";
        private const string ExpirationClaimType = "exp";

        private const string RoleClaimUri =
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        private const string NameIdentifierClaimUri =
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

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

            // Important:
            // Do not clear storage when the token is temporarily missing.
            // ExternalLogin.razor may currently be saving the token.
            if (string.IsNullOrWhiteSpace(token))
            {
                return CreateAnonymousState();
            }

            var claims =
                ParseClaimsFromJwt(token);

            if (claims.Count == 0 ||
                IsTokenExpired(claims) ||
                !ContainsAdminRole(claims))
            {
                await _tokenService.ClearTokensAsync();

                return CreateAnonymousState();
            }

            return CreateAuthenticatedState(claims);
        }

        public void NotifyUserAuthenticated()
        {
            var authenticationStateTask =
                GetAuthenticationStateAsync();

            NotifyAuthenticationStateChanged(
                authenticationStateTask);
        }

        public void NotifyUserLoggedOut()
        {
            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    CreateAnonymousState()));
        }

        public static bool IsAdminToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            var claims =
                ParseClaimsFromJwt(token);

            return ContainsAdminRole(claims);
        }

        public static List<Claim> ParseClaimsFromJwt(
            string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return [];
            }

            var payload =
                GetJwtPayload(token);

            if (string.IsNullOrWhiteSpace(payload))
            {
                return [];
            }

            try
            {
                var jsonBytes =
                    Convert.FromBase64String(payload);

                var keyValuePairs =
                    JsonSerializer.Deserialize<
                        Dictionary<string, JsonElement>>(
                            jsonBytes);

                if (keyValuePairs is null)
                {
                    return [];
                }

                var claims =
                    new List<Claim>();

                foreach (var pair in keyValuePairs)
                {
                    AddClaim(
                        claims,
                        pair.Key,
                        pair.Value);
                }

                return claims;
            }
            catch (FormatException)
            {
                return [];
            }
            catch (JsonException)
            {
                return [];
            }
        }

        private static AuthenticationState
            CreateAuthenticatedState(
                IEnumerable<Claim> claims)
        {
            var identity =
                new ClaimsIdentity(
                    claims,
                    JwtAuthenticationType,
                    ClaimTypes.Name,
                    ClaimTypes.Role);

            var principal =
                new ClaimsPrincipal(identity);

            return new AuthenticationState(principal);
        }

        private static AuthenticationState
            CreateAnonymousState()
        {
            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity()));
        }

        private static bool ContainsAdminRole(
            IEnumerable<Claim> claims)
        {
            return claims.Any(claim =>
                claim.Type == ClaimTypes.Role &&
                claim.Value.Equals(
                    AdminRole,
                    StringComparison.OrdinalIgnoreCase));
        }

        private static void AddClaim(
            ICollection<Claim> claims,
            string claimType,
            JsonElement claimValue)
        {
            if (claimValue.ValueKind is
                JsonValueKind.Null or
                JsonValueKind.Undefined)
            {
                return;
            }

            var normalizedClaimType =
                NormalizeClaimType(claimType);

            if (claimValue.ValueKind ==
                JsonValueKind.Array)
            {
                AddArrayClaims(
                    claims,
                    normalizedClaimType,
                    claimValue);

                return;
            }

            claims.Add(
                new Claim(
                    normalizedClaimType,
                    claimValue.ToString()));
        }

        private static void AddArrayClaims(
            ICollection<Claim> claims,
            string claimType,
            JsonElement claimValues)
        {
            foreach (var claimValue in
                     claimValues.EnumerateArray())
            {
                if (claimValue.ValueKind is
                    JsonValueKind.Null or
                    JsonValueKind.Undefined)
                {
                    continue;
                }

                claims.Add(
                    new Claim(
                        claimType,
                        claimValue.ToString()));
            }
        }

        private static string NormalizeClaimType(
            string claimType)
        {
            if (claimType.Equals(
                    "role",
                    StringComparison.OrdinalIgnoreCase) ||
                claimType.Equals(
                    RoleClaimUri,
                    StringComparison.Ordinal))
            {
                return ClaimTypes.Role;
            }

            if (claimType.Equals(
                    "email",
                    StringComparison.OrdinalIgnoreCase))
            {
                return ClaimTypes.Email;
            }

            if (claimType.Equals(
                    "sub",
                    StringComparison.OrdinalIgnoreCase) ||
                claimType.Equals(
                    "nameid",
                    StringComparison.OrdinalIgnoreCase) ||
                claimType.Equals(
                    NameIdentifierClaimUri,
                    StringComparison.Ordinal))
            {
                return ClaimTypes.NameIdentifier;
            }

            return claimType;
        }

        private static string GetJwtPayload(
            string token)
        {
            var tokenParts =
                token.Split('.');

            if (tokenParts.Length < 2)
            {
                return string.Empty;
            }

            var payload =
                tokenParts[1]
                    .Replace('-', '+')
                    .Replace('_', '/');

            var missingPadding =
                payload.Length % 4;

            if (missingPadding == 0)
            {
                return payload;
            }

            return payload.PadRight(
                payload.Length + 4 - missingPadding,
                '=');
        }

        private static bool IsTokenExpired(
            IEnumerable<Claim> claims)
        {
            var expiryClaim =
                claims.FirstOrDefault(claim =>
                    claim.Type.Equals(
                        ExpirationClaimType,
                        StringComparison.Ordinal));

            if (expiryClaim is null)
            {
                return true;
            }

            if (!long.TryParse(
                    expiryClaim.Value,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out var expirySeconds))
            {
                return true;
            }

            try
            {
                var expiryDate =
                    DateTimeOffset.FromUnixTimeSeconds(
                        expirySeconds);

                return expiryDate <=
                    DateTimeOffset.UtcNow;
            }
            catch (ArgumentOutOfRangeException)
            {
                return true;
            }
        }
    }
}