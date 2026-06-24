using HealthAxisCore_Admin.Dtos.Doctors;
using System.Net.Http.Json;

namespace HealthAxisCore_Admin.Services;

public class DoctorAdminService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public DoctorAdminService(
        HttpClient httpClient,
        AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<List<AdminDoctorDto>> GetDoctorsAsync()
    {
        await _authService.AddBearerTokenAsync();

        var doctors = await _httpClient.GetFromJsonAsync<List<AdminDoctorDto>>(
            "api/admin/doctors");

        return doctors ?? new List<AdminDoctorDto>();
    }

    public async Task<bool> CreateDoctorAsync(CreateDoctorDto createDoctorDto)
    {
        await _authService.AddBearerTokenAsync();

        var response = await _httpClient.PostAsJsonAsync(
            "api/admin/doctors",
            createDoctorDto);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateDoctorAsync(
        int doctorId,
        UpdateDoctorDto updateDoctorDto)
    {
        await _authService.AddBearerTokenAsync();

        var response = await _httpClient.PutAsJsonAsync(
            $"api/admin/doctors/{doctorId}",
            updateDoctorDto);

        return response.IsSuccessStatusCode;
    }
}