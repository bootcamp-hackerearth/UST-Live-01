using HealthApp.AdminPortal.Services;
using HealthApp.AdminPortal.Services.Interface;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HealthApp.AdminPortal.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenService _tokenService;
        private readonly AuthRedirectState _authRedirectState;

        public CustomAuthStateProvider(
            ITokenService tokenService,
            AuthRedirectState authRedirectState)
        {
            _tokenService = tokenService;
            _authRedirectState = authRedirectState;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _tokenService.GetToken();

            if (string.IsNullOrWhiteSpace(token))
            {
                return GetAnonymousState();
            }

            if (IsTokenExpired(token))
            {
                await _tokenService.RemoveToken();
                _authRedirectState.SetMessage("Your session has expired. Please login again.");
                return GetAnonymousState();
            }

            var principal = CreateClaimsPrincipalFromToken(token);

            return new AuthenticationState(principal);
        }

        public void NotifyUserAuthentication(string token)
        {
            var principal = CreateClaimsPrincipalFromToken(token);

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(principal)));
        }

        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(
                Task.FromResult(GetAnonymousState()));
        }

        private static AuthenticationState GetAnonymousState()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

            return new AuthenticationState(anonymousUser);
        }

        private static bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var expiryClaim = jwt.Claims
                .FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Exp);

            if (expiryClaim == null)
            {
                return true;
            }

            var expiryUnixTime = long.Parse(expiryClaim.Value);

            var expiryDateTime = DateTimeOffset
                .FromUnixTimeSeconds(expiryUnixTime)
                .UtcDateTime;

            return expiryDateTime <= DateTime.UtcNow;
        }

        private static ClaimsPrincipal CreateClaimsPrincipalFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var claims = jwt.Claims.Select(claim =>
            {
                if (claim.Type == "role")
                {
                    return new Claim(ClaimTypes.Role, claim.Value);
                }

                if (claim.Type == "name")
                {
                    return new Claim(ClaimTypes.Name, claim.Value);
                }

                if (claim.Type == "email")
                {
                    return new Claim(ClaimTypes.Email, claim.Value);
                }

                return claim;
            }).ToList();

            var identity = new ClaimsIdentity(
                claims,
                authenticationType: "jwt",
                nameType: ClaimTypes.Name,
                roleType: ClaimTypes.Role);

            return new ClaimsPrincipal(identity);
        }
    }
}