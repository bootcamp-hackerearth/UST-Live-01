using HealthCareApp.Data;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
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

        public new async Task<List<Appointment>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync(ct);
        }

        public new async Task<Appointment?> GetByIdAsync(int appointmentId, CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId, ct);
        }

        public async Task<List<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.ScheduledDate)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetByDoctorIdAsync(int doctorId, CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId)
                .OrderByDescending(a => a.ScheduledDate)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetByStatusAsync(AppointmentStatus status, CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Status == status)
                .OrderByDescending(a => a.ScheduledDate)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetUpcomingAppointmentsAsync(CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.ScheduledDate.Date >= DateTime.Today
                            && a.Status != AppointmentStatus.Cancelled
                            && a.Status != AppointmentStatus.Completed)
                .OrderBy(a => a.ScheduledDate)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetUpcomingAppointmentsByPatientIdAsync(int patientId, CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId
                            && a.ScheduledDate.Date >= DateTime.Today
                            && a.Status != AppointmentStatus.Cancelled
                            && a.Status != AppointmentStatus.Completed)
                .OrderBy(a => a.ScheduledDate)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetUpcomingAppointmentsByDoctorIdAsync(int doctorId, CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId
                            && a.ScheduledDate.Date >= DateTime.Today
                            && a.Status != AppointmentStatus.Cancelled
                            && a.Status != AppointmentStatus.Completed)
                .OrderBy(a => a.ScheduledDate)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetPendingAppointmentsByPatientIdAsync(int patientId, CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId
                            && a.Status == AppointmentStatus.Pending)
                .OrderBy(a => a.ScheduledDate)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetPendingAppointmentsByDoctorIdAsync(int doctorId, CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId
                            && a.Status == AppointmentStatus.Pending)
                .OrderBy(a => a.ScheduledDate)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetTodayConfirmedAppointmentsByDoctorIdAsync(int doctorId, CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId
                            && a.ScheduledDate.Date == DateTime.Today
                            && a.Status == AppointmentStatus.Confirmed)
                .OrderBy(a => a.TimeSlot)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetCancelledAppointmentsByPatientIdAsync(int patientId, CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId
                            && a.Status == AppointmentStatus.Cancelled)
                .OrderByDescending(a => a.ScheduledDate)
                .ToListAsync(ct);
        }

        public async Task<List<Appointment>> GetCancelledAppointmentsByDoctorIdAsync(int doctorId, CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId
                            && a.Status == AppointmentStatus.Cancelled)
                .OrderByDescending(a => a.ScheduledDate)
                .ToListAsync(ct);
        }

        public async Task<int> CountActiveAppointmentsByDoctorAndDateAsync(int doctorId, DateTime date, CancellationToken ct = default)
        {
            var selectedDate = date.Date;

            return await _context.Appointments
                .CountAsync(a => a.DoctorId == doctorId
                                 && a.ScheduledDate.Date == selectedDate
                                 && a.Status != AppointmentStatus.Cancelled
                                 && a.Status != AppointmentStatus.Completed,
                            ct);
        }

        public async Task<bool> IsSlotBookedAsync(int doctorId, DateTime date, string timeSlot, CancellationToken ct = default)
        {
            var selectedDate = date.Date;

            return await _context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId
                               && a.ScheduledDate.Date == selectedDate
                               && a.TimeSlot == timeSlot
                               && a.Status != AppointmentStatus.Cancelled
                               && a.Status != AppointmentStatus.Completed,
                          ct);
        }

        public async Task<bool> PatientHasActiveAppointmentWithDoctorOnDateAsync(
            int patientId,
            int doctorId,
            DateTime date,
            CancellationToken ct = default)
        {
            var selectedDate = date.Date;

            return await _context.Appointments
                .AnyAsync(a => a.PatientId == patientId
                               && a.DoctorId == doctorId
                               && a.ScheduledDate.Date == selectedDate
                               && a.Status != AppointmentStatus.Cancelled
                               && a.Status != AppointmentStatus.Completed,
                          ct);
        }

        public async Task<bool> PatientHasActiveAppointmentOnDateAndSlotAsync(
            int patientId,
            DateTime date,
            string timeSlot,
            CancellationToken ct = default)
        {
            var selectedDate = date.Date;

            return await _context.Appointments
                .AnyAsync(a => a.PatientId == patientId
                               && a.ScheduledDate.Date == selectedDate
                               && a.TimeSlot == timeSlot
                               && a.Status != AppointmentStatus.Cancelled
                               && a.Status != AppointmentStatus.Completed,
                          ct);
        }
    }
}