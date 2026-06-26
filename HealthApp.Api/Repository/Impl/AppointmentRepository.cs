using HealthApp.Api.Data;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repository.Impl
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly HealthAppDbContext _context;

        public AppointmentRepository(HealthAppDbContext context) : base(context)
        {
            _context = context;
        }

        // ✅ GET ALL (IMPORTANT)
        public async Task<List<Appointment>?> getallAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
        }

        // ✅ GET BY ID
        public async Task<Appointment?> getbyidAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
        }

        // ✅ SLOT CHECK (NO INCLUDE NEEDED)
        public async Task<bool> IsSlotBookedAsync(int doctorId, DateTime date, string timeSlot)
        {
            return await _context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.TimeSlot == timeSlot);
        }

        // ✅ GET UPCOMING (FIXED ✅)
        public async Task<List<Appointment>?> GetUpcomingByDoctorAsync(int doctorId, DateTime from, DateTime to)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate.Date >= from &&
                    a.ScheduledDate.Date <= to)
                .ToListAsync();
        }

        // ✅ GET BOOKED SLOTS (NO INCLUDE NEEDED)
        public async Task<List<string>?> GetBookedSlotsAsync(int doctorId, DateTime date)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.ScheduledDate.Date == date.Date)
                .Select(a => a.TimeSlot)
                .ToListAsync();
        }

        // ✅ CANCEL (FIXED ✅)
        public async Task<Appointment?> CancelAppointmentAsync(int appointmentId, string reason)
        {
            var existing = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (existing == null) return null;

            existing.Status = "Cancelled";
            existing.CancellationReason = reason;

            await _context.SaveChangesAsync();
            return existing;
        }

        // ✅ UPDATE STATUS (FIXED ✅)
        public async Task<Appointment?> UpdateStatusAsync(int appointmentId, string status)
        {
            var existing = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (existing == null) return null;

            existing.Status = status;

            await _context.SaveChangesAsync();
            return existing;
        }

        // ✅ FILTER (FIXED ✅)
        public async Task<List<Appointment>?> GetByPatientAndDoctor(int? patientId, int? doctorId)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .AsQueryable();

            if (patientId.HasValue)
                query = query.Where(a => a.PatientId == patientId.Value);

            if (doctorId.HasValue)
                query = query.Where(a => a.DoctorId == doctorId.Value);

            return await query.ToListAsync();
        }
    }
}