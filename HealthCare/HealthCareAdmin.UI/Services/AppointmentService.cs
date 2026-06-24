
using Healthcare.Shared.DTOs.Appointment;
using Healthcare.Shared.DTOs.Appointments;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class AppointmentService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;

    public AppointmentService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task<List<AppointmentReportDto>> GetAppointmentReport(DateTime start, DateTime end)
    {
        var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var url = $"appointments?StartDate={start:yyyy-MM-dd}&EndDate={end:yyyy-MM-dd}";


        var result = await _http.GetFromJsonAsync<PagedAppointmentResponse>(url);
        return result?.Items ?? new List<AppointmentReportDto>();

    }
}
