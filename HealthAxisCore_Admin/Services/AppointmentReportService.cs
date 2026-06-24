using HealthAxisCore_Admin.Dtos.Reports;
using System.Net.Http.Json;

namespace HealthAxisCore_Admin.Services;

public class AppointmentReportService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public AppointmentReportService(
        HttpClient httpClient,
        AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<List<AppointmentReportDto>> GetAppointmentReportsAsync()
    {
        await _authService.AddBearerTokenAsync();

        var reports = await _httpClient.GetFromJsonAsync<List<AppointmentReportDto>>(
            "api/admin/reports/appointments");

        return reports ?? new List<AppointmentReportDto>();
    }
}