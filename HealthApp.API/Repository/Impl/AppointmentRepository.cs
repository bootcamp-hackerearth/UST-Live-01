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
        private readonly HealthAppDBEntities _db;

        public AppointmentRepository(HealthAppDBEntities db)
        {
            _db = db;
        }

        public async Task Add(Appointment appointment)
        {
            _db.Appointments.Add(appointment);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Appointment appointment)
        {
            _db.Entry(appointment).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetAll()
        {
            return await _db.Appointments.ToListAsync();
        }

        public async Task<Appointment> GetById(int id)
        {
            return await _db.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
        }

        public async Task<List<Appointment>> GetByPatient(int patientId)
        {
            return await _db.Appointments
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetUpcomingByDoctor(int doctorId, DateTime from, DateTime to)
        {
            return await _db.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.ScheduledDate >= from &&
                            a.ScheduledDate <= to &&
                            a.Status == AppointmentStatus.Confirmed)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetPendingByDoctor(int doctorId)
        {
            return await _db.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.Status == AppointmentStatus.Pending)
                .ToListAsync();
        }

        public async Task<bool> IsSlotBooked(int doctorId, DateTime date, string timeSlot)
        {
            return await _db.Appointments.AnyAsync(a =>
                a.DoctorId == doctorId &&
                DbFunctions.TruncateTime(a.ScheduledDate) == date.Date &&
                a.TimeSlot == timeSlot &&
                a.Status != AppointmentStatus.Cancelled);
        }

        public async Task<List<string>> GetBookedSlots(int doctorId, DateTime date)
        {
            return await _db.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            DbFunctions.TruncateTime(a.ScheduledDate) == date.Date &&
                            a.Status != AppointmentStatus.Cancelled)
                .Select(a => a.TimeSlot)
                .ToListAsync();
        }

        public async Task<Doctor> GetDoctorById(int doctorId)
        {
            return await _db.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId);
        }
    }
}