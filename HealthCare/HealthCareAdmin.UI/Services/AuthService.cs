using Healthcare.Shared.DTOs.Authentication;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace HealthCareAdmin.UI.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _js;

        public AuthService(HttpClient httpClient, IJSRuntime js)
        {
            _httpClient = httpClient;
            _js = js;
        }

        public async Task<AuthorResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<AuthorResponseDto>();

            if (result != null && !string.IsNullOrEmpty(result.AccessToken))
            {
                //  SAVE TOKEN HERE
                await _js.InvokeVoidAsync("localStorage.setItem", "token", result.AccessToken);
            }

            return result;
        }
    }
}
