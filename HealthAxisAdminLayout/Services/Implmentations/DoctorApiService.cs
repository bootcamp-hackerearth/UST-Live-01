using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Doctor;
using HealthAxisAdminLayout.Services.Interfaces;
using System.Net.Http.Json;

namespace HealthAxisAdminLayout.Services.Implementations
{
    public class DoctorApiService : IDoctorApiService
    {
        private readonly HttpClient _http;

        public DoctorApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<DoctorResponseDTO>> GetDoctorsAsync()
        {
            var result = await _http.GetFromJsonAsync<List<DoctorResponseDTO>>("api/doctor");

            return result ?? new List<DoctorResponseDTO>();
        }
        public async Task<PagedResponseDTO<DoctorResponseDTO>> GetDoctorsPagedAsync(
    int pageNumber,
    int pageSize,
    string? search,
    string? specialisation,
    string? status)
        {
            var query = new List<string>
    {
        $"pageNumber={pageNumber}",
        $"pageSize={pageSize}"
    };

            if (!string.IsNullOrWhiteSpace(search))
            {
                query.Add($"search={Uri.EscapeDataString(search)}");
            }

            if (!string.IsNullOrWhiteSpace(specialisation) && specialisation != "All")
            {
                query.Add($"specialisation={Uri.EscapeDataString(specialisation)}");
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
                query.Add($"status={Uri.EscapeDataString(status)}");
            }

            var url = $"api/doctor/paged?{string.Join("&", query)}";

            var result = await _http.GetFromJsonAsync<PagedResponseDTO<DoctorResponseDTO>>(url);

            return result ?? new PagedResponseDTO<DoctorResponseDTO>();
        }
        public async Task<DoctorResponseDTO?> GetDoctorByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<DoctorResponseDTO>($"api/doctor/{id}");
        }

        public async Task<bool> CreateDoctorAsync(CreateDoctorDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/doctor", dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoctorAsync(int id, CreateDoctorDTO dto)
        {
            var response = await _http.PutAsJsonAsync($"api/doctor/{id}", dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/doctor/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SetDoctorStatusAsync(int doctorId, bool status)
        {
            var response = await _http.PatchAsync(
                $"api/doctor/status/{doctorId}?status={status}",
                null
            );

            return response.IsSuccessStatusCode;
        }
    }
}