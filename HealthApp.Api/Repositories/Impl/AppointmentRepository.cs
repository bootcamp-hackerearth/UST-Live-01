using HealthApp.Api.Data;
using HealthApp.Api.Enums;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repositories.Impl
{
    public class AppointmentRepository(HealthAppDbContext context) : Repository<Appointment>(context), IAppointmentRepository
    {
        public async Task<IEnumerable<Appointment>> GetAppointmentsAsync(
            int? doctorId = null,
            int? patientId = null,
            bool onlyUpcoming = false,
            CancellationToken ct = default)
        {
            var query = context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .AsQueryable();

            if (doctorId.HasValue)
            {
                query = query.Where(a => a.DoctorId == doctorId.Value);
            }

            if (patientId.HasValue)
            {
                query = query.Where(a => a.PatientId == patientId.Value);
            }

            if (onlyUpcoming)
            {
                query = query.Where(a => a.ScheduledDate >= DateOnly.FromDateTime(DateTime.Today));
            }

            return await query
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync(ct);
        }


        public async Task<bool> HasAppointmentWithDoctorOnSameDayAsync(
            int patientId,
            int doctorId,
            DateOnly date,
            CancellationToken ct = default)
        {
            return await context.Appointments
                .AnyAsync(a =>
                    a.PatientId == patientId &&
                    a.DoctorId == doctorId &&
                    a.ScheduledDate == date &&
                    a.Status != AppointmentStatus.Cancelled,
                    ct);
        }


        public async Task<bool> HasPatientSlotConflictAsync(
            int patientId,
            DateOnly date,
            string slot,
            CancellationToken ct = default)
        {
            return await context.Appointments
                .AnyAsync(a =>
                    a.PatientId == patientId &&
                    a.ScheduledDate == date &&
                    a.TimeSlot == slot &&
                    a.Status != AppointmentStatus.Cancelled,
                    ct);
        }


        public async Task<bool> IsDoctorSlotBookedAsync(
            int doctorId,
            DateOnly date,
            string slot,
            CancellationToken ct = default)
        {
            return await context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate == date &&
                    a.TimeSlot == slot &&
                    a.Status != AppointmentStatus.Cancelled,
                    ct);
        }
    }
}
