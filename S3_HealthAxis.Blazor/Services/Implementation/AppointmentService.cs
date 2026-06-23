using S3_HealthAxis.Shared.DTOs.Appointment;
using System.Net.Http.Json;

namespace S3_HealthAxis.Blazor.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly HttpClient _httpClient;

        public AppointmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AppointmentDetailsDto>?> GetAllAsync() =>
            await _httpClient.GetFromJsonAsync<List<AppointmentDetailsDto>>("api/Appointments");

        public async Task<AppointmentDetailsDto?> GetByIdAsync(int id) =>
            await _httpClient.GetFromJsonAsync<AppointmentDetailsDto>($"api/Appointments/{id}");

        public async Task<List<PatientAppointmentHistoryDto>?> GetPatientHistoryAsync(int patientId) =>
            await _httpClient.GetFromJsonAsync<List<PatientAppointmentHistoryDto>>($"api/Appointments/patient/{patientId}");

        public async Task<List<DoctorScheduleItemDto>?> GetDoctorTodayScheduleAsync(int doctorId) =>
            await _httpClient.GetFromJsonAsync<List<DoctorScheduleItemDto>>($"api/Appointments/doctor/{doctorId}/today");

        public async Task<List<DoctorScheduleItemDto>?> GetDoctorWeekScheduleAsync(int doctorId, DateOnly startDate, DateOnly endDate) =>
            await _httpClient.GetFromJsonAsync<List<DoctorScheduleItemDto>>($"api/Appointments/doctor/{doctorId}/week?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        public async Task<List<DoctorScheduleItemDto>?> GetDoctorUpcomingScheduleAsync(int doctorId) =>
            await _httpClient.GetFromJsonAsync<List<DoctorScheduleItemDto>>($"api/Appointments/doctor/{doctorId}/upcoming");

        public async Task<AppointmentDto?> CreateAsync(CreateAppointmentDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Appointments", dto);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<AppointmentDto>();

            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> UpdateAsync(int id, UpdateAppointmentDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Appointments/{id}", dto);
            if (response.IsSuccessStatusCode) return true;
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> UpdateStatusAsync(int id, UpdateAppointmentStatusDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Appointments/{id}/status", dto);
            if (response.IsSuccessStatusCode) return true;
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> ConfirmAsync(int id)
        {
            var response = await _httpClient.PutAsync($"api/Appointments/{id}/confirm", null);
            if (response.IsSuccessStatusCode) return true;
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> CompleteAsync(int id)
        {
            var response = await _httpClient.PutAsync($"api/Appointments/{id}/complete", null);
            if (response.IsSuccessStatusCode) return true;
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> CancelAsync(int id, CancelAppointmentDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Appointments/{id}/cancel", dto);
            if (response.IsSuccessStatusCode) return true;
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }
}