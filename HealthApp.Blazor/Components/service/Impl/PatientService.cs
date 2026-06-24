using HealthApp.Blazor.Components.service.Interface;
using HealthApp.Shared.Dto;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HealthApp.Blazor.Components.Services
{
    public class PatientService : IPatientService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public PatientService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private void AddAuthHeader()
        {
            if (!string.IsNullOrEmpty(_authService.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _authService.Token);
            }
        }

        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            AddAuthHeader();

            var response = await _httpClient.GetAsync("api/patientapi");

            if (!response.IsSuccessStatusCode)
            {
                return new List<PatientDto>();
            }

            return await response.Content.ReadFromJsonAsync<List<PatientDto>>()
                   ?? new List<PatientDto>();
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int id)
        {
            AddAuthHeader();

            var response = await _httpClient.GetAsync($"api/patientapi/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<PatientDto>();
        }

        public async Task<int> GetPatientCountAsync()
        {
            try
            {
                var doctors = await GetAllPatientsAsync();
                return doctors.Count;
            }
            catch
            {
                return 0;
            }
        }

    }
}
