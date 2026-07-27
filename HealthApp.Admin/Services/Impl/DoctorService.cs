using HealthApp.Admin.Services.Interface;
using HealthApp.Shared.Dto;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HealthApp.Admin.Services.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public DoctorService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private void AddAuthHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(_authService.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _authService.Token);
            }
        }

        private class ErrorResponse
        {
            public string Message { get; set; } = "";
        }

        public async Task<PagedResponse<DoctorDto>> GetAllDoctorsAsync(int pageNumber, int pageSize)
        {
            AddAuthHeader();

            var response = await _httpClient.GetAsync(
                $"api/doctors/all?pageNumber={pageNumber}&pageSize={pageSize}");

            if (!response.IsSuccessStatusCode)
                return new PagedResponse<DoctorDto>();

            return await response.Content.ReadFromJsonAsync<PagedResponse<DoctorDto>>()
                   ?? new PagedResponse<DoctorDto>();
        }

        public async Task<PagedResponse<DoctorDto>> GetActiveDoctorsAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetAsync(
                $"api/doctors/activedoctors?pageNumber={pageNumber}&pageSize={pageSize}");

            if (!response.IsSuccessStatusCode)
                return new PagedResponse<DoctorDto>();

            return await response.Content.ReadFromJsonAsync<PagedResponse<DoctorDto>>()
                   ?? new PagedResponse<DoctorDto>();
        }

        public async Task<PagedResponse<DoctorDto>> SearchBySpecialisationAsync(
            string type, int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetAsync(
                $"api/doctors/specialisation/{type}?pageNumber={pageNumber}&pageSize={pageSize}");

            if (!response.IsSuccessStatusCode)
                return new PagedResponse<DoctorDto>();

            return await response.Content.ReadFromJsonAsync<PagedResponse<DoctorDto>>()
                   ?? new PagedResponse<DoctorDto>();
        }

        public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
        {
            AddAuthHeader();

            var response = await _httpClient.GetAsync($"api/doctors/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<DoctorDto>();
        }

        public async Task<int> GetDoctorCountAsync()
        {
            var result = await GetAllDoctorsAsync(1, 1);
            return result.TotalRecords;
        }

        public async Task<(bool Success, string Message)> UpdateDoctorAsync(int id, DoctorDto dto)
        {
            try
            {
                AddAuthHeader();

                var response = await _httpClient.PutAsJsonAsync($"api/doctors/{id}", dto);

                if (response.IsSuccessStatusCode)
                    return (true, "Doctor updated successfully");

                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

                return (false, error?.Message ?? "Update failed");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> CreateDoctorAsync(DoctorDto dto)
        {
            try
            {
                AddAuthHeader();

                var request = new DoctorRegisterDto
                {
                    FullName = dto.FullName!,
                    Email = dto.Email!,
                    Specialisation = dto.Specialisation!,
                    DoctorPhoneNumber = dto.DoctorPhoneNumber,
                    ConsultationFee = dto.ConsultationFee,
                    PracticeStartDate = dto.PracticeStartDate,
                    IsActive = dto.IsActive
                };

                var response = await _httpClient
                    .PostAsJsonAsync("api/auth/doctorregister", request);

                if (response.IsSuccessStatusCode)
                    return (true, "Doctor created successfully");

                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

                return (false, error?.Message ?? "Something went wrong");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}