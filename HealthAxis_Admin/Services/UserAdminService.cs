using HealthAxis.Shared.DTO.AdminDtos;
using System.Net.Http.Json;

namespace HealthAxis_Admin.Services
{
    public sealed class UserAdminService
    {
        private const string UsersEndpoint = "api/admin/users";

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
    }
}