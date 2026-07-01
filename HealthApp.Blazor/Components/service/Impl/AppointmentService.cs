using HealthApp.Blazor.Components.service.Interface;
using HealthApp.Shared.Dto;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HealthApp.Blazor.Components.service.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public AppointmentService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private async Task PrepareRequest()
        {
            await _authService.InitializeAsync();

            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(_authService.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _authService.Token);
            }
        }

        private async Task HandleUnauthorized(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _authService.LogoutAsync();
                throw new Exception("Session expired. Please login again.");
            }
        }

        public async Task<PagedResponse<AppointmentDto>> GetPagedAppointmentsAsync(
            int pageNumber,
            int pageSize)
        {
            try
            {
                await PrepareRequest();

                var response = await _httpClient.GetAsync(
                    $"/api/appointments/paged?pageNumber={pageNumber}&pageSize={pageSize}");

                await HandleUnauthorized(response);

                if (!response.IsSuccessStatusCode)
                    return new PagedResponse<AppointmentDto>();

                return await response.Content.ReadFromJsonAsync<PagedResponse<AppointmentDto>>()
                       ?? new PagedResponse<AppointmentDto>();
            }
            catch
            {
                return new PagedResponse<AppointmentDto>();
            }
        }

        public async Task<PagedResponse<AppointmentDto>> GetFilteredAppointmentsAsync(
            int? patientId,
            int? doctorId,
            int pageNumber,
            int pageSize)
        {
            try
            {
                await PrepareRequest();

                var url =
                    $"/api/appointments/filter-paged?patientId={patientId}" +
                    $"&doctorId={doctorId}" +
                    $"&pageNumber={pageNumber}" +
                    $"&pageSize={pageSize}";

                var response = await _httpClient.GetAsync(url);

                await HandleUnauthorized(response);

                if (!response.IsSuccessStatusCode)
                    return new PagedResponse<AppointmentDto>();

                return await response.Content.ReadFromJsonAsync<PagedResponse<AppointmentDto>>()
                       ?? new PagedResponse<AppointmentDto>();
            }
            catch
            {
                return new PagedResponse<AppointmentDto>();
            }
        }

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
        {
            try
            {
                await PrepareRequest();

                var response = await _httpClient.GetAsync($"/api/appointments/{id}");

                await HandleUnauthorized(response);

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<AppointmentDto>();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<string>> CheckDoctorAvailabilityAsync(int doctorId, DateTime date)
        {
            try
            {
                await PrepareRequest();

                var response = await _httpClient.GetAsync(
                    $"/api/appointments/doctor/{doctorId}/availability?date={date:yyyy-MM-dd}");

                await HandleUnauthorized(response);

                if (!response.IsSuccessStatusCode)
                    return new List<string>();

                return await response.Content.ReadFromJsonAsync<List<string>>()
                       ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public async Task<List<AppointmentDto>> GetUpcomingAppointmentsAsync(
            int doctorId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                await PrepareRequest();

                var response = await _httpClient.GetAsync(
                    $"/api/appointments/doctor/{doctorId}/upcoming?fromDate={fromDate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd}");

                await HandleUnauthorized(response);

                if (!response.IsSuccessStatusCode)
                    return new List<AppointmentDto>();

                return await response.Content.ReadFromJsonAsync<List<AppointmentDto>>()
                       ?? new List<AppointmentDto>();
            }
            catch
            {
                return new List<AppointmentDto>();
            }
        }

        public async Task<int> GetAppointmentCountAsync()
        {
            var res = await GetPagedAppointmentsAsync(1, 1);
            return res.TotalRecords;
        }
    }
}