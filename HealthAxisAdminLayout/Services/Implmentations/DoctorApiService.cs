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

        public async Task<List<DoctorResponseDto>> GetDoctorsAsync()
        {
            var result = await _http.GetFromJsonAsync<List<DoctorResponseDto>>("doctor");

            return result ?? new List<DoctorResponseDto>();
        }

        public async Task<PagedResponseDto<DoctorResponseDto>> GetDoctorsPagedAsync(
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

            var url = $"doctor/paged?{string.Join("&", query)}";

            var result = await _http.GetFromJsonAsync<PagedResponseDto<DoctorResponseDto>>(url);

            return result ?? new PagedResponseDto<DoctorResponseDto>();
        }

        public async Task<DoctorResponseDto?> GetDoctorByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<DoctorResponseDto>($"doctor/{id}");
        }

        public async Task<CreateDoctorResultDto?> CreateDoctorAsync(CreateDoctorDto dto)
        {
            var response = await _http.PostAsJsonAsync("doctor", dto);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<CreateDoctorResultDto>();

            return result;
        }

        public async Task<bool> UpdateDoctorAsync(int id, CreateDoctorDto dto)
        {
            var response = await _http.PutAsJsonAsync($"doctor/{id}", dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var response = await _http.DeleteAsync($"doctor/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SetDoctorStatusAsync(int doctorId, bool status)
        {
            var response = await _http.PatchAsync(
                $"doctor/status/{doctorId}?status={status}",
                null
            );

            return response.IsSuccessStatusCode;
        }
    }
}

