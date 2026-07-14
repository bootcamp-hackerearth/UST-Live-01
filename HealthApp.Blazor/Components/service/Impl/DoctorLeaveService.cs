using HealthApp.Blazor.Components.service.Interface;
using HealthApp.Shared.Dto;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HealthApp.Blazor.Components.service.Impl
{
    public class DoctorLeaveService : IDoctorLeaveService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public DoctorLeaveService(
            HttpClient httpClient,
            IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private void AddAuthHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrWhiteSpace(_authService.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _authService.Token);
            }
        }

        public async Task<PagedResponse<DoctorLeaveDto>> GetAllDoctorLeavesAsync(
            int pageNumber,
            int pageSize)
        {
            AddAuthHeader();

            var response = await _httpClient.GetAsync(
                $"api/doctor-leaves/all?pageNumber={pageNumber}&pageSize={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                return new PagedResponse<DoctorLeaveDto>();
            }

            return await response.Content
                .ReadFromJsonAsync<PagedResponse<DoctorLeaveDto>>()
                ?? new PagedResponse<DoctorLeaveDto>();
        }

        public async Task<List<DoctorLeaveDto>> GetDoctorLeavesAsync(int doctorId)
        {
            AddAuthHeader();

            var response = await _httpClient.GetAsync(
                $"api/doctor-leaves/doctor/{doctorId}");

            if (!response.IsSuccessStatusCode)
            {
                return new List<DoctorLeaveDto>();
            }

            return await response.Content
                .ReadFromJsonAsync<List<DoctorLeaveDto>>()
                ?? new List<DoctorLeaveDto>();
        }
    }
}