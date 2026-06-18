using HealthAxis.API.Data;
using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Repositories
{
    public class AppointmentRepository
        : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(HealthAxisDbContext context)
            : base(context)
        {
        }

        public async Task<List<Appointment>> GetByPatientIdAsync(
            int patientId,
            CancellationToken ct = default)
        {
            return await DbSet
                .AsNoTracking()
                .Where(appointment => appointment.PatientId == patientId)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            return await DbSet
                .AsNoTracking()
                .Where(appointment => appointment.DoctorId == doctorId)
                .ToListAsync(ct);
        }

        public async Task<bool> IsSlotBookedAsync(
            int doctorId,
            DateTime scheduledDate,
            string timeSlot,
            CancellationToken ct = default)
        {
            return await DbSet.AnyAsync(
                appointment =>
                    appointment.DoctorId == doctorId &&
                    appointment.ScheduledDate.Date == scheduledDate.Date &&
                    appointment.TimeSlot == timeSlot &&
                    appointment.Status != AppointmentStatus.Cancelled,
                ct);
        }
    }
}

