using HealthApp.Blazor.Components.service.Interface;
using HealthApp.Shared.Dto;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

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

        private async Task PrepareRequestAsync()
        {
            await _authService.InitializeAsync();

            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(_authService.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _authService.Token);
            }
        }

        private async Task HandleUnauthorizedAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _authService.LogoutAsync();
                throw new UnauthorizedAccessException("Session expired. Please login again.");
            }
        }

        private async Task<T> GetAsync<T>(string url, T fallbackValue)
        {
            try
            {
                await PrepareRequestAsync();

                using var response = await _httpClient.GetAsync(url);

                await HandleUnauthorizedAsync(response);

                if (!response.IsSuccessStatusCode)
                {
                    return fallbackValue;
                }

                return await response.Content.ReadFromJsonAsync<T>() ?? fallbackValue;
            }
            catch (HttpRequestException)
            {
                return fallbackValue;
            }
            catch (NotSupportedException)
            {
                return fallbackValue;
            }
            catch (JsonException)
            {
                return fallbackValue;
            }
        }

        public Task<PagedResponse<AppointmentDto>> GetPagedAppointmentsAsync(
            int pageNumber,
            int pageSize)
        {
            var url = $"/api/appointments/paged?pageNumber={pageNumber}&pageSize={pageSize}";

            return GetAsync(url, new PagedResponse<AppointmentDto>());
        }

        public Task<PagedResponse<AppointmentDto>> GetFilteredAppointmentsAsync(
            int? patientId,
            int? doctorId,
            int pageNumber,
            int pageSize)
        {
            var url =
                $"/api/appointments/filter-paged?patientId={patientId}" +
                $"&doctorId={doctorId}" +
                $"&pageNumber={pageNumber}" +
                $"&pageSize={pageSize}";

            return GetAsync(url, new PagedResponse<AppointmentDto>());
        }

        public Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
        {
            var url = $"/api/appointments/{id}";

            return GetAsync<AppointmentDto?>(url, null);
        }

        public Task<List<string>> CheckDoctorAvailabilityAsync(int doctorId, DateTime date)
        {
            var url = $"/api/appointments/doctor/{doctorId}/availability?date={date:yyyy-MM-dd}";

            return GetAsync(url, new List<string>());
        }

        public Task<List<AppointmentDto>> GetUpcomingAppointmentsAsync(
            int doctorId,
            DateTime fromDate,
            DateTime toDate)
        {
            var url =
                $"/api/appointments/doctor/{doctorId}/upcoming" +
                $"?fromDate={fromDate:yyyy-MM-dd}" +
                $"&toDate={toDate:yyyy-MM-dd}";

            return GetAsync(url, new List<AppointmentDto>());
        }

        public async Task<int> GetAppointmentCountAsync()
        {
            var response = await GetPagedAppointmentsAsync(1, 1);
            return response.TotalRecords;
        }
    }
}