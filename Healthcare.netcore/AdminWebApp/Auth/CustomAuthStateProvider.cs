using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace AdminWebApp.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _js;

        public CustomAuthStateProvider(IJSRuntime js)
        {
            _js = js;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");

            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public void NotifyUserLoggedIn(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(anonymousUser)));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];

            payload = payload.Replace('-', '+').Replace('_', '/');

            switch (payload.Length % 4)
            {
                case 2:
                    payload += "==";
                    break;
                case 3:
                    payload += "=";
                    break;
            }

            var jsonBytes = Convert.FromBase64String(payload);
            var values = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonBytes);

            if (values == null)
            {
                return claims;
            }

            foreach (var item in values)
            {
                if (item.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var arrayItem in item.Value.EnumerateArray())
                    {
                        AddClaim(claims, item.Key, arrayItem.ToString());
                    }
                }
                else
                {
                    AddClaim(claims, item.Key, item.Value.ToString());
                }
            }

            return claims;
        }

        private void AddClaim(List<Claim> claims, string key, string value)
        {
            claims.Add(new Claim(key, value));

            if (key == "role" || key.EndsWith("/role"))
            {
                claims.Add(new Claim(ClaimTypes.Role, value));
            }

            if (key == "email" || key.EndsWith("/emailaddress"))
            {
                claims.Add(new Claim(ClaimTypes.Email, value));
            }
        }
    }
}