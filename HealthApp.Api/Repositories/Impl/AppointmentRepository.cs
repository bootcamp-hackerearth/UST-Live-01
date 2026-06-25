using HealthApp.Api.Data;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repositories.Impl
{
    public class AppointmentRepository(HealthAppDbContext context)
        : Repository<Appointment>(context), IAppointmentRepository
    {
        public async Task<(IEnumerable<Appointment> Items, int TotalCount)> GetAppointmentsAsync(
            AppointmentFilterDto filter,
            CancellationToken ct = default)
        {
            filter ??= new AppointmentFilterDto();

            var query = context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .AsQueryable();

            if (filter.DoctorId.HasValue)
            {
                query = query.Where(a => a.DoctorId == filter.DoctorId.Value);
            }

            if (filter.PatientId.HasValue)
            {
                query = query.Where(a => a.PatientId == filter.PatientId.Value);
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(a => a.Status == filter.Status.Value);
            }

            if (filter.Date.HasValue)
            {
                query = query.Where(a => a.ScheduledDate == filter.Date.Value);
            }
            else
            {
                if (filter.FromDate.HasValue)
                {
                    query = query.Where(a => a.ScheduledDate >= filter.FromDate.Value);
                }

                if (filter.ToDate.HasValue)
                {
                    query = query.Where(a => a.ScheduledDate <= filter.ToDate.Value);
                }
            }

            if (filter.OnlyUpcoming)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);

                query = query.Where(a => a.ScheduledDate >= today);
            }

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(ct);

            return (items, totalCount);
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