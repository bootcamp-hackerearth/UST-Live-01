using HealthAxis_Admin.Models;
using HealthAxis.Shared.DTO.AppointmentDtos;
using System.Net.Http.Json;

namespace HealthAxis_Admin.Services
{
    public sealed class ReportService
    {
        private const string AppointmentEndpoint = "/api/appointments";

        private readonly HttpClient _httpClient;
        

        public ReportService(
            HttpClient httpClient,
            AuthService authService)
        {
            _httpClient = httpClient;
            
        }

        public async Task<List<AppointmentReportDto>> GetAppointmentReportsAsync()
        {
           
            var appointments = await _httpClient.GetFromJsonAsync<List<AppointmentDto>>(
                AppointmentEndpoint);

            if (appointments is null)
            {
                return new List<AppointmentReportDto>();
            }

            return appointments
                .Select(appointment => new AppointmentReportDto
                {
                    AppointmentId = appointment.AppointmentId,
                    PatientName = appointment.PatientName,
                    DoctorName = appointment.DoctorName,
                    Specialisation = appointment.Specialisation.ToString(),
                    AppointmentDate = appointment.ScheduledDate,
                    TimeSlot = appointment.TimeSlot,
                    Status = appointment.Status.ToString()
                })
                .ToList();
        }
    }
}
