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
                         && a.Status != "Cancelled")
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

        public async Task<List<AppointmentReportDto>> GetDailyReport() =>
            await _dbSet
                .GroupBy(a => a.ScheduledDate)
                .Select(g => new AppointmentReportDto
                {
                    Date = g.Key,
                    PendingCount = g.Count(a => a.Status == "Pending"),
                    ConfirmedCount = g.Count(a => a.Status == "Confirmed"),
                    CancelledCount = g.Count(a => a.Status == "Cancelled"),
                    CompletedCount = g.Count(a => a.Status == "Completed")
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

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
                         && a.Status != "Cancelled")
                .ToListAsync();

            foreach (var appointment in appointments)
            {
                appointment.Status = "Cancelled";
                appointment.CancellationReason = "Doctor on leave";
            }
        }

     }
}
