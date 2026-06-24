using HealthAxisCore_Admin.Dtos.Users;
using System.Net.Http.Json;

namespace HealthAxisCore_Admin.Services;

public class UserAdminService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public UserAdminService(
        HttpClient httpClient,
        AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<List<AdminUserDto>> GetUsersAsync(string? role = null)
    {
        await _authService.AddBearerTokenAsync();

        var url = string.IsNullOrWhiteSpace(role)
            ? "api/admin/users"
            : $"api/admin/users?role={Uri.EscapeDataString(role)}";

        var users = await _httpClient.GetFromJsonAsync<List<AdminUserDto>>(url);

        return users ?? new List<AdminUserDto>();
    }
}