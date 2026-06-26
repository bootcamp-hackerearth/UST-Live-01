using HealthAxisAdminLayout.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;


public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAuthStateService _authState;

    public CustomAuthenticationStateProvider(IAuthStateService authState)
    {
        _authState = authState;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            await _authState.LoadFromStorageAsync();
        }
        catch
        {
            // ignore JSInterop timing errors
        }

        ClaimsIdentity identity;

        if (!string.IsNullOrWhiteSpace(_authState.Token))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _authState.Email ?? ""),
                new Claim(ClaimTypes.Role, _authState.Role ?? "")
            };

            identity = new ClaimsIdentity(claims, "jwt");
        }
        else
        {
            identity = new ClaimsIdentity();
        }

        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }
}
