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

        public async Task<List<PatientResponseDTO>> GetPatientsAsync()
        {
            var result = await _http.GetFromJsonAsync<List<PatientResponseDTO>>(
                "api/patient"
            );

            return result ?? new List<PatientResponseDTO>();
        }

        public async Task<PatientResponseDTO?> GetPatientByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<PatientResponseDTO>(
                $"api/patient/{id}"
            );
        }

        public async Task<List<PatientResponseDTO>> SearchPatientsAsync(string? name, string? email)
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
                ? "api/patient/search"
                : $"api/patient/search?{string.Join("&", query)}";

            var result = await _http.GetFromJsonAsync<List<PatientResponseDTO>>(url);

            return result ?? new List<PatientResponseDTO>();
        }

        public async Task<PagedResponseDTO<PatientResponseDTO>> GetPatientsPagedAsync(
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

            var url = $"api/patient/paged?{string.Join("&", query)}";

            var result = await _http.GetFromJsonAsync<PagedResponseDTO<PatientResponseDTO>>(url);

            return result ?? new PagedResponseDTO<PatientResponseDTO>();
        }

        public async Task<bool> CreatePatientAsync(CreatePatientDTO dto)
        {
            var response = await _http.PostAsJsonAsync(
                "api/patient",
                dto
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePatientAsync(int id, UpdatePatientDTO dto)
        {
            var response = await _http.PutAsJsonAsync(
                $"api/patient/{id}",
                dto
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var response = await _http.DeleteAsync(
                $"api/patient/{id}"
            );

            return response.IsSuccessStatusCode;
        }
    }
}