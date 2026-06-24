using HealthAxis.Shared.DTOs.Appointment;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AdminWebApp.Services
{
    public class AppointmentService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public AppointmentService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        public async Task<List<AppointmentDto>?> GetAppointmentsAsync()
        {
            await AttachTokenAsync();

            return await _http.GetFromJsonAsync<List<AppointmentDto>>(
                "api/appointments");
        }

        public async Task<HttpResponseMessage> UpdateAppointmentStatusAsync(
            int appointmentId,
            UpdateAppointmentStatusDto dto)
        {
            await AttachTokenAsync();

            return await _http.PutAsJsonAsync(
                $"api/appointments/{appointmentId}/status",
                dto);
        }

        private async Task AttachTokenAsync()
        {
            var token = await _js.InvokeAsync<string>(
                "localStorage.getItem",
                "token");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}