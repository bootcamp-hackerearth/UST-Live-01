using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Patient;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AdminWebApp.Services
{
    public class PatientService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public PatientService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        public async Task<PagedResponse<PatientDto>?> GetPatientsAsync(
            int pageNumber = 1,
            int pageSize = 100)
        {
            await AttachTokenAsync();

            return await _http.GetFromJsonAsync<PagedResponse<PatientDto>>(
                $"api/patients?pageNumber={pageNumber}&pageSize={pageSize}");
        }

        public async Task<HttpResponseMessage> AddPatientAsync(CreatePatientDto patient)
        {
            await AttachTokenAsync();

            return await _http.PostAsJsonAsync("api/patients", patient);
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