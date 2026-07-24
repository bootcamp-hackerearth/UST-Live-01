using HealthAxisCore_Admin.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HealthAxisCore_Admin.Providers;

public class ApiAuthenticationStateProvider
    : AuthenticationStateProvider
{
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

        if (string.IsNullOrWhiteSpace(token))
        {
            return CreateAnonymousState();
        }

        try
        {
            if (IsTokenExpired(token))
            {
                await _tokenService.ClearTokensAsync();

                return CreateAnonymousState();
            }

            var claims =
                GetClaimsFromJwt(token);

            var identity =
                new ClaimsIdentity(
                    claims,
                    authenticationType: "jwt",
                    nameType:
                        ClaimTypes.NameIdentifier,
                    roleType:
                        ClaimTypes.Role);

            var user =
                new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch
        {
            // CHANGED:
            // A malformed or unreadable token must not crash the
            // Blazor application during authentication startup.
            await _tokenService.ClearTokensAsync();

            return CreateAnonymousState();
        }
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

    private static AuthenticationState
        CreateAnonymousState()
    {
        var identity =
            new ClaimsIdentity();

        var user =
            new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    private static bool IsTokenExpired(
        string token)
    {
        var handler =
            new JwtSecurityTokenHandler();

        var jwtToken =
            handler.ReadJwtToken(token);

        return jwtToken.ValidTo <= DateTime.UtcNow;
    }

    private static List<Claim> GetClaimsFromJwt(
        string token)
    {
        var handler =
            new JwtSecurityTokenHandler();

        var jwtToken =
            handler.ReadJwtToken(token);

        var claims =
            jwtToken.Claims.ToList();

        AddRoleClaimIfNeeded(claims);

        AddUserIdClaimIfNeeded(claims);

        AddEmailClaimIfNeeded(claims);

        return claims;
    }

    private static void AddRoleClaimIfNeeded(
        List<Claim> claims)
    {
        var roleClaim =
            claims.FirstOrDefault(claim =>
                claim.Type == ClaimTypes.Role ||
                claim.Type == "Role" ||
                claim.Type == "role" ||
                claim.Type ==
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

        if (roleClaim is null)
        {
            return;
        }

        var alreadyMapped =
            claims.Any(claim =>
                claim.Type == ClaimTypes.Role &&
                claim.Value == roleClaim.Value);

        if (!alreadyMapped)
        {
            claims.Add(
                new Claim(
                    ClaimTypes.Role,
                    roleClaim.Value));
        }
    }

    private static void AddUserIdClaimIfNeeded(
        List<Claim> claims)
    {
        var userIdClaim =
            claims.FirstOrDefault(claim =>
                claim.Type == "UserId" ||
                claim.Type ==
                    ClaimTypes.NameIdentifier ||
                claim.Type == "nameid" ||
                claim.Type == "sub");

        if (userIdClaim is null)
        {
            return;
        }

        var alreadyMapped =
            claims.Any(claim =>
                claim.Type ==
                    ClaimTypes.NameIdentifier &&
                claim.Value == userIdClaim.Value);

        if (!alreadyMapped)
        {
            claims.Add(
                new Claim(
                    ClaimTypes.NameIdentifier,
                    userIdClaim.Value));
        }
    }

    private static void AddEmailClaimIfNeeded(
        List<Claim> claims)
    {
        var emailClaim =
            claims.FirstOrDefault(claim =>
                claim.Type == ClaimTypes.Email ||
                claim.Type == "email" ||
                claim.Type == "Email");

        if (emailClaim is null)
        {
            return;
        }

        var alreadyMapped =
            claims.Any(claim =>
                claim.Type == ClaimTypes.Email &&
                claim.Value == emailClaim.Value);

        if (!alreadyMapped)
        {
            claims.Add(
                new Claim(
                    ClaimTypes.Email,
                    emailClaim.Value));
        }
    }
}