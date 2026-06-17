using HealthApp.Api.Data;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HospitalManagementAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repository.Impl
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly HealthAppDbContext _context;

        public AppointmentRepository(HealthAppDbContext context) : base(context)
        {
            _context=context; 
        }

        public async Task<Appointment?> GetDoctorByIdAsync(int doctorId)
        {
            var exiting = await _context.Set<Appointment>()
                                        .FirstOrDefaultAsync(a => a.DoctorId == doctorId);
            if (exiting == null) return null;
            return exiting;
        }

        public async Task<bool> IsSlotBookedAsync(int doctorId, DateTime date, string timeSlot)
        {
            var existing = await _context.Set<Appointment>()
                .FirstOrDefaultAsync(a => a.DoctorId == doctorId && 
                a.ScheduledDate.Date == date.Date && a.TimeSlot == timeSlot);
            if (existing == null) return false;
            return true;
        }

        public async Task<List<Appointment>?> GetByPatientAsync(int patientId)
        {
            var existing = await _context.Set<Appointment>()
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
            if (existing == null) return null;
            return existing;
        }

        public async Task<List<Appointment>?> GetUpcomingByDoctorAsync(int doctorId, DateTime from, DateTime to)
        {
            var existing = await _context.Set<Appointment>()
                .Where(a => a.DoctorId == doctorId && a.ScheduledDate.Date >= from && 
                    a.ScheduledDate.Date<=to).ToListAsync();
            if (existing == null) return null;
            return existing;

        }

        public async Task<List<string>?> GetBookedSlotsAsync(int doctorId, DateTime date)
        {
            var existing = await _context.Set<Appointment>()
                .Where(a => a.DoctorId == doctorId && a.ScheduledDate.Date == date.Date)
                .Select(a => a.TimeSlot)
                .ToListAsync();
            if (existing == null) return null;
            return existing;
        }

        public async Task<Appointment?> CancelAppointmentAsync(int appointmentId, string reason)
        {
            var existing = await _context.Set<Appointment>()
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
            if (existing == null) return null;
            existing.Status = "Cancelled";
            existing.CancellationReason = reason;
            await _context.SaveChangesAsync();
            return existing ;
        }

        public async Task<Appointment?> UpdateStatusAsync(int appointmentId, string status)
        {
            var existing = await _context.Set<Appointment>()
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
            if (existing == null) return null;
            existing.Status = status;
            await _context.SaveChangesAsync();
            return existing;

        }
    }
}
