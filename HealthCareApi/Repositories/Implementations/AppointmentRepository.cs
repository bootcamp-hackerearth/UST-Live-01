//using HealthCareApi.Helper;
using HealthCare.Shared;
using HealthCareApi;
using HealthCareApi.Repositories.Implementations;
using HealthCareApi.Repositories.Interfaces;
using HealthCareWebApi;
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
        //  Safety
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize > 50 ? 50 : pageSize;

        //  Base Query
        IQueryable<Appointment> query = _context.Appointments
               .Include(a => a.Patient)
            .Include(a => a.Doctor)

            .Where(a => a.PatientId == patientId);

        //  Filter by Status
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(a => a.Status == status);
        }

        //   IMPORTANT: Get total count BEFORE pagination
        int totalCount = await query.CountAsync();

        //  Sorting (latest first)
        query = query
            .OrderBy(a => a.ScheduledDate)
            .ThenByDescending(a => a.TimeSlot);

        //  Pagination
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        //  Return paged result
        return new PagedResult<Appointment>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<PagedResult<Appointment>> GetDoctorAppointmentsAsync(
    int doctorId,
    string status,
    int pageNumber,
    int pageSize)
    {
        //  Safety
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize > 50 ? 50 : pageSize;

        //  Base Query
        IQueryable<Appointment> query = _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => a.DoctorId == doctorId);

        //  Filter by Status
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(a => a.Status == status);
        }

        //  Total count BEFORE pagination
        int totalCount = await query.CountAsync();

        //  Sorting (by date, then time)
        query = query
            .OrderBy(a => a.ScheduledDate)
            .ThenByDescending(a => a.TimeSlot);

        //  Pagination
        var items = await query
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

    public async Task<List<string>> GetBookedSlotsAsync(int doctorId, DateTime date)
    {
        return await _context.Appointments
            .Where(a => a.DoctorId == doctorId &&
                        DbFunctions.TruncateTime(a.ScheduledDate) == date.Date &&
                        a.Status != "Cancelled")
            .Select(a => a.TimeSlot)
            .ToListAsync();
    }

    public async Task<PagedResult<Appointment>> GetUpcomingAppointmentsAsync(
    int? patientId,
    int? doctorId,
    int pageNumber,
    int pageSize)
    {
        var today = DateTime.Today;

        var query = _context.Appointments.AsQueryable();

        //  From today onwards
        query = query.Where(a => a.ScheduledDate >= today);

        //  CASE 1: No filters → exclude both Cancelled + Completed
        if (!patientId.HasValue && !doctorId.HasValue)
        {
            query = query;
        }
        else
        {
            //  CASE 2: Filters applied → exclude only Cancelled
            query = query.Where(a => a.Status != "Cancelled");

            //  Optional filters
            if (patientId.HasValue)
            {
                query = query.Where(a => a.PatientId == patientId.Value);
            }

            if (doctorId.HasValue)
            {
                query = query.Where(a => a.DoctorId == doctorId.Value);
            }
        }

        //  Order by date + time
        query = query
            .OrderBy(a => a.ScheduledDate)
            .ThenBy(a => a.TimeSlot);

        var totalCount = await query.CountAsync();

        var items = await query
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

    public async Task<bool> PatientExistsAsync(int patientId)
    {
        return await _context.Patients.AnyAsync(d => d.PatientId == patientId && d.IsActive);
    }

    public async Task<bool> DoctorExistsAsync(int doctorId)
    {
        return await _context.Doctors.AnyAsync(d => d.DoctorId == doctorId && d.IsActive);
    }

}