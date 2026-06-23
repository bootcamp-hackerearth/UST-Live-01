using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using System.Net.Http.Json;

namespace HealthApp.AdminBlazor.Services;

public class AdminApiService(HttpClient httpClient)
{
    public async Task<List<DoctorDto>> GetDoctorsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<DoctorDto>>("api/admin/doctors")
            ?? new List<DoctorDto>();
    }

    public async Task<CreateDoctorResponseDto?> CreateDoctorAsync(CreateDoctorDto dto)
    {
        var response = await httpClient.PostAsJsonAsync("api/admin/doctors", dto);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<CreateDoctorResponseDto>();
    }

    public async Task<DoctorDto?> UpdateDoctorAsync(
        int doctorId,
        UpdateDoctorDto dto)
    {
        var response = await httpClient.PutAsJsonAsync(
            $"api/admin/doctors/{doctorId}",
            dto);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<DoctorDto>();
    }

    public async Task<List<UserDto>> GetUsersAsync(string? role = null)
    {
        var url = string.IsNullOrWhiteSpace(role) || role == "All"
            ? "api/admin/users"
            : $"api/admin/users?role={role}";

        return await httpClient.GetFromJsonAsync<List<UserDto>>(url)
            ?? new List<UserDto>();
    }

    public async Task<List<AppointmentReportDto>> GetAppointmentReportsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<AppointmentReportDto>>(
                "api/admin/reports/appointments")
            ?? new List<AppointmentReportDto>();
    }

    public async Task<List<PatientDto>> GetPatientsAsync(
        string? search = null,
        GenderType? gender = null,
        bool? hasInsurance = null)
    {
        var queryParams = new List<string>();

        if (!string.IsNullOrWhiteSpace(search))
        {
            queryParams.Add($"search={Uri.EscapeDataString(search)}");
        }

        if (gender.HasValue)
        {
            queryParams.Add($"gender={gender.Value}");
        }

        if (hasInsurance.HasValue)
        {
            queryParams.Add($"hasInsurance={hasInsurance.Value.ToString().ToLower()}");
        }

        var url = queryParams.Count == 0
            ? "api/admin/patients"
            : $"api/admin/patients?{string.Join("&", queryParams)}";

        return await httpClient.GetFromJsonAsync<List<PatientDto>>(url)
            ?? new List<PatientDto>();
    }
}