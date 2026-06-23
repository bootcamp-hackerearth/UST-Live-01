using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace AdminWebApp.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;

        public CustomAuthStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string? token = await _jsRuntime.InvokeAsync<string>(
                    "localStorage.getItem",
                    "token");

                if (string.IsNullOrWhiteSpace(token))
                {
                    return GetAnonymousState();
                }

                List<Claim> claims = ParseClaimsFromJwt(token);

                ClaimsIdentity identity = new ClaimsIdentity(
                    claims,
                    "jwt",
                    ClaimTypes.Name,
                    ClaimTypes.Role);

                ClaimsPrincipal user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "token");
                return GetAnonymousState();
            }
        }

        public void NotifyUserLoggedIn(string token)
        {
            List<Claim> claims = ParseClaimsFromJwt(token);

            ClaimsIdentity identity = new ClaimsIdentity(
                claims,
                "jwt",
                ClaimTypes.Name,
                ClaimTypes.Role);

            ClaimsPrincipal user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLoggedOut()
        {
            NotifyAuthenticationStateChanged(
                Task.FromResult(GetAnonymousState()));
        }

        private static AuthenticationState GetAnonymousState()
        {
            ClaimsPrincipal anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

            return new AuthenticationState(anonymousUser);
        }

        private static List<Claim> ParseClaimsFromJwt(string jwt)
        {
            List<Claim> claims = new List<Claim>();

            string[] parts = jwt.Split('.');

            if (parts.Length < 2)
            {
                return claims;
            }

            string payload = parts[1]
                .Replace('-', '+')
                .Replace('_', '/');

            switch (payload.Length % 4)
            {
                case 2:
                    payload += "==";
                    break;

                case 3:
                    payload += "=";
                    break;
            }

            byte[] jsonBytes = Convert.FromBase64String(payload);

            Dictionary<string, JsonElement>? keyValuePairs =
                JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonBytes);

            if (keyValuePairs == null)
            {
                return claims;
            }

            foreach (KeyValuePair<string, JsonElement> kvp in keyValuePairs)
            {
                if (kvp.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement item in kvp.Value.EnumerateArray())
                    {
                        AddClaim(claims, kvp.Key, item.ToString());
                    }
                }
                else
                {
                    AddClaim(claims, kvp.Key, kvp.Value.ToString());
                }
            }

            return claims;
        }

        private static void AddClaim(List<Claim> claims, string key, string value)
        {
            claims.Add(new Claim(key, value));

            string lowerKey = key.ToLowerInvariant();

            if (lowerKey == "role" ||
                lowerKey == "roles" ||
                lowerKey.EndsWith("/role"))
            {
                claims.Add(new Claim(ClaimTypes.Role, value));
            }

            if (lowerKey == "email" ||
                lowerKey.EndsWith("/emailaddress"))
            {
                claims.Add(new Claim(ClaimTypes.Email, value));
            }

            if (lowerKey == "sub" ||
                lowerKey.EndsWith("/nameidentifier"))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, value));
            }
        }
    }
}