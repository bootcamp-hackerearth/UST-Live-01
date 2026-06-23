using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace HealthApp.AdminBlazor.Services;

public class AdminAuthenticationStateProvider(
    TokenStorageService tokenStorageService)
    : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal Anonymous =
        new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await tokenStorageService.GetAccessTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(Anonymous);
        }

        var identity = CreateIdentityFromToken(token);

        if (identity is null)
        {
            return new AuthenticationState(Anonymous);
        }

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyUserAuthentication(string token)
    {
        var identity = CreateIdentityFromToken(token);

        var user = identity is null
            ? Anonymous
            : new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(Anonymous)));
    }

    private static ClaimsIdentity? CreateIdentityFromToken(string token)
    {
        try
        {
            var claims = ParseClaimsFromJwt(token).ToList();

            var expClaim = claims.FirstOrDefault(c => c.Type == "exp");

            if (expClaim is not null &&
                long.TryParse(expClaim.Value, out var expSeconds))
            {
                var expiryDate = DateTimeOffset
                    .FromUnixTimeSeconds(expSeconds)
                    .UtcDateTime;

                if (expiryDate < DateTime.UtcNow)
                {
                    return null;
                }
            }

            return new ClaimsIdentity(claims, "jwt");
        }
        catch
        {
            return null;
        }
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];

        var jsonBytes = ParseBase64WithoutPadding(payload);

        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        var claims = new List<Claim>();

        if (keyValuePairs is null)
        {
            return claims;
        }

        foreach (var item in keyValuePairs)
        {
            var key = item.Key;
            var value = item.Value?.ToString() ?? string.Empty;

            if (key == "role" ||
                key == ClaimTypes.Role ||
                key.EndsWith("/role"))
            {
                claims.Add(new Claim(ClaimTypes.Role, value));
            }
            else if (key == "nameid" ||
                     key == ClaimTypes.NameIdentifier ||
                     key.EndsWith("/nameidentifier"))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, value));
            }
            else if (key == "email" ||
                     key == ClaimTypes.Email ||
                     key.EndsWith("/emailaddress"))
            {
                claims.Add(new Claim(ClaimTypes.Email, value));
            }
            else
            {
                claims.Add(new Claim(key, value));
            }
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;

            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(
            base64.Replace('-', '+').Replace('_', '/'));
    }
}