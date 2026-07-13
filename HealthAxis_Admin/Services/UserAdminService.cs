using HealthAxis.Shared.DTO.AdminDtos;
using HealthAxis.Shared.DTO.AuthDtos;
using System.Net.Http.Json;

namespace HealthAxis_Admin.Services
{
    public sealed class UserAdminService
    {
        private const string UsersEndpoint = "api/admin/users";
        private const string ResetPasswordEndpoint = "api/Auth/admin/reset-password";

        private readonly HttpClient _httpClient;

        public UserAdminService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AdminUserDto>> GetAllUsersAsync()
        {
            var users = await _httpClient.GetFromJsonAsync<List<AdminUserDto>>(
                UsersEndpoint);

            return users ?? new List<AdminUserDto>();
        }

        public async Task<(bool Success, string Message)> ResetPasswordAsync(
            AdminResetPasswordDto request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    ResetPasswordEndpoint,
                    request);

                var result =
                    await response.Content.ReadFromJsonAsync<ApiMessageResponse>();

                var message = result?.Message ??
                    GetDefaultResetPasswordMessage(response.IsSuccessStatusCode);

                return (response.IsSuccessStatusCode, message);
            }
            catch (HttpRequestException)
            {
                return (false, "Could not connect to the server.");
            }
            catch (InvalidOperationException)
            {
                return (false, "Reset password service is not available.");
            }
        }

        private static string GetDefaultResetPasswordMessage(bool isSuccess)
        {
            return isSuccess
                ? "Password reset successfully."
                : "Could not reset password.";
        }

        private sealed class ApiMessageResponse
        {
            public string Message { get; set; } = string.Empty;
        }
    }
}