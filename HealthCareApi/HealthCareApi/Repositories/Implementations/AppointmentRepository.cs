using HealthCare.Shared;
using HealthCareApi;
//using HealthCareApi.Helper;
using HealthCareApi.Repositories.Implementations;
using HealthCareApi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(HealthAppDbContext context) : base(context)
    {
    }

    // Check if slot exists for doctor
    public async Task<bool> SlotExistsAsync(int doctorId, string timeSlot)
    {
        return await _context.DoctorAvailableSlots.AnyAsync(s =>
            s.DoctorId == doctorId &&
            s.TimeSlot == timeSlot);
    }

    // Check if slot already booked
    public async Task<bool> IsSlotBookedAsync(int doctorId, DateTime date, string timeSlot)
    {
        return await _context.Appointments.AnyAsync(a =>
            a.DoctorId == doctorId &&
            DbFunctions.TruncateTime(a.ScheduledDate) == date.Date &&
            a.TimeSlot == timeSlot &&
            a.Status != "Cancelled"); // Cancelled slots can be reused
    }

    public async Task<PagedResult<Appointment>> GetPatientAppointmentsAsync(
        int patientId,
        string status,
        int pageNumber,
        int pageSize)
    {
        // ✅ Safety
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize > 50 ? 50 : pageSize;

        // ✅ Base Query
        IQueryable<Appointment> query = _context.Appointments
            .Where(a => a.PatientId == patientId);

        // ✅ Filter by Status
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(a => a.Status == status);
        }

        // ✅ ✅ IMPORTANT: Get total count BEFORE pagination
        int totalCount = await query.CountAsync();

        // ✅ Sorting (latest first)
        query = query
            .OrderByDescending(a => a.ScheduledDate)
            .ThenByDescending(a => a.TimeSlot);

        // ✅ Pagination
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // ✅ Return paged result
        return new PagedResult<Appointment>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<IEnumerable<Appointment>> GetTodayAppointmentsAsync(int doctorId)
    {
        var today = DateTime.Today;

        return await _context.Appointments
            .Where(a => a.DoctorId == doctorId &&
                        DbFunctions.TruncateTime(a.ScheduledDate) == today)
            .OrderBy(a => a.TimeSlot)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetWeeklyAppointmentsAsync(int doctorId)
    {
        var startOfWeek = DateTime.Today;
        var endOfWeek = startOfWeek.AddDays(7);

        return await _context.Appointments
            .Where(a => a.DoctorId == doctorId &&
                        a.ScheduledDate >= startOfWeek &&
                        a.ScheduledDate < endOfWeek)
            .OrderBy(a => a.ScheduledDate)
            .ThenBy(a => a.TimeSlot)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date)
    {
        return await _context.Appointments
            .Where(a => DbFunctions.TruncateTime(a.ScheduledDate) == date.Date)
            .OrderBy(a => a.TimeSlot)
            .ToListAsync();
    }
}