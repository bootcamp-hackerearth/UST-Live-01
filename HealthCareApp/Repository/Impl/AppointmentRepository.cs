using HealthCareApp.Data;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Repository.Impl
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        private readonly HealthAxisDbContext _context;

        public AppointmentRepository(HealthAxisDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAppointmentsByDateAsync(
            DateTime scheduledDate,
            CancellationToken ct = default)
        {
            var selectedDate = scheduledDate.Date;
            var nextDate = selectedDate.AddDays(1);

            return await _context.Appointments
                .AsNoTracking()
                .Where(a =>
                    a.ScheduledDate >= selectedDate &&
                    a.ScheduledDate < nextDate)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetAppointmentsForFilterOptionsAsync(
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Patient != null && a.Doctor != null)
                .ToListAsync(ct);
        }

        public async Task<(List<Appointment> Items, int TotalRecords)> GetPagedAppointmentsAsync(
            AppointmentPaginationQueryDto query,
            CancellationToken ct = default)
        {
            query ??= new AppointmentPaginationQueryDto();

            int pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;

            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            pageSize = pageSize > 100 ? 100 : pageSize;

            var appointmentsQuery = _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .AsQueryable();

            appointmentsQuery = ApplyAppointmentFilters(
                appointmentsQuery,
                query);

            int totalRecords = await appointmentsQuery.CountAsync(ct);

            var appointments = await appointmentsQuery
                .OrderByDescending(a => a.ScheduledDate)
                .ThenByDescending(a => a.AppointmentId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (appointments, totalRecords);
        }

        public new async Task<List<Appointment>> GetAllAsync(
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenByDescending(a => a.AppointmentId)
                .ToListAsync(ct);
        }

        public new async Task<Appointment?> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == id, ct);
        }

        public async Task<List<Appointment>> GetByPatientIdAsync(
            int patientId,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenByDescending(a => a.AppointmentId)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenByDescending(a => a.AppointmentId)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetByStatusAsync(
            AppointmentStatus status,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Status == status)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenByDescending(a => a.AppointmentId)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetUpcomingAppointmentsAsync(
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.ScheduledDate.Date >= DateTime.Today &&
                            a.Status != AppointmentStatus.Cancelled &&
                            a.Status != AppointmentStatus.Completed)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.AppointmentId)
                .ToListAsync(ct);
        }

        public async Task<List<string>> GetBookedTimeSlotsByDoctorAndDateAsync(
            int doctorId,
            DateTime date,
            CancellationToken ct = default)
        {
            var selectedDate = date.Date;
            var nextDate = selectedDate.AddDays(1);

            return await _context.Appointments
                .AsNoTracking()
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate >= selectedDate &&
                    a.ScheduledDate < nextDate &&
                    a.Status != AppointmentStatus.Cancelled)
                .Select(a => a.TimeSlot)
                .Distinct()
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetUpcomingAppointmentsByPatientIdAsync(
            int patientId,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId &&
                            a.ScheduledDate.Date >= DateTime.Today &&
                            a.Status != AppointmentStatus.Cancelled &&
                            a.Status != AppointmentStatus.Completed)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.AppointmentId)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetUpcomingAppointmentsByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId &&
                            a.ScheduledDate.Date >= DateTime.Today &&
                            a.Status != AppointmentStatus.Cancelled &&
                            a.Status != AppointmentStatus.Completed)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.AppointmentId)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetPendingAppointmentsByPatientIdAsync(
            int patientId,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId &&
                            a.Status == AppointmentStatus.Pending)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.AppointmentId)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetPendingAppointmentsByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId &&
                            a.Status == AppointmentStatus.Pending)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.AppointmentId)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetTodayConfirmedAppointmentsByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId &&
                            a.ScheduledDate >= today &&
                            a.ScheduledDate < tomorrow &&
                            a.Status == AppointmentStatus.Confirmed)
                .OrderBy(a => a.TimeSlot)
                .ThenBy(a => a.AppointmentId)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetCancelledAppointmentsByPatientIdAsync(
            int patientId,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId &&
                            a.Status == AppointmentStatus.Cancelled)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenByDescending(a => a.AppointmentId)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetCancelledAppointmentsByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId &&
                            a.Status == AppointmentStatus.Cancelled)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenByDescending(a => a.AppointmentId)
                .ToListAsync(ct);
        }

        public async Task<int> CountActiveAppointmentsByDoctorAndDateAsync(
            int doctorId,
            DateTime date,
            CancellationToken ct = default)
        {
            var selectedDate = date.Date;
            var nextDate = selectedDate.AddDays(1);

            return await _context.Appointments
                .CountAsync(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate >= selectedDate &&
                    a.ScheduledDate < nextDate &&
                    a.Status != AppointmentStatus.Cancelled &&
                    a.Status != AppointmentStatus.Completed,
                    ct);
        }

        public async Task<bool> IsSlotBookedAsync(
            int doctorId,
            DateTime date,
            string timeSlot,
            CancellationToken ct = default)
        {
            var selectedDate = date.Date;
            var nextDate = selectedDate.AddDays(1);

            return await _context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate >= selectedDate &&
                    a.ScheduledDate < nextDate &&
                    a.TimeSlot == timeSlot &&
                    a.Status != AppointmentStatus.Cancelled &&
                    a.Status != AppointmentStatus.Completed,
                    ct);
        }

        public async Task<bool> PatientHasActiveAppointmentWithDoctorOnDateAsync(
            int patientId,
            int doctorId,
            DateTime date,
            CancellationToken ct = default)
        {
            var selectedDate = date.Date;
            var nextDate = selectedDate.AddDays(1);

            return await _context.Appointments
                .AnyAsync(a =>
                    a.PatientId == patientId &&
                    a.DoctorId == doctorId &&
                    a.ScheduledDate >= selectedDate &&
                    a.ScheduledDate < nextDate &&
                    a.Status != AppointmentStatus.Cancelled &&
                    a.Status != AppointmentStatus.Completed,
                    ct);
        }

        public async Task<bool> PatientHasActiveAppointmentOnDateAndSlotAsync(
            int patientId,
            DateTime date,
            string timeSlot,
            CancellationToken ct = default)
        {
            var selectedDate = date.Date;
            var nextDate = selectedDate.AddDays(1);

            return await _context.Appointments
                .AnyAsync(a =>
                    a.PatientId == patientId &&
                    a.ScheduledDate >= selectedDate &&
                    a.ScheduledDate < nextDate &&
                    a.TimeSlot == timeSlot &&
                    a.Status != AppointmentStatus.Cancelled &&
                    a.Status != AppointmentStatus.Completed,
                    ct);
        }

        private static IQueryable<Appointment> ApplyAppointmentFilters(
            IQueryable<Appointment> appointmentsQuery,
            AppointmentPaginationQueryDto query)
        {
            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                string searchTerm = query.SearchTerm.Trim();

                appointmentsQuery = appointmentsQuery.Where(a =>
                    (a.Patient != null &&
                     a.Patient.PatientName.Contains(searchTerm)) ||
                    (a.Doctor != null &&
                     a.Doctor.DoctorName.Contains(searchTerm)) ||
                    a.TimeSlot.Contains(searchTerm) ||
                    (a.CancellationReason != null &&
                     a.CancellationReason.Contains(searchTerm)));
            }

            if (query.PatientId is not null)
            {
                appointmentsQuery = appointmentsQuery.Where(a =>
                    a.PatientId == query.PatientId.Value);
            }

            if (query.DoctorId is not null)
            {
                appointmentsQuery = appointmentsQuery.Where(a =>
                    a.DoctorId == query.DoctorId.Value);
            }

            if (query.Status is not null)
            {
                appointmentsQuery = appointmentsQuery.Where(a =>
                    a.Status == query.Status.Value);
            }

            if (query.ScheduledDate is not null)
            {
                var selectedDate = query.ScheduledDate.Value.Date;
                var nextDate = selectedDate.AddDays(1);

                appointmentsQuery = appointmentsQuery.Where(a =>
                    a.ScheduledDate >= selectedDate &&
                    a.ScheduledDate < nextDate);
            }

            if (query.UpcomingOnly is not null && query.UpcomingOnly.Value)
            {
                appointmentsQuery = appointmentsQuery.Where(a =>
                    a.ScheduledDate.Date >= DateTime.Today &&
                    a.Status != AppointmentStatus.Cancelled &&
                    a.Status != AppointmentStatus.Completed);
            }

            return appointmentsQuery;
        }
    }
}