using HealthCareApp.AdminBlazor.Dtos.Appointments;
using HealthCareApp.AdminBlazor.Services.Interfaces;

namespace HealthCareApp.AdminBlazor.Services.Impl
{
    public class AppointmentAdminService : IAppointmentAdminService
    {
        private static readonly List<AppointmentDto> Appointments = new()
        {
            new AppointmentDto
            {
                AppointmentId = 1,
                PatientId = 1,
                PatientName = "Ravi Kumar",
                DoctorId = 1,
                DoctorName = "Arun Menon",
                ScheduledDate = DateTime.Today,
                TimeSlot = "10:00 AM - 10:30 AM",
                Status = AppointmentStatusDto.Pending,
                CancellationReason = null,
                CreatedDate = DateTime.Today.AddDays(-2)
            },
            new AppointmentDto
            {
                AppointmentId = 2,
                PatientId = 2,
                PatientName = "Anjali Nair",
                DoctorId = 2,
                DoctorName = "Meera Nair",
                ScheduledDate = DateTime.Today,
                TimeSlot = "11:00 AM - 11:30 AM",
                Status = AppointmentStatusDto.Confirmed,
                CancellationReason = null,
                CreatedDate = DateTime.Today.AddDays(-1)
            },
            new AppointmentDto
            {
                AppointmentId = 3,
                PatientId = 3,
                PatientName = "Kiran Das",
                DoctorId = 3,
                DoctorName = "Vikram Das",
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "02:00 PM - 02:30 PM",
                Status = AppointmentStatusDto.Cancelled,
                CancellationReason = "Patient requested cancellation",
                CreatedDate = DateTime.Today.AddDays(-3)
            },
            new AppointmentDto
            {
                AppointmentId = 4,
                PatientId = 1,
                PatientName = "Ravi Kumar",
                DoctorId = 2,
                DoctorName = "Meera Nair",
                ScheduledDate = DateTime.Today.AddDays(-1),
                TimeSlot = "03:00 PM - 03:30 PM",
                Status = AppointmentStatusDto.Completed,
                CancellationReason = null,
                CreatedDate = DateTime.Today.AddDays(-5)
            },
            new AppointmentDto
            {
                AppointmentId = 5,
                PatientId = 2,
                PatientName = "Anjali Nair",
                DoctorId = 1,
                DoctorName = "Arun Menon",
                ScheduledDate = DateTime.Today.AddDays(3),
                TimeSlot = "09:00 AM - 09:30 AM",
                Status = AppointmentStatusDto.Pending,
                CancellationReason = null,
                CreatedDate = DateTime.Today
            }
        };

        public Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        {
            var appointments = Appointments
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();

            return Task.FromResult(appointments);
        }

        public Task<AppointmentDto?> GetAppointmentByIdAsync(int appointmentId)
        {
            var appointment = Appointments
                .FirstOrDefault(a => a.AppointmentId == appointmentId);

            return Task.FromResult(appointment);
        }

        public Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto request)
        {
            int nextId = Appointments.Any()
                ? Appointments.Max(a => a.AppointmentId) + 1
                : 1;

            var appointment = new AppointmentDto
            {
                AppointmentId = nextId,
                PatientId = request.PatientId,
                PatientName = GetPatientName(request.PatientId),
                DoctorId = request.DoctorId,
                DoctorName = GetDoctorName(request.DoctorId),
                ScheduledDate = request.ScheduledDate.Date,
                TimeSlot = request.TimeSlot,
                Status = request.Status,
                CancellationReason = request.Status == AppointmentStatusDto.Cancelled
                    ? "Cancelled by admin"
                    : null,
                CreatedDate = DateTime.Today
            };

            Appointments.Add(appointment);

            return Task.FromResult(appointment);
        }

        public Task<AppointmentDto?> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto request)
        {
            var appointment = Appointments
                .FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment is null)
            {
                return Task.FromResult<AppointmentDto?>(null);
            }

            appointment.PatientId = request.PatientId;
            appointment.PatientName = GetPatientName(request.PatientId);
            appointment.DoctorId = request.DoctorId;
            appointment.DoctorName = GetDoctorName(request.DoctorId);
            appointment.ScheduledDate = request.ScheduledDate.Date;
            appointment.TimeSlot = request.TimeSlot;
            appointment.Status = request.Status;
            appointment.CancellationReason = request.Status == AppointmentStatusDto.Cancelled
                ? request.CancellationReason
                : null;

            return Task.FromResult<AppointmentDto?>(appointment);
        }

        public Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            var appointment = Appointments
                .FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment is null)
            {
                return Task.FromResult(false);
            }

            Appointments.Remove(appointment);

            return Task.FromResult(true);
        }

        private static string GetPatientName(int patientId)
        {
            return patientId switch
            {
                1 => "Ravi Kumar",
                2 => "Anjali Nair",
                3 => "Kiran Das",
                4 => "Ownership Patient",
                _ => $"Patient #{patientId}"
            };
        }

        private static string GetDoctorName(int doctorId)
        {
            return doctorId switch
            {
                1 => "Arun Menon",
                2 => "Meera Nair",
                3 => "Vikram Das",
                4 => "Rishi Doctor",
                _ => $"Doctor #{doctorId}"
            };
        }
    }
}
