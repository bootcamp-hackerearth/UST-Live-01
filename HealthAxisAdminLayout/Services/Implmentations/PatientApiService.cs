using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Patient;
using HealthAxisAdminLayout.Services.Interfaces;

namespace HealthAxisAdminLayout.Services.Implementations
{
    public class PatientApiService : IPatientApiService
    {
        private readonly HttpClient _http;

        public PatientApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<PatientResponseDto>> GetPatientsAsync()
        {
            var result = await _http.GetFromJsonAsync<List<PatientResponseDto>>(
                "patient"
            );

            return result ?? new List<PatientResponseDto>();
        }

        public async Task<PatientResponseDto?> GetPatientByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<PatientResponseDto>(
                $"patient/{id}"
            );
        }

        public async Task<List<PatientResponseDto>> SearchPatientsAsync(string? name, string? email)
        {
            var query = new List<string>();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query.Add($"name={Uri.EscapeDataString(name)}");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query.Add($"email={Uri.EscapeDataString(email)}");
            }

            var url = query.Count == 0
                ? "patient/search"
                : $"patient/search?{string.Join("&", query)}";

            var result = await _http.GetFromJsonAsync<List<PatientResponseDto>>(url);

            return result ?? new List<PatientResponseDto>();
        }

        public async Task<PagedResponseDto<PatientResponseDto>> GetPatientsPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            string? gender)
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

            if (!string.IsNullOrWhiteSpace(gender) && gender != "All")
            {
                query.Add($"gender={Uri.EscapeDataString(gender)}");
            }

            var url = $"patient/paged?{string.Join("&", query)}";

            var result = await _http.GetFromJsonAsync<PagedResponseDto<PatientResponseDto>>(url);

            return result ?? new PagedResponseDto<PatientResponseDto>();
        }

        public async Task<bool> CreatePatientAsync(CreatePatientDto dto)
        {
            var response = await _http.PostAsJsonAsync(
                "patient",
                dto
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePatientAsync(int id, UpdatePatientDto dto)
        {
            var response = await _http.PutAsJsonAsync(
                $"patient/{id}",
                dto
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var response = await _http.DeleteAsync(
                $"patient/{id}"
            );

            return response.IsSuccessStatusCode;
        }
    }
}

