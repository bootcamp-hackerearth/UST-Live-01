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
        : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>>
            GetAllAsync()
        {
            return await _context.Appointments
                .ToListAsync();
        }

        public async Task<Appointment>
            GetByIdAsync(
                int appointmentId)
        {
            return await _context.Appointments
                .FirstOrDefaultAsync(a =>
                    a.AppointmentId ==
                    appointmentId);
        }

        public async Task
            AddAsync(
                Appointment appointment)
        {
            _context.Appointments
                .Add(appointment);

            await _context
                .SaveChangesAsync();
        }

        public async Task
            UpdateAsync(
                Appointment appointment)
        {
            _context.Entry(appointment)
                .State =
                EntityState.Modified;

            await _context
                .SaveChangesAsync();
        }

        public async Task
            DeleteAsync(
                int appointmentId)
        {
            var appointment =
                await GetByIdAsync(
                    appointmentId);

            if (appointment != null)
            {
                _context.Appointments
                    .Remove(appointment);

                await _context
                    .SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Appointment>>
            GetAppointmentsByPatientAsync(
                int patientId)
        {
            return await _context.Appointments
                .Where(a =>
                    a.PatientId ==
                    patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>>
            GetAppointmentsByDoctorAsync(
                int doctorId)
        {
            return await _context.Appointments
                .Where(a =>
                    a.DoctorId ==
                    doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>>
            GetTodayScheduleAsync(
                int doctorId)
        {
            var today = DateTime.Today;

            return await _context.Appointments
                .Where(a =>
                    a.DoctorId == doctorId &&
                    DbFunctions.TruncateTime(
                        a.ScheduledDate) == today)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>>
            GetWeeklyScheduleAsync(
                int doctorId)
        {
            var start = DateTime.Today;
            var end = start.AddDays(7);

            return await _context.Appointments
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate >= start &&
                    a.ScheduledDate <= end)
                .ToListAsync();
        }

        public async Task<Appointment>
            GetNextAppointmentByPatientAsync(
                int patientId)
        {
            return await _context.Appointments
                .Where(a =>
                    a.PatientId == patientId &&
                    a.ScheduledDate >= DateTime.Now)
                .OrderBy(a =>
                    a.ScheduledDate)
                .FirstOrDefaultAsync();
        }

        public async Task<bool>
    IsSlotAvailableAsync(
        int doctorId,
        DateTime scheduledDate,
        string timeSlot)
        {
            return !await _context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate == scheduledDate &&
                    a.TimeSlot == timeSlot);
        }

        public async Task<bool>
     IsSlotAvailableForUpdateAsync(
         int appointmentId,
         int doctorId,
         DateTime scheduledDate,
         string timeSlot)
        {
            return !await _context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate == scheduledDate &&
                    a.TimeSlot == timeSlot &&
                    a.AppointmentId != appointmentId);
        }
    }
}