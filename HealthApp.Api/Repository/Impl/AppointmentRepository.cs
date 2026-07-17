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



        public async Task<bool> IsSlotBookedAsync(int doctorId, DateTime date, string timeSlot)
        {
            return await _context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.TimeSlot == timeSlot);
        }

         
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

        public async Task<List<string>> GetBookedSlotsAsync(int doctorId, DateTime date)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.ScheduledDate.Date == date.Date &&
                            a.TimeSlot != null)
                .Select(a => a.TimeSlot!)
                .ToListAsync();
        }



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


        public async Task<List<Appointment>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
        }


        public async Task<List<Appointment>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
        }







        // Page

        public async Task<(List<Appointment> Items, int TotalCount)> GetPagedAppointmentsAsync(int pageNumber,int pageSize)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(a => a.ScheduledDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }


        public async Task<(List<Appointment> Items, int TotalCount)> GetByPatientAndDoctor(int? patientId, int? doctorId,
            int pageNumber,int pageSize)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .AsQueryable();

            if (patientId.HasValue)
                query = query.Where(a => a.PatientId == patientId.Value);

            if (doctorId.HasValue)
                query = query.Where(a => a.DoctorId == doctorId.Value);


            var totalCount = await query.CountAsync();


            var items = await query
                .OrderBy(a => a.ScheduledDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}