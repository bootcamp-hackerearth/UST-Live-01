using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace AdminWebApp.Auth
{
    public class CustomAuthStateProvider
        : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _httpClient;

        public CustomAuthStateProvider(
            IJSRuntime jsRuntime,
            HttpClient httpClient)
        {
            _jsRuntime = jsRuntime;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState>
            GetAuthenticationStateAsync()
        {
            try
            {
                var token =
                    await _jsRuntime.InvokeAsync<string?>(
                        "localStorage.getItem",
                        "token");

                if (string.IsNullOrWhiteSpace(token))
                {
                    ClearAuthorizationHeader();
                    return GetAnonymousState();
                }

                var claims =
                    ParseClaimsFromJwt(token);

                if (claims.Count == 0)
                {
                    await RemoveStoredTokenAsync();

                    ClearAuthorizationHeader();

                    return GetAnonymousState();
                }

                SetAuthorizationHeader(token);

                var identity = new ClaimsIdentity(
                    claims,
                    "jwt",
                    ClaimTypes.Name,
                    ClaimTypes.Role);

                var user =
                    new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch
            {
                await RemoveStoredTokenAsync();

                ClearAuthorizationHeader();

                return GetAnonymousState();
            }
        }

        public void NotifyUserLoggedIn(
            string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                ClearAuthorizationHeader();

                NotifyAuthenticationStateChanged(
                    Task.FromResult(
                        GetAnonymousState()));

                return;
            }

            var claims =
                ParseClaimsFromJwt(token);

            SetAuthorizationHeader(token);

            var identity = new ClaimsIdentity(
                claims,
                "jwt",
                ClaimTypes.Name,
                ClaimTypes.Role);

            var user =
                new ClaimsPrincipal(identity);

            var authenticationState =
                new AuthenticationState(user);

            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    authenticationState));
        }

        public void NotifyUserLoggedOut()
        {
            ClearAuthorizationHeader();

            NotifyAuthenticationStateChanged(
                Task.FromResult(
                    GetAnonymousState()));
        }

        private void SetAuthorizationHeader(
            string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);
        }

        private void ClearAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                null;
        }

        private async Task RemoveStoredTokenAsync()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "localStorage.removeItem",
                    "token");
            }
            catch
            {
                // Ignore JavaScript errors while recovering
                // from an invalid authentication state.
            }
        }

        private static AuthenticationState
            GetAnonymousState()
        {
            var anonymousUser =
                new ClaimsPrincipal(
                    new ClaimsIdentity());

            return new AuthenticationState(
                anonymousUser);
        }

        private static List<Claim>
            ParseClaimsFromJwt(
                string jwt)
        {
            var claims =
                new List<Claim>();

            var parts = jwt.Split('.');

            if (parts.Length < 2)
            {
                return claims;
            }

            var payload = parts[1]
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

            var jsonBytes =
                Convert.FromBase64String(
                    payload);

            var keyValuePairs =
                JsonSerializer.Deserialize<
                    Dictionary<string, JsonElement>>(
                    jsonBytes);

            if (keyValuePairs == null)
            {
                return claims;
            }

            foreach (var keyValuePair
                     in keyValuePairs)
            {
                if (
                    keyValuePair.Value.ValueKind ==
                    JsonValueKind.Array)
                {
                    foreach (
                        var item in
                        keyValuePair.Value
                            .EnumerateArray())
                    {
                        AddClaim(
                            claims,
                            keyValuePair.Key,
                            item.ToString());
                    }
                }
                else
                {
                    AddClaim(
                        claims,
                        keyValuePair.Key,
                        keyValuePair.Value.ToString());
                }
            }

            return claims;
        }

        private static void AddClaim(
            List<Claim> claims,
            string key,
            string value)
        {
            claims.Add(
                new Claim(key, value));

            var lowerKey =
                key.ToLowerInvariant();

            if (
                lowerKey == "role" ||
                lowerKey == "roles" ||
                lowerKey.EndsWith("/role"))
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        value));
            }

            if (
                lowerKey == "email" ||
                lowerKey.EndsWith(
                    "/emailaddress"))
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.Email,
                        value));
            }

            if (
                lowerKey == "sub" ||
                lowerKey.EndsWith(
                    "/nameidentifier"))
            {
                claims.Add(
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        value));
            }
        }
    }
}