using HealthApp.Blazor.Components.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace HealthApp.Blazor.Components.Services
{
    public class AuthService
    {
            private bool _isLoggedIn;

            public bool IsLoggedIn => _isLoggedIn;

            public void Login()
            {
                _isLoggedIn = true;
            }

            public void Logout()
            {
                _isLoggedIn = false;
            }
        }
}







//public class AuthService : IAuthService
//{
//    private readonly HttpClient http;
//    private readonly IJSRuntime js;
//    private readonly AuthenticationStateProvider authStateProvider;

//    public AuthService(HttpClient http, IJSRuntime js, AuthenticationStateProvider authStateProvider)
//    {
//        this.http = http;
//        this.js = js;
//        this.authStateProvider = authStateProvider;
//    }

//    public async Task<AuthResult> LoginAsync(string email, string password)
//    {
//        var resp = await http.PostAsJsonAsync("/api/auth/login", new { email, password });
//        if (!resp.IsSuccessStatusCode)
//        {
//            var msg = await resp.Content.ReadAsStringAsync();
//            return new AuthResult { Success = false, Error = msg };
//        }

//        var payload = await resp.Content.ReadFromJsonAsync<LoginResponse>();
//        await js.InvokeVoidAsync("localStorage.setItem", "authToken", payload.Token);

//        // notify auth state provider
//        if (authStateProvider is CustomAuthStateProvider cap)
//            await cap.NotifyUserAuthentication(payload.Token);

//        return new AuthResult { Success = true, Token = payload.Token };
//    }

//    public async Task LogoutAsync()
//    {
//        await js.InvokeVoidAsync("localStorage.removeItem", "authToken");
//        if (authStateProvider is CustomAuthStateProvider cap)
//            await cap.NotifyUserLogout();
//    }

//    private class LoginResponse { public string Token { get; set; } }
//}