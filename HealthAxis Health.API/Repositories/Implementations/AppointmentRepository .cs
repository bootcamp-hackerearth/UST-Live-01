using HealthAxisHealth.API.Data;
using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisHealth.API.Repositories.Implementations
{
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

        public async Task<IEnumerable<Appointment>>
            GetByPatientIdAsync(
                int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a =>
                    a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>>
            GetByDoctorIdAsync(
                int doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Where(a =>
                    a.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>>
            GetDoctorAppointmentsByDateAsync(
                int doctorId,
                DateTime date)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Where(a =>
                    a.DoctorId == doctorId
                    &&
                    a.ScheduledDate.Date == date.Date)
                .ToListAsync();
        }

        public async Task<Appointment?>
            GetAppointmentWithDetailsAsync(
                int appointmentId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(
                    a => a.AppointmentId ==
                         appointmentId);
        }

        public async Task<bool>
            IsSlotAvailableAsync(
                int doctorId,
                DateTime scheduledDate,
                string timeSlot)
        {
            return !await _context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == doctorId
                    &&
                    a.ScheduledDate.Date ==
                        scheduledDate.Date
                    &&
                    a.TimeSlot == timeSlot);
        }

        public async Task<PagedResultDto<Appointment>>
    GetPagedAsync(
        PaginationParams pagination)
        {
            IQueryable<Appointment> query =
                _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor);

            if (!string.IsNullOrWhiteSpace(
                pagination.Search))
            {
                string search =
                    pagination.Search.Trim();

                query = query.Where(a =>
                    a.Patient.FullName.Contains(search) ||
                    a.Doctor.FullName.Contains(search) ||
                    a.TimeSlot.Contains(search));
            }

            int totalRecords =
                await query.CountAsync();

            List<Appointment> appointments =
                await query
                    .OrderByDescending(a => a.ScheduledDate)
                    .Skip(
                        (pagination.PageNumber - 1)
                        * pagination.PageSize)
                    .Take(
                        pagination.PageSize)
                    .ToListAsync();

            return new PagedResultDto<Appointment>
            {
                Items = appointments,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalRecords = totalRecords
            };
        }

        #endregion
    }
}
