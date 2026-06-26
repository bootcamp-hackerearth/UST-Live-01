using Healthcare.Shared.DTOs.Appointments;
using System.Net.Http.Json;

namespace HealthCareAdmin.UI.Services
{
    public class AppointmentService
    {
        private readonly HttpClient _http;

        public AppointmentService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<AppointmentReportDto>> GetAppointmentReport(DateTime start, DateTime end)
        {
            var url = $"api/admin/appointments/report?startDate={start:yyyy-MM-dd}&endDate={end:yyyy-MM-dd}";

            var result = await _http.GetFromJsonAsync<List<AppointmentReportDto>>(url);

            return result ?? new List<AppointmentReportDto>();
        }

        public async Task<AppointmentSummaryDto> GetDashboard()
        {
            return await _http.GetFromJsonAsync<AppointmentSummaryDto>("api/admin/dashboard")
                   ?? new AppointmentSummaryDto();
        }
    }
}