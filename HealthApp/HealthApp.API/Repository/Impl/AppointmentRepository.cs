using HealthApp.API.Data;
using HealthApp.API.Repository.Interface;
using HealthApp.Shared.Constant;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApp.API.Repository.Impl
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HealthAppEntities _db;

        public AppointmentRepository(HealthAppEntities db)
        {
            _db = db;
        }

        // ADD
        public async Task AddAsync(Appointment appointment)
        {
            _db.Appointments.Add(appointment);
            await _db.SaveChangesAsync();
        }

        // SAVE
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        // GET ALL
        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _db.Appointments.ToListAsync();
        }

        // GET BY ID
        public async Task<Appointment> GetByIdAsync(int id)
        {
            return await _db.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
        }

        // CHECK SAME PATIENT DUPLICATE BOOKING
        public async Task<bool> ExistsPatientBookingAsync(
            int patientId,
            int doctorId,
            DateTime date,
            string slot)
        {
            return await _db.Appointments.AnyAsync(a =>
                a.PatientId == patientId &&
                a.DoctorId == doctorId &&
                a.ScheduledDate == date &&
                a.TimeSlot == slot &&
                a.Status != AppointmentStatus.Cancelled);
        }

        // CHECK SLOT BOOKED
        public async Task<bool> IsSlotBookedAsync(
            int doctorId,
            DateTime date,
            string slot)
        {
            return await _db.Appointments.AnyAsync(a =>
                a.DoctorId == doctorId &&
                a.ScheduledDate == date &&
                a.TimeSlot == slot &&
                a.Status != AppointmentStatus.Cancelled);
        }

        // PATIENT APPOINTMENTS
        public async Task<List<Appointment>> GetAppointmentsByPatientAsync(int patientId)
        {
            return await _db.Appointments
                .Where(a => a.PatientId == patientId)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();
        }

        // DOCTOR PENDING APPOINTMENTS
        public async Task<List<Appointment>> GetPendingAppointmentsByDoctorAsync(int doctorId)
        {
            return await _db.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.Status == AppointmentStatus.Pending)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();
        }

        // DOCTOR UPCOMING APPOINTMENTS
        public async Task<List<Appointment>> GetUpcomingAppointmentsByDoctorAsync(
            int doctorId,
            DateTime fromDate,
            DateTime toDate)
        {
            return await _db.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.ScheduledDate >= fromDate &&
                            a.ScheduledDate <= toDate &&
                            a.Status == AppointmentStatus.Confirmed)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();
        }

        // BOOKED SLOTS
        public async Task<List<string>> GetBookedSlotsAsync(
            int doctorId,
            DateTime date)
        {
            return await _db.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.ScheduledDate == date &&
                            a.Status != AppointmentStatus.Cancelled)
                .Select(a => a.TimeSlot)
                .ToListAsync();
        }
    }
}
