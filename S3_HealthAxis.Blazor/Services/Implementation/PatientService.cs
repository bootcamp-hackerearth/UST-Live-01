using S3_HealthAxis.Shared.DTOs.Patient;
using System.Net.Http.Json;

namespace S3_HealthAxis.Blazor.Services
{
    public class PatientService : IPatientService
    {
        private readonly HttpClient _httpClient;

        public PatientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PatientDto>?> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PatientDto>>("api/Patients");
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<PatientDto>($"api/Patients/{id}");
        }

        public async Task<List<PatientSearchResultDto>?> SearchByNameAsync(string name)
        {
            return await _httpClient.GetFromJsonAsync<List<PatientSearchResultDto>>($"api/Patients/search?name={name}");
        }

        public async Task<PatientDto?> CreateAsync(CreatePatientDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Patients", dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PatientDto>();
            }
            // Throw exception to be caught and displayed by the UI
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> UpdateAsync(int id, UpdatePatientDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Patients/{id}", dto);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> ActivateAsync(int id)
        {
            var response = await _httpClient.PutAsync($"api/Patients/{id}/activate", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var response = await _httpClient.PutAsync($"api/Patients/{id}/deactivate", null);
            return response.IsSuccessStatusCode;
        }
    }
}