using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Doctor;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AdminWebApp.Services
{
    public class DoctorService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public DoctorService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        public async Task<PagedResponse<DoctorDto>?> GetDoctorsAsync(
            int pageNumber = 1,
            int pageSize = 100)
        {
            await AttachTokenAsync();

            return await _http.GetFromJsonAsync<PagedResponse<DoctorDto>>(
                $"api/doctors?pageNumber={pageNumber}&pageSize={pageSize}");
        }

        public async Task<HttpResponseMessage> AddDoctorAsync(CreateDoctorDto doctor)
        {
            await AttachTokenAsync();

            return await _http.PostAsJsonAsync("api/doctors", doctor);
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