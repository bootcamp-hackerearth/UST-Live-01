using HealthAxisHealth.API.Data;
using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.API.Repositories.Implementations
{
    [ExcludeFromCodeCoverage]
    public class AppointmentRepository :
        Repository<Appointment>,
        IAppointmentRepository
    {
        #region Constructor

        public AppointmentRepository(
            ApplicationDbContext context)
            : base(context)
        {
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(
            int patientId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Appointment>> GetDoctorAppointmentsByDateAsync(
            int doctorId,
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate.Date == date.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<Appointment?> GetAppointmentWithDetailsAsync(
            int appointmentId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(
                    a => a.AppointmentId == appointmentId,
                    cancellationToken);
        }

        public async Task<bool> IsSlotAvailableAsync(
            int doctorId,
            DateTime scheduledDate,
            string timeSlot,
            CancellationToken cancellationToken = default)
        {
            return !await _context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate.Date == scheduledDate.Date &&
                    a.TimeSlot == timeSlot,
                    cancellationToken);
        }

        public async Task<PagedResultDto<Appointment>> GetPagedAsync(
            PaginationParams pagination,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Appointment> query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor);

            if (!string.IsNullOrWhiteSpace(pagination.Search))
            {
                string search = pagination.Search.Trim();

                query = query.Where(a =>
                    a.Patient.FullName.Contains(search) ||
                    a.Doctor.FullName.Contains(search) ||
                    a.TimeSlot.Contains(search));
            }

            int totalRecords = await query.CountAsync(cancellationToken);

            int pageNumber = pagination.PageNumber ?? 1;
            int pageSize = pagination.PageSize ?? 10;

            List<Appointment> appointments = await query
                .OrderByDescending(a => a.ScheduledDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResultDto<Appointment>
            {
                Items = appointments,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        #endregion
    }
}