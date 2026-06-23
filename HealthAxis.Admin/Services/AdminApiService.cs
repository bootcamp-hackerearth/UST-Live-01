using HealthAxis.API.DTOs.Admin;
using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.DTOs.CommonDtos;
using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.DTOs.Patients;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace HealthAxis.Admin.Services
{
    public class AdminApiService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AdminApiService(
            HttpClient httpClient,
            TokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<List<DoctorReadDto>> GetDoctorsAsync()
        {
            await AddBearerTokenAsync();

            HttpResponseMessage response =
                await _httpClient.GetAsync("api/admin/doctors");

            await EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<List<DoctorReadDto>>(
                _jsonOptions) ?? new List<DoctorReadDto>();
        }

        public async Task<DoctorReadDto> CreateDoctorAsync(
            AdminDoctorCreateDto request)
        {
            await AddBearerTokenAsync();

            HttpResponseMessage response =
                await _httpClient.PostAsJsonAsync(
                    "api/admin/doctors",
                    request);

            await EnsureSuccessAsync(response);

            DoctorReadDto? doctor =
                await response.Content.ReadFromJsonAsync<DoctorReadDto>(
                    _jsonOptions);

            return doctor
                ?? throw new InvalidOperationException(
                    "Invalid doctor response received.");
        }

        public async Task<DoctorReadDto> UpdateDoctorAsync(
            int doctorId,
            DoctorUpdateDto request)
        {
            await AddBearerTokenAsync();

            HttpResponseMessage response =
                await _httpClient.PutAsJsonAsync(
                    $"api/admin/doctors/{doctorId}",
                    request);

            await EnsureSuccessAsync(response);

            DoctorReadDto? doctor =
                await response.Content.ReadFromJsonAsync<DoctorReadDto>(
                    _jsonOptions);

            return doctor
                ?? throw new InvalidOperationException(
                    "Invalid doctor response received.");
        }

        public async Task<List<PatientReadDto>> GetPatientsAsync()
        {
            await AddBearerTokenAsync();

            HttpResponseMessage response =
                await _httpClient.GetAsync("api/admin/patients");

            await EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<List<PatientReadDto>>(
                _jsonOptions) ?? new List<PatientReadDto>();
        }

        public async Task<PatientReadDto> UpdatePatientAsync(
            int patientId,
            PatientUpdateDto request)
        {
            await AddBearerTokenAsync();

            HttpResponseMessage response =
                await _httpClient.PutAsJsonAsync(
                    $"api/admin/patients/{patientId}",
                    request);

            await EnsureSuccessAsync(response);

            PatientReadDto? patient =
                await response.Content.ReadFromJsonAsync<PatientReadDto>(
                    _jsonOptions);

            return patient
                ?? throw new InvalidOperationException(
                    "Invalid patient response received.");
        }

        public async Task<List<AppointmentReportDto>> GetAppointmentReportsAsync()
        {
            await AddBearerTokenAsync();

            HttpResponseMessage response =
                await _httpClient.GetAsync("api/admin/reports/appointments");

            await EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<List<AppointmentReportDto>>(
                _jsonOptions) ?? new List<AppointmentReportDto>();
        }

        public async Task<AdminDashboardSummary> GetDashboardSummaryAsync()
        {
            await AddBearerTokenAsync();

            Task<List<DoctorReadDto>> doctorsTask =
                GetDoctorsAsync();

            Task<List<PatientReadDto>> patientsTask =
                GetPatientsAsync();

            Task<List<AppointmentReportDto>> reportsTask =
                GetAppointmentReportsAsync();

            await Task.WhenAll(
                doctorsTask,
                patientsTask,
                reportsTask);

            List<DoctorReadDto> doctors =
                await doctorsTask;

            List<PatientReadDto> patients =
                await patientsTask;

            List<AppointmentReportDto> reports =
                await reportsTask;

            return new AdminDashboardSummary
            {
                TotalDoctors = doctors.Count,

                ActiveDoctors = doctors.Count(doctor =>
                    doctor.IsActive),

                TotalPatients = patients.Count,

                CompletedAppointments = reports.Sum(report =>
                    report.CompletedCount),

                LastRefreshedAt = DateTime.Now
            };
        }

        private async Task AddBearerTokenAsync()
        {
            string? token = await _tokenService.GetAccessTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new UnauthorizedAccessException(
                    "Login token was not found. Please login again.");
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task EnsureSuccessAsync(
            HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            string message =
                await GetErrorMessageAsync(response);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException(message);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new InvalidOperationException(
                    "You do not have permission to access this admin resource.");
            }

            throw new InvalidOperationException(message);
        }

        private async Task<string> GetErrorMessageAsync(
            HttpResponseMessage response)
        {
            try
            {
                ErrorResponseDto? error =
                    await response.Content.ReadFromJsonAsync<ErrorResponseDto>(
                        _jsonOptions);

                if (!string.IsNullOrWhiteSpace(error?.Message))
                {
                    return error.Message;
                }
            }
            catch
            {
                // Ignore parsing issues and return generic message below.
            }

            return $"Request failed with status code {(int)response.StatusCode}.";
        }
    }

    public class AdminDashboardSummary
    {
        public int TotalDoctors { get; set; }

        public int ActiveDoctors { get; set; }

        public int TotalPatients { get; set; }

        public int CompletedAppointments { get; set; }

        public DateTime LastRefreshedAt { get; set; }
    }
}
