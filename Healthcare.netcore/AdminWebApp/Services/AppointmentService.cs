using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.DTOs.Common;
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

        public async Task<PagedResponse<AppointmentDto>?> GetAppointmentsAsync(
            int pageNumber = 1,
            int pageSize = 100)
        {
            await AttachTokenAsync();

            return await _http.GetFromJsonAsync<PagedResponse<AppointmentDto>>(
                $"api/appointment?pageNumber={pageNumber}&pageSize={pageSize}");
        }

        public async Task<HttpResponseMessage> AddAppointmentAsync(CreateAppointmentDto appointment)
        {
            await AttachTokenAsync();

            return await _http.PostAsJsonAsync("api/appointment", appointment);
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