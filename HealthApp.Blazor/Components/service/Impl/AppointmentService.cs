using HealthApp.Blazor.Components.service.Interface;
using HealthApp.Shared.Dto;
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

        private void AddAuthHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(_authService.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _authService.Token);
            }
        }

        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        {
            Console.WriteLine($"TOKEN: {_authService.Token}");
            try
            {
                AddAuthHeader();

                var response = await _httpClient.GetAsync("/api/appointments");

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

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
        {
            try
            {
                AddAuthHeader();

                var response = await _httpClient.GetAsync($"/api/appointments/{id}");

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
                AddAuthHeader();

                var response = await _httpClient.GetAsync(
                    $"/api/appointments/doctor/{doctorId}/availability?date={date:yyyy-MM-dd}");

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
                AddAuthHeader();

                var response = await _httpClient.GetAsync(
                    $"/api/appointments/doctor/{doctorId}/upcoming?fromDate={fromDate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd}");

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

        public async Task<List<AppointmentDto>> GetByPatientDoctorAsync(int patientId, int doctorId)
        {
            try
            {
                AddAuthHeader();

                var response = await _httpClient.GetAsync(
                    $"/api/appointments/by-patient-doctor?patientId={patientId}&doctorId={doctorId}");

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
            try
            {
                var appointment = await GetAllAppointmentsAsync();
                return appointment.Count;
            }
            catch
            {
                return 0;
            }
        }
    }
}