using Microsoft.EntityFrameworkCore;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;
using HealthAxis.Shared.Enums;

namespace HealthAxisCore_Api.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly HealthAppDbContext _context;

        public AppointmentRepository(HealthAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetByDoctor(int doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByPatient(int patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> FilterAppointments(
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            return await _context.Appointments
                .Where(a =>
                    (!status.HasValue || a.Status == status) &&
                    (!startDate.HasValue || a.ScheduledDate >= startDate) &&
                    (!endDate.HasValue || a.ScheduledDate <= endDate))
                .ToListAsync();
        }

        public async Task CancelAppointment(int id, string reason)
        {
            var appt = await _context.Appointments.FindAsync(id);

            if (appt != null)
            {
                appt.Status = AppointmentStatus.Cancelled;
                appt.CancellationReason = reason;

                await _context.SaveChangesAsync();
            }
        }

        public async Task ConfirmAppointment(int id)
        {
            var appt = await _context.Appointments.FindAsync(id);

            if (appt != null)
            {
                appt.Status = AppointmentStatus.Confirmed;

                await _context.SaveChangesAsync();
            }
        }

        public async Task CompleteAppointment(int id)
        {
            var appt = await _context.Appointments.FindAsync(id);

            if (appt != null)
            {
                appt.Status = AppointmentStatus.Completed;

                await _context.SaveChangesAsync();
            }
        }
    }
}