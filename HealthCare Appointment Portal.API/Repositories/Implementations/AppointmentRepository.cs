using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Repositories
{
    public class AppointmentRepository
        : Repository<Appointment>,
          IAppointmentRepository
    {
        public AppointmentRepository(
            ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Appointment>>
            GetAppointmentsByPatientAsync(
                int patientId)
        {
            return await _dbSet
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.PatientId == patientId)
                .OrderBy(a =>
                    a.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>>
            GetAppointmentsByDoctorAsync(
                int doctorId)
        {
            return await _dbSet
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.DoctorId == doctorId)
                .OrderBy(a =>
                    a.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>>
            GetUpcomingAppointmentsAsync()
        {
            return await _dbSet
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.ScheduledDate >= DateTime.Today
                    &&
                    a.Status ==
                    AppointmentStatus.Confirmed)
                .OrderBy(a =>
                    a.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>>
            GetCompletedAppointmentsAsync()
        {
            return await _dbSet
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.Status ==
                    AppointmentStatus.Completed)
                .ToListAsync();
        }

        public async Task<Appointment>
            GetConflictingAppointmentAsync(
                int doctorId,
                DateTime date,
                string timeSlot)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a =>
                    a.DoctorId == doctorId
                    &&
                    DbFunctions.TruncateTime(
                        a.ScheduledDate)
                    ==
                    DbFunctions.TruncateTime(
                        date)
                    &&
                    a.TimeSlot == timeSlot
                    &&
                    a.Status !=
                    AppointmentStatus.Cancelled);
        }

        public async Task<bool>
            IsSlotAvailableAsync(
                int doctorId,
                DateTime date,
                string timeSlot)
        {
            return !await _dbSet
                .AnyAsync(a =>
                    a.DoctorId == doctorId
                    &&
                    DbFunctions.TruncateTime(
                        a.ScheduledDate)
                    ==
                    DbFunctions.TruncateTime(
                        date)
                    &&
                    a.TimeSlot == timeSlot
                    &&
                    a.Status !=
                    AppointmentStatus.Cancelled);
        }

        public async Task<IEnumerable<Appointment>>
            GetTodayScheduleAsync(
                int doctorId)
        {
            return await _dbSet
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.DoctorId == doctorId
                    &&
                    DbFunctions.TruncateTime(
                        a.ScheduledDate)
                    ==
                    DbFunctions.TruncateTime(
                        DateTime.Today))
                .OrderBy(a =>
                    a.TimeSlot)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>>
            GetWeeklyScheduleAsync(
                int doctorId)
        {
            DateTime today =
                DateTime.Today;

            DateTime weekEnd =
                today.AddDays(7);

            return await _dbSet
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.DoctorId == doctorId
                    &&
                    a.ScheduledDate >= today
                    &&
                    a.ScheduledDate <= weekEnd)
                .OrderBy(a =>
                    a.ScheduledDate)
                .ThenBy(a =>
                    a.TimeSlot)
                .ToListAsync();
        }

        public async Task<Appointment>
            GetNextAppointmentByPatientAsync(
                int patientId)
        {
            return await _dbSet
                .Include(a => a.Doctor)
                .Where(a =>
                    a.PatientId == patientId
                    &&
                    a.Status !=
                    AppointmentStatus.Cancelled
                    &&
                    a.ScheduledDate >=
                    DateTime.Today)
                .OrderBy(a =>
                    a.ScheduledDate)
                .FirstOrDefaultAsync();
        }
    }
}