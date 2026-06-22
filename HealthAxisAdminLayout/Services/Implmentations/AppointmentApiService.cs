using HealthAxisAdminLayout.DTOs.Appointment;
using HealthAxisAdminLayout.Services.Interfaces;

namespace HealthAxisAdminLayout.Services.Implementations
{
    public class AppointmentApiService : IAppointmentApiService
    {
        private readonly List<AppointmentResponseDTO> _appointments = new()
        {
            new AppointmentResponseDTO
            {
                AppointmentId = 1,
                PatientId = 1,
                DoctorId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM",
                Status = 0
            },
            new AppointmentResponseDTO
            {
                AppointmentId = 2,
                PatientId = 2,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(2),
                TimeSlot = "11:00 AM",
                Status = 1
            }
        };

        public Task<List<AppointmentResponseDTO>> GetAppointmentsAsync()
        {
            return Task.FromResult(_appointments.ToList());
        }

        public Task<bool> ConfirmAppointmentAsync(int appointmentId)
        {
            var appointment = _appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment == null)
                return Task.FromResult(false);

            appointment.Status = 1;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            var appointment = _appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment == null)
                return Task.FromResult(false);

            _appointments.Remove(appointment);
            return Task.FromResult(true);
        }
    }
}