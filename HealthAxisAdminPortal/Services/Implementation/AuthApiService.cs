using HealthAxisAdminPortal.Services.Interfaces;
using HealthAxisApplicn.Dto.Auth;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace HealthAxisAdminPortal.Services.Implementation
{
    public class AuthApiService: IAuthApiService
    {
        private readonly HttpClient http;

        public AuthApiService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<AuthResponse?> LoginAsync(LoginDto dto)
        {
            var response = await http.PostAsJsonAsync("/api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

    }
}
