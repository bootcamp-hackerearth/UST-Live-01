using S3_HealthAxis.Shared.DTOs.Doctor;
using System.Net.Http.Json;

namespace S3_HealthAxis.Blazor.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly HttpClient _httpClient;

        public DoctorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DoctorDto>?> GetAllAsync(string? sortBy = null, int? specialisation = null)
        {
            var url = "api/Doctors";
            var query = new List<string>();

            if (!string.IsNullOrWhiteSpace(sortBy))
                query.Add($"sortBy={sortBy}");

            if (specialisation.HasValue)
                query.Add($"specialisation={specialisation.Value}");

            if (query.Any())
                url += "?" + string.Join("&", query);

            return await _httpClient.GetFromJsonAsync<List<DoctorDto>>(url);
        }

        public async Task<DoctorDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<DoctorDto>($"api/Doctors/{id}");
        }

        public async Task<DoctorCreationResultDto?> CreateAsync(CreateDoctorDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Doctors", dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<DoctorCreationResultDto>();
            }
            return null; // Return null if creation failed (e.g. duplicate email)
        }

        public async Task<bool> UpdateAsync(int id, UpdateDoctorDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Doctors/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActivateAsync(int id)
        {
            var response = await _httpClient.PutAsync($"api/Doctors/{id}/activate", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var response = await _httpClient.PutAsync($"api/Doctors/{id}/deactivate", null);
            return response.IsSuccessStatusCode;
        }
    }
}