using HealthApp.Blazor.Components.Models;

namespace HealthApp.Blazor.Components.Services
{
    public class AppointmentService
    {
        private readonly List<Appointment> _appointments = new()
        {
            new Appointment
            {
                AppointmentId = 1,
                DoctorId = 1,
                PatientId = 1,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM - 10:30 AM",
                Status = "Confirmed",
                CancellationReason = null,
                Doctor = new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr. Anil Kumar"
                },
                Patient = new Patient
                {
                    PatientId = 1,
                    FullName = "Arun S"
                }
            },
            new Appointment
            {
                AppointmentId = 2,
                DoctorId = 2,
                PatientId = 2,
                ScheduledDate = DateTime.Today.AddDays(2),
                TimeSlot = "11:00 AM - 11:30 AM",
                Status = "Pending",
                CancellationReason = null,
                Doctor = new Doctor
                {
                    DoctorId = 2,
                    FullName = "Dr. Meera Nair"
                },
                Patient = new Patient
                {
                    PatientId = 2,
                    FullName = "Divya R"
                }
            },
            new Appointment
            {
                AppointmentId = 3,
                DoctorId = 1,
                PatientId = 3,
                ScheduledDate = DateTime.Today.AddDays(-1),
                TimeSlot = "09:00 AM - 09:30 AM",
                Status = "Completed",
                CancellationReason = null,
                Doctor = new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr. Anil Kumar"
                },
                Patient = new Patient
                {
                    PatientId = 3,
                    FullName = "Kiran P"
                }
            }
        };

        public Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return Task.FromResult(_appointments.ToList());
        }

        public Task<int> GetAppointmentCountAsync()
        {
            return Task.FromResult(_appointments.Count);
        }

        public Task<Appointment?> GetAppointmentByIdAsync(int id)
        {
            var appointment = _appointments.FirstOrDefault(a => a.AppointmentId == id);
            return Task.FromResult(appointment);
        }

        public Task<List<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            var result = _appointments
                .Where(a => a.DoctorId == doctorId)
                .ToList();

            return Task.FromResult(result);
        }

        public Task<List<Appointment>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            var result = _appointments
                .Where(a => a.PatientId == patientId)
                .ToList();

            return Task.FromResult(result);
        }

        public Task<List<Appointment>> GetUpcomingAppointmentsAsync()
        {
            var result = _appointments
                .Where(a => a.ScheduledDate.Date >= DateTime.Today)
                .OrderBy(a => a.ScheduledDate)
                .ToList();

            return Task.FromResult(result);
        }

        public Task<Appointment> AddAppointmentAsync(Appointment appointment)
        {
            appointment.AppointmentId = _appointments.Any()
                ? _appointments.Max(a => a.AppointmentId) + 1
                : 1;

            _appointments.Add(appointment);

            return Task.FromResult(appointment);
        }

        public Task<Appointment?> CancelAppointmentAsync(int appointmentId, string reason)
        {
            var existingAppointment = _appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (existingAppointment == null)
            {
                return Task.FromResult<Appointment?>(null);
            }

            existingAppointment.Status = "Cancelled";
            existingAppointment.CancellationReason = reason;

            return Task.FromResult<Appointment?>(existingAppointment);
        }
    }
}