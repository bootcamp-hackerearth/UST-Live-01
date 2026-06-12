using HealthCare.Shared;
using HealthCareApi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCareApi.Repositories.Implementations
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(HealthAppDbContext context) : base(context)
        {
        }

        // Check if slot exists for doctor
        public async Task<bool> SlotExistsAsync(int doctorId, string timeSlot)
        {
            return await _context.DoctorAvailableSlots
                .AnyAsync(s => s.DoctorId == doctorId && s.TimeSlot == timeSlot);
        }

        // Get all slots of a doctor
        public async Task<List<string>> GetDoctorSlotsAsync(int doctorId)
        {
            return await _context.DoctorAvailableSlots
                .Where(s => s.DoctorId == doctorId)
                .Select(s => s.TimeSlot)
                .ToListAsync();
        }

        // Check if slot already booked
        public async Task<bool> IsSlotBookedAsync(int doctorId, DateTime date, string timeSlot)
        {
            return await _context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == doctorId &&
                    DbFunctions.TruncateTime(a.ScheduledDate) == date.Date &&
                    a.TimeSlot == timeSlot &&
                    a.Status != "Cancelled");
        }

        // Patient Appointments with Pagination
        public async Task<PagedResult<Appointment>> GetPatientAppointmentsAsync(
            int patientId,
            string status,
            int pageNumber,
            int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize > 50 ? 50 : pageSize;

            IQueryable<Appointment> query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId);

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(a => a.Status == status);
            }

            int totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(a => a.ScheduledDate)
                .ThenByDescending(a => a.TimeSlot)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Appointment>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        // Doctor Appointments with Pagination
        public async Task<PagedResult<Appointment>> GetDoctorAppointmentsAsync(
            int doctorId,
            string status,
            int pageNumber,
            int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize > 50 ? 50 : pageSize;

            IQueryable<Appointment> query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId);

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(a => a.Status == status);
            }

            int totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(a => a.ScheduledDate)
                .ThenByDescending(a => a.TimeSlot)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Appointment>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        // By Date
        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            return await _context.Appointments
                .Where(a => DbFunctions.TruncateTime(a.ScheduledDate) == date.Date)
                .OrderBy(a => a.TimeSlot)
                .ToListAsync();
        }

        // Booked Slots
        public async Task<List<string>> GetBookedSlotsAsync(int doctorId, DateTime date)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            DbFunctions.TruncateTime(a.ScheduledDate) == date.Date &&
                            a.Status != "Cancelled")
                .Select(a => a.TimeSlot)
                .ToListAsync();
        }

        // Upcoming Appointments
        public async Task<PagedResult<Appointment>> GetUpcomingAppointmentsAsync(
            int? patientId,
            int? doctorId,
            int pageNumber,
            int pageSize)
        {
            var today = DateTime.Today;

            var query = _context.Appointments
                .Where(a => a.ScheduledDate >= today);

            if (patientId.HasValue || doctorId.HasValue)
            {
                query = query.Where(a => a.Status != "Cancelled");

                if (patientId.HasValue)
                    query = query.Where(a => a.PatientId == patientId.Value);

                if (doctorId.HasValue)
                    query = query.Where(a => a.DoctorId == doctorId.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Appointment>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        // FIXED METHODS 
        public async Task<bool> PatientExistsAsync(int patientId)
        {
            return await _context.Patients
                .AnyAsync(p => p.PatientId == patientId && p.IsActive);
        }

        public async Task<bool> DoctorExistsAsync(int doctorId)
        {
            return await _context.Doctors
                .AnyAsync(d => d.DoctorId == doctorId && d.IsActive);
        }
    }
}
