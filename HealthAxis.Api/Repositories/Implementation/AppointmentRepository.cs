using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Repositories.Implementation
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<List<Appointment>> GetAppointmentsAsync(
            int? patientId,
            int? doctorId,
            DateTime? date,
            CancellationToken ct = default)
        {
            IQueryable<Appointment> query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor);

            if (patientId.HasValue)
            {
                query = query.Where(a => a.PatientId == patientId.Value);
            }

            if (doctorId.HasValue)
            {
                query = query.Where(a => a.DoctorId == doctorId.Value);
            }

            if (date.HasValue)
            {
                query = query.Where(a => a.ScheduledDate.Date == date.Value.Date);
            }

            return await query
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync(ct);
        }

        public async Task<Appointment?> GetDetailsAsync(
            int appointmentId,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId, ct);
        }

        public async Task<List<AppointmentReportDto>> GetAppointmentReportAsync(
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .GroupBy(a => a.ScheduledDate.Date)
                .Select(g => new AppointmentReportDto
                {
                    Date = g.Key,
                    Confirmed = g.Count(a => a.Status == "Confirmed"),
                    Cancelled = g.Count(a => a.Status == "Cancelled"),
                    Completed = g.Count(a => a.Status == "Completed")
                })
                .OrderBy(x => x.Date)
                .ToListAsync(ct);
        }

        public async Task<bool> DoctorHasAppointmentAtSlotAsync(
            int doctorId,
            DateTime scheduledDate,
            string timeSlot,
            CancellationToken ct = default)
        {
            return await _context.Appointments.AnyAsync(a =>
                a.DoctorId == doctorId &&
                a.ScheduledDate.Date == scheduledDate.Date &&
                a.TimeSlot == timeSlot &&
                a.Status != "Cancelled",
                ct);
        }
    }
}