using System.Net.Http.Json;
using HealthCare.Api.DTOs.Doctor;
using HealthCare.Shared.DTOs;
using System.Text.Json;

namespace HealthCare.Admin.Services
{
    public class DoctorService
    {
        private readonly HttpClient _httpClient;

        public DoctorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedResult<DoctorListDto>> GetDoctorsAsync(
            DoctorFilter filter)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                queryParams.Add(
                    $"Search={Uri.EscapeDataString(filter.Search)}");
            }

            if (!string.IsNullOrWhiteSpace(filter.Specialisation))
            {
                queryParams.Add(
                    $"Specialisation={Uri.EscapeDataString(filter.Specialisation)}");
            }

            if (filter.IsActive.HasValue)
            {
                queryParams.Add(
                    $"IsActive={filter.IsActive.Value.ToString().ToLower()}");
            }

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                queryParams.Add(
                    $"SortBy={Uri.EscapeDataString(filter.SortBy)}");
            }

            queryParams.Add(
                $"IsDescending={filter.IsDescending.ToString().ToLower()}");

            queryParams.Add($"PageNumber={filter.PageNumber}");
            queryParams.Add($"PageSize={filter.PageSize}");

            var queryString = string.Join("&", queryParams);
            var url = $"api/admin/doctors?{queryString}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new InvalidOperationException(
                    $"API Error: {(int)response.StatusCode} " +
                    $"{response.ReasonPhrase}. {error}");
            }

            var result = await response.Content
                .ReadFromJsonAsync<PagedResult<DoctorListDto>>();

            return result ?? new PagedResult<DoctorListDto>();
        }

        public async Task<DoctorListDto?> GetDoctorByIdAsync(
            int doctorId)
        {
            var response = await _httpClient.GetAsync(
                $"api/admin/doctors/{doctorId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new InvalidOperationException(
                    $"Failed to fetch doctor. " +
                    $"Status: {(int)response.StatusCode} " +
                    $"{response.ReasonPhrase}. {error}");
            }

            return await response.Content
                .ReadFromJsonAsync<DoctorListDto>();
        }

        public async Task<bool> UpdateDoctorAsync(
            int doctorId,
            UpdateDoctorDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/admin/doctors/{doctorId}",
                dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new InvalidOperationException(
                    $"Doctor update failed. " +
                    $"Status: {(int)response.StatusCode} " +
                    $"{response.ReasonPhrase}. {error}");
            }

            return true;
        }

        public async Task<bool> UpdateDoctorStatusAsync(
            int doctorId,
            bool isActive)
        {
            var response = await _httpClient.PatchAsJsonAsync(
                $"api/admin/doctors/{doctorId}/status",
                isActive);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new InvalidOperationException(
                    $"Doctor status update failed. " +
                    $"Status: {(int)response.StatusCode} " +
                    $"{response.ReasonPhrase}. {error}");
            }

            return true;
        }

        public async Task<bool> DeleteDoctorAsync(int doctorId)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/admin/doctors/{doctorId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new InvalidOperationException(
                    $"Doctor deletion failed. " +
                    $"Status: {(int)response.StatusCode} " +
                    $"{response.ReasonPhrase}. {error}");
            }

            return true;
        }

        public async Task<bool> CreateDoctorAsync(
            CreateDoctorDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/auth/register/doctor",
                dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new InvalidOperationException(
                    $"Registration failed. " +
                    $"Status: {(int)response.StatusCode} " +
                    $"{response.ReasonPhrase}. {error}");
            }

            return true;
        }
    }
}