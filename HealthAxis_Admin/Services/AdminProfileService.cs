using HealthAxis.Shared.DTO.AdminDtos;
using System.Net.Http.Json;
using System.Text.Json;

namespace HealthAxis_Admin.Services
{
    public sealed class AdminProfileService
    {
        private const string ProfileEndpoint = "/api/admin/profile";

        private const string ChangePasswordEndpoint =
            "/api/admin/profile/change-password";

        private readonly HttpClient _httpClient;

        public AdminProfileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AdminProfileDto> GetProfileAsync()
        {
            var profile = await _httpClient.GetFromJsonAsync<AdminProfileDto>(
                ProfileEndpoint);

            return profile ?? new AdminProfileDto();
        }

        public async Task<(bool Success, string Message)> UpdateProfileAsync(
            UpdateAdminProfileDto profileDto)
        {
            using var response = await _httpClient.PutAsJsonAsync(
                ProfileEndpoint,
                profileDto);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Profile updated successfully.");
            }

            var errorMessage = await ReadErrorMessageAsync(response);
            return (false, errorMessage);
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(
            AdminChangePasswordDto passwordDto)
        {
            using var response = await _httpClient.PutAsJsonAsync(
                ChangePasswordEndpoint,
                passwordDto);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Password changed successfully.");
            }

            var errorMessage = await ReadErrorMessageAsync(response);
            return (false, errorMessage);
        }

        private static async Task<string> ReadErrorMessageAsync(
            HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return "Request failed.";
            }

            try
            {
                using var document = JsonDocument.Parse(content);

                if (document.RootElement.TryGetProperty(
                        "message",
                        out var messageElement))
                {
                    return messageElement.GetString() ?? "Request failed.";
                }
            }
            catch (JsonException)
            {
                return content;
            }

            return "Request failed.";
        }
    }
}
