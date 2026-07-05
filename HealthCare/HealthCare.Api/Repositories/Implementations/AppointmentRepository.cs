using Microsoft.EntityFrameworkCore;
using HealthCare.Api.Data;
using Healthcare.Shared.DTOs.Appointments;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;

namespace HealthCare.Api.Repositories.Implementations
{
    public class AppointmentRepository : Repository<Appointment>,IAppointmentRepository
    {

        public AppointmentRepository(HealthCareDbContext context) : base(context) { }
        public async Task<List<string>> AvailableTimeSlots(DateOnly date, int doctorId) =>
            await _dbSet
                .Where(a => a.ScheduledDate == date
                         && a.DoctorId == doctorId
                         && a.Status != AppointmentStatus.Cancelled)
                .Select(a => a.TimeSlot)
                .ToListAsync();


        public async Task<bool> IsAvailable(DateOnly date, int doctorId, string timeSlot)
        {
            var exists = await _dbSet.AnyAsync(a =>
                a.ScheduledDate == date
                && a.DoctorId == doctorId
                && a.TimeSlot == timeSlot
                && a.Status != "Cancelled");

            return !exists;
        }

        public async Task<List<AppointmentReportDto>> GetDailyReport(DateOnly startDate, DateOnly endDate)
        {
            return await _dbSet
                .Where(a =>
                    a.ScheduledDate >= startDate &&
                    a.ScheduledDate <= endDate)

                .GroupBy(a => a.ScheduledDate)

                .Select(g => new AppointmentReportDto
                {
                    Date = g.Key,
                    PendingCount = g.Count(a => a.Status == AppointmentStatus.Pending),
                    ConfirmedCount = g.Count(a => a.Status == AppointmentStatus.Confirmed),
                    CancelledCount = g.Count(a => a.Status == AppointmentStatus.Cancelled),
                    CompletedCount = g.Count(a => a.Status == AppointmentStatus.Completed),
                    DailyRevenue = g.Where(a => a.Status == AppointmentStatus.Completed)
                .Sum(a => a.Doctor.ConsultationFee)

                })

                .OrderBy(r => r.Date)
                .ToListAsync();
        }

        public async Task<List<AppointmentListDto>> GetDoctorSchedule(DateOnly date, int id) =>
            await _dbSet
                .Where(a => a.ScheduledDate == date && a.DoctorId == id)
                .Select(a => new AppointmentListDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientName = a.Patient.FullName,
                    DoctorName = a.Doctor.FullName,
                    ScheduledDate = a.ScheduledDate,
                    TimeSlot = a.TimeSlot,
                    Status = a.Status
                })
                .ToListAsync();

        public async Task<List<AppointmentListDto>> GetPatientSchedule(DateOnly date, int id) =>
            await _dbSet
                .Where(a => a.ScheduledDate == date && a.PatientId == id)
                .Select(a => new AppointmentListDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientName = a.Patient.FullName,
                    DoctorName = a.Doctor.FullName,
                    ScheduledDate = a.ScheduledDate,
                    TimeSlot = a.TimeSlot,
                    Status = a.Status
                })
                .ToListAsync();

        public async Task<List<AppointmentListDto>> GetAppointmentByPatient(int id) =>
            await _dbSet
                .Where(a => a.PatientId == id && a.ScheduledDate >= DateOnly.FromDateTime(DateTime.Today))
                .Select(a => new AppointmentListDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientName = a.Patient.FullName,
                    DoctorName = a.Doctor.FullName,
                    ScheduledDate = a.ScheduledDate,
                    TimeSlot = a.TimeSlot,
                    Status = a.Status
                })
                .ToListAsync();

        public async Task<List<AppointmentListDto>> GetAppointmentByDoctor(int id) =>
            await _dbSet
                .Where(a => a.DoctorId == id && a.ScheduledDate >= DateOnly.FromDateTime(DateTime.Today))
                .Select(a => new AppointmentListDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientName = a.Patient.FullName,
                    DoctorName = a.Doctor.FullName,
                    ScheduledDate = a.ScheduledDate,
                    TimeSlot = a.TimeSlot,
                    Status = a.Status
                })
                .ToListAsync();

        public async Task CancelAppointmentsByDoctorDate(int doctorId, DateOnly date)
        {
            var appointments = await _dbSet
                .Where(a => a.DoctorId == doctorId
                         && a.ScheduledDate == date
                         && a.Status != AppointmentStatus.Cancelled)
                .ToListAsync();

            foreach (var appointment in appointments)
            {
                appointment.Status = AppointmentStatus.Cancelled;
                appointment.CancellationReason = "Doctor on leave";
            }

        }

    }
}
