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

        // ✅ GET ALL
        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        {
            try
            {
                await PrepareRequest();

                var response = await _httpClient.GetAsync("/api/appointments");

                await HandleUnauthorized(response);

                if (!response.IsSuccessStatusCode)
                    return new List<AppointmentDto>();

                return await response.Content.ReadFromJsonAsync<List<AppointmentDto>>()
                       ?? new List<AppointmentDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return new List<AppointmentDto>();
            }
        }

        // ✅ GET BY ID
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

        // ✅ AVAILABILITY
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

        // ✅ UPCOMING
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

        // ✅ FILTER
        public async Task<List<AppointmentDto>> GetByPatientDoctorAsync(int patientId, int doctorId)
        {
            try
            {
                await PrepareRequest();

                var response = await _httpClient.GetAsync(
                    $"/api/appointments/by-patient-doctor?patientId={patientId}&doctorId={doctorId}");

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

        // ✅ COUNT
        public async Task<int> GetAppointmentCountAsync()
        {
            var data = await GetAllAppointmentsAsync();
            return data.Count;
        }
    }
}
