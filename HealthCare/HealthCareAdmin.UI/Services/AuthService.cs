using Healthcare.Shared.DTOs.Authentication;
using System.Net.Http.Json;

namespace HealthCareAdmin.UI.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthorResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/auth/login",
                loginDto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content
                    .ReadFromJsonAsync<AuthorResponseDto>();
            }

            return null;
        }
    }
}