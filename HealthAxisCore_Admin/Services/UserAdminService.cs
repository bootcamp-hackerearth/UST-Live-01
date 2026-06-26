using HealthAxisCore_Admin.Dtos.Common;
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

    public async Task<PagedResultDto<AdminUserDto>> GetUsersAsync(
        string? role = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        await _authService.AddBearerTokenAsync();

        var url = string.IsNullOrWhiteSpace(role)
            ? $"api/admin/users?pageNumber={pageNumber}&pageSize={pageSize}"
            : $"api/admin/users?role={Uri.EscapeDataString(role)}&pageNumber={pageNumber}&pageSize={pageSize}";

        var result = await _httpClient.GetFromJsonAsync<PagedResultDto<AdminUserDto>>(url);

        return result ?? new PagedResultDto<AdminUserDto>();
    }
}