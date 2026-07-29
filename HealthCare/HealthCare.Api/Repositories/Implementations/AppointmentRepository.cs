using HealthCare.Api.Data;
using HealthCare.Api.DTOs.Appointment;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Shared.DTOs.Appointment;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Api.Repositories.Implementations
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
private const string Pending = "Pending";

private const string Confirmed = "Confirmed";

private const string Cancelled = "Cancelled";

private const string Completed = "Completed";
        public AppointmentRepository(HealthCareDbContext context) : base(context) { }

        public IQueryable<Appointment> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<List<string>> BookedTimeSlots(DateOnly date, int doctorId) =>
            await _dbSet
                .Where(a => a.ScheduledDate == date
                         && a.DoctorId == doctorId
                         && a.Status != Cancelled)
                .Select(a => a.TimeSlot)
                .ToListAsync();

        public async Task<bool> IsAvailable(DateOnly date, int doctorId, string timeSlot)
        {
            var exists = await _dbSet.AnyAsync(a =>
                a.ScheduledDate == date
                && a.DoctorId == doctorId
                && a.TimeSlot == timeSlot
                && a.Status != Cancelled);

            return !exists;
        }

        public async Task<List<AppointmentReportDto>> GetReport(DateOnly fromDate, DateOnly toDate)
        {
            return await _dbSet
                .Where(a => a.ScheduledDate >= fromDate && a.ScheduledDate <= toDate)
                .GroupBy(a => a.ScheduledDate)
                .Select(g => new AppointmentReportDto
                {
                    Date = g.Key,
                    PendingCount = g.Count(a => a.Status == Pending),
                    ConfirmedCount = g.Count(a => a.Status == Confirmed),
                    CancelledCount = g.Count(a => a.Status == Cancelled),
                    CompletedCount = g.Count(a => a.Status == Completed),

                    Revenue = g
                        .Where(a => a.Status == Completed)
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
                    PatientId = a.PatientId,
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
                    PatientId = a.PatientId,
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
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullName,
                    DoctorName = a.Doctor.FullName,
                    ScheduledDate = a.ScheduledDate,
                    TimeSlot = a.TimeSlot,
                    Status = a.Status
                })
                .ToListAsync();

        public async Task<List<AppointmentListDto>> GetAppointmentByDoctor(int id) =>
            await _dbSet
                .Where(a => a.DoctorId == id && a.ScheduledDate >= DateOnly.FromDateTime(DateTime.Today) && (a.Status == "Pending" || a.Status == "Confirmed"))
                .Select(a => new AppointmentListDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
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
                         && a.Status != Cancelled)
                .ToListAsync();

            foreach (var appointment in appointments)
            {
                appointment.Status = Cancelled;
                appointment.CancellationReason = "Doctor on leave";
            }
        }

        public async Task<AppointmentSummaryDto> GetSummaryAsync()
        {
            var fromDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-30));
            var toDate = DateOnly.FromDateTime(DateTime.Today);

            var result = await _dbSet
                .Where(a => a.ScheduledDate >= fromDate && a.ScheduledDate <= toDate)
                .GroupBy(a => 1)
                .Select(g => new AppointmentSummaryDto
                {
                    PendingCount = g.Count(a => a.Status == Pending),
                    ConfirmedCount = g.Count(a => a.Status == Confirmed),
                    CancelledCount = g.Count(a => a.Status == Cancelled),
                    CompletedCount = g.Count(a => a.Status == Completed),

                    TotalRevenue = g
                        .Where(a => a.Status == Completed)
                        .Sum(a => a.Doctor.ConsultationFee)
                })
                .FirstOrDefaultAsync();

            return result ?? new AppointmentSummaryDto();
        }

        public async Task<AppointmentSummaryDto> GetDashboardSummaryAsync()
        {
            var result = await _dbSet
                .GroupBy(a => 1)
                .Select(g => new AppointmentSummaryDto
                {
                    PendingCount = g.Count(a => a.Status == Pending),

                    ConfirmedCount = g.Count(a => a.Status == Confirmed),

                    CancelledCount = g.Count(a => a.Status == Cancelled),

                    CompletedCount = g.Count(a => a.Status == Completed),

                    TotalRevenue = g
                        .Where(a => a.Status == Completed)
                        .Sum(a => a.Doctor.ConsultationFee)
                })
                .FirstOrDefaultAsync();

            return result ?? new AppointmentSummaryDto();
        } 
    }
}