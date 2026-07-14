using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;

namespace HealthApp.AdminBlazor.Services;

public class AdminApiService(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public async Task<PagedResultDto<DoctorDto>> GetDoctorsAsync(
        int pageNumber = 1,
        int pageSize = 5)
    {
        var url = $"api/admin/doctors?pageNumber={pageNumber}&pageSize={pageSize}";

        return await httpClient.GetFromJsonAsync<PagedResultDto<DoctorDto>>(url, JsonOptions)
            ?? new PagedResultDto<DoctorDto>
            {
                Items = new List<DoctorDto>(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = 0
            };
    }

    public async Task<CreateDoctorResponseDto?> CreateDoctorAsync(CreateDoctorDto dto)
    {
        var response = await httpClient.PostAsJsonAsync(
            "api/admin/doctors",
            dto,
            JsonOptions);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<CreateDoctorResponseDto>(JsonOptions);
    }

    public async Task<DoctorDto?> UpdateDoctorAsync(
        int doctorId,
        UpdateDoctorDto dto)
    {
        var response = await httpClient.PutAsJsonAsync(
            $"api/admin/doctors/{doctorId}",
            dto,
            JsonOptions);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<DoctorDto>(JsonOptions);
    }

    public async Task<List<DoctorLeaveDto>> GetDoctorLeaveHistoryAsync(int doctorId)
    {
        return await httpClient.GetFromJsonAsync<List<DoctorLeaveDto>>(
                $"api/doctor-leaves/doctor/{doctorId}",
                JsonOptions)
            ?? new List<DoctorLeaveDto>();
    }

    public async Task<DoctorLeaveStatusDto?> GetDoctorLeaveStatusAsync(
        int doctorId,
        DateTime date)
    {
        var dateText = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var url = $"api/doctor-leaves/doctor/{doctorId}/status?date={dateText}";

        return await httpClient.GetFromJsonAsync<DoctorLeaveStatusDto>(url, JsonOptions);
    }

    public async Task<List<UserDto>> GetUsersAsync(string? role = null)
    {
        var url = string.IsNullOrWhiteSpace(role) || role == "All"
            ? "api/admin/users"
            : $"api/admin/users?role={Uri.EscapeDataString(role)}";

        return await httpClient.GetFromJsonAsync<List<UserDto>>(url, JsonOptions)
            ?? new List<UserDto>();
    }

    public async Task<List<AppointmentDto>> GetAppointmentsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<AppointmentDto>>(
                "api/appointments",
                JsonOptions)
            ?? new List<AppointmentDto>();
    }

    public async Task<PagedResultDto<AppointmentReportDto>> GetAppointmentReportsAsync(
        int pageNumber = 1,
        int pageSize = 5)
    {
        var url = $"api/admin/reports/appointments?pageNumber={pageNumber}&pageSize={pageSize}";

        return await httpClient.GetFromJsonAsync<PagedResultDto<AppointmentReportDto>>(url, JsonOptions)
            ?? new PagedResultDto<AppointmentReportDto>
            {
                Items = new List<AppointmentReportDto>(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = 0
            };
    }

    public async Task<PagedResultDto<PatientDto>> GetPatientsAsync(
        string? search = null,
        GenderType? gender = null,
        bool? hasInsurance = null,
        int pageNumber = 1,
        int pageSize = 5)
    {
        var queryParams = new List<string>
        {
            $"pageNumber={pageNumber}",
            $"pageSize={pageSize}"
        };

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
            queryParams.Add($"hasInsurance={hasInsurance.Value.ToString().ToLowerInvariant()}");
        }

        var url = $"api/admin/patients?{string.Join("&", queryParams)}";

        return await httpClient.GetFromJsonAsync<PagedResultDto<PatientDto>>(url, JsonOptions)
            ?? new PagedResultDto<PatientDto>
            {
                Items = new List<PatientDto>(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = 0
            };
    }
}
