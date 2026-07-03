using HealthAxisApplicn.Data;
using HealthAxisApplicn.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisApplicn.Repositories.Impl
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        private readonly AppDbContext _context;
        public AppointmentRepository(AppDbContext context): base(context)
        {
            _context = context;
        }


        public async Task<List<Appointment>> GetUpcomingAppointmentsByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            return await _context.Set<Appointment>()
                .AsNoTracking()
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate.Date >= DateTime.UtcNow.Date &&
                    (a.Status == "Pending" || a.Status == "Confirmed")
                )
                .OrderBy(a => a.ScheduledDate)
                .ToListAsync(ct);
        }



        public async Task<List<Appointment>> GetAppointmentsByPatientIdAsync(
            int patientId,
            CancellationToken ct = default)
        {
            return await _context.Set<Appointment>()
                .AsNoTracking()
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.ScheduledDate)
                .ToListAsync(ct);
        }


        public async Task<List<Appointment>> GetAppointmentsByDoctorNameAsync(string doctorName, CancellationToken ct = default)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Doctor)
                .Where(a => a.Doctor.DoctorName == doctorName)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientNameAsync(string patientName, CancellationToken ct = default)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Patient)
                .Where(a => a.Patient.PatientName == patientName)
                .ToListAsync(ct);
        }

        public async Task<bool> DoctorHasConflictAsync(int doctorId, DateTime date, string timeSlot, CancellationToken ct = default)
        {
            return await _context.Set<Appointment>()
                .AnyAsync(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.TimeSlot == timeSlot &&
                    a.Status!="Cancelled", ct);
        }

        public async Task<bool> PatientHasConflictAsync(int patientId, DateTime date, string timeSlot, CancellationToken ct = default)
        {
            return await _context.Set<Appointment>()
                .AnyAsync(a =>
                    a.PatientId == patientId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.TimeSlot == timeSlot, ct);
        }

        public async Task<bool> PatientHasAppointmentOnDateAsync(int patientId, DateTime date, CancellationToken ct = default)
        {
            return await _context.Set<Appointment>()
                .AnyAsync(a =>
                    a.PatientId == patientId &&
                    a.ScheduledDate.Date == date.Date, ct);
        }

        public async Task<List<Appointment>> GetTodayAppointmentsAsync(int doctorId, CancellationToken ct = default)
        {
            return await _context.Appointments
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate.Date == DateTime.Today)
                .OrderBy(a => a.TimeSlot)
                .ToListAsync(ct);
        }

    }
}
