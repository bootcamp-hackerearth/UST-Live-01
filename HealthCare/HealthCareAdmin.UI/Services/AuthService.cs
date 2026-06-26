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

        public async Task<(AuthorResponseDto? Result, string? Error)> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var errorObj = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                        if (errorObj != null && errorObj.ContainsKey("message"))
                            return (null, errorObj["message"]);
                    }
                    catch { }

                    var error = await response.Content.ReadAsStringAsync();
                    return (null, error);
                }

                var result = await response.Content.ReadFromJsonAsync<AuthorResponseDto>();

                if (result == null)
                    return (null, "Invalid server response");

                if (!string.IsNullOrEmpty(result.AccessToken))
                {
                    await _js.InvokeVoidAsync("localStorage.setItem", "token", result.AccessToken);
                }

                return (result, null);
            }
            catch
            {
                return (null, "Unable to connect to server");
            }
        }
    }
}