using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace HealthCareApp.AdminBlazor.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private const string TokenStorageKey = "token";
        private const string AuthenticationType = "jwt";

        private readonly IJSRuntime _jsRuntime;

        private ClaimsPrincipal _currentUser = CreateAnonymousUser();

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_currentUser.Identity?.IsAuthenticated == true)
            {
                return new AuthenticationState(_currentUser);
            }

            var token = await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                TokenStorageKey);

            if (string.IsNullOrWhiteSpace(token))
            {
                return CreateAnonymousAuthenticationState();
            }

            var claims = ParseClaimsFromJwt(token);

            if (claims.Count == 0)
            {
                return CreateAnonymousAuthenticationState();
            }

            _currentUser = CreateAuthenticatedUser(claims);

            return new AuthenticationState(_currentUser);
        }

        public void NotifyUserLoggedIn(string token)
        {
            var claims = ParseClaimsFromJwt(token);

            _currentUser = CreateAuthenticatedUser(claims);

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(_currentUser)));
        }

        public void NotifyUserLoggedOut()
        {
            _currentUser = CreateAnonymousUser();

            NotifyAuthenticationStateChanged(
                Task.FromResult(CreateAnonymousAuthenticationState()));
        }

        private static AuthenticationState CreateAnonymousAuthenticationState()
        {
            return new AuthenticationState(CreateAnonymousUser());
        }

        private static ClaimsPrincipal CreateAnonymousUser()
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        private static ClaimsPrincipal CreateAuthenticatedUser(List<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, AuthenticationType);

            return new ClaimsPrincipal(identity);
        }

        private static List<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();

            var tokenParts = jwt.Split('.');

            if (tokenParts.Length < 2)
            {
                return claims;
            }

            try
            {
                var payloadBytes = DecodeJwtPayload(tokenParts[1]);

                var keyValuePairs =
                    JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(payloadBytes);

                if (keyValuePairs is null)
                {
                    return claims;
                }

                foreach (var keyValuePair in keyValuePairs)
                {
                    AddClaims(claims, keyValuePair.Key, keyValuePair.Value);
                }
            }
            catch (FormatException)
            {
                return claims;
            }
            catch (JsonException)
            {
                return claims;
            }

            return claims;
        }

        private static byte[] DecodeJwtPayload(string payload)
        {
            var base64 = payload
                .Replace('-', '+')
                .Replace('_', '/');

            base64 = AddBase64Padding(base64);

            return Convert.FromBase64String(base64);
        }

        private static string AddBase64Padding(string base64)
        {
            int remainder = base64.Length % 4;

            if (remainder == 2)
            {
                return base64 + "==";
            }

            if (remainder == 3)
            {
                return base64 + "=";
            }

            return base64;
        }

        private static void AddClaims(
            List<Claim> claims,
            string claimType,
            JsonElement claimValue)
        {
            if (claimValue.ValueKind == JsonValueKind.Array)
            {
                foreach (var value in claimValue.EnumerateArray())
                {
                    claims.Add(new Claim(claimType, value.ToString()));
                }

                return;
            }

            claims.Add(new Claim(claimType, claimValue.ToString()));
        }
    }
}