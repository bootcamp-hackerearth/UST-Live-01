using Microsoft.EntityFrameworkCore;
using HealthAxis.API.Data;
using S3_HealthAxisApi.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;


namespace S3_HealthAxisApi.Repository.Implementation
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HealthAxisDbContext _context;

        public AppointmentRepository(HealthAxisDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
        }

        public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetDoctorTodayScheduleAsync(int doctorId, DateOnly today)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId && a.ScheduledDate == today)
                .OrderBy(a => a.TimeSlot)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetDoctorWeekScheduleAsync(int doctorId, DateOnly startDate, DateOnly endDate)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId &&
                            a.ScheduledDate >= startDate &&
                            a.ScheduledDate <= endDate)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();
        }

        public async Task<bool> ExistsSamePatientSameDoctorSameDateAsync(int patientId, int doctorId, DateOnly date)
        {
            return await _context.Appointments.AnyAsync(a =>
                a.PatientId == patientId &&
                a.DoctorId == doctorId &&
                a.ScheduledDate == date &&
                a.Status != AppointmentStatus.Cancelled);
        }

        public async Task<bool> ExistsSamePatientSameSlotSameDateAsync(int patientId, DateOnly date, int timeSlot)
        {
            if (!Enum.IsDefined(typeof(AppointmentTimeSlot), timeSlot))
                throw new ArgumentException("Invalid appointment time slot.");

            var slotEnum = (AppointmentTimeSlot)timeSlot;

            return await _context.Appointments.AnyAsync(a =>
                a.PatientId == patientId &&
                a.ScheduledDate == date &&
                a.TimeSlot == slotEnum &&
                a.Status != AppointmentStatus.Cancelled);
        }

        public async Task<bool> ExistsSameDoctorSameSlotSameDateAsync(int doctorId, DateOnly date, int timeSlot)
        {
            if (!Enum.IsDefined(typeof(AppointmentTimeSlot), timeSlot))
                throw new ArgumentException("Invalid appointment time slot.");

            var slotEnum = (AppointmentTimeSlot)timeSlot;

            return await _context.Appointments.AnyAsync(a =>
                a.DoctorId == doctorId &&
                a.ScheduledDate == date &&
                a.TimeSlot == slotEnum &&
                a.Status != AppointmentStatus.Cancelled);
        }

        public async Task<bool> ExistsSamePatientSameDoctorSameDateAsync(int patientId, int doctorId, DateOnly date, int appointmentId)
        {
            return await _context.Appointments.AnyAsync(a =>
                a.AppointmentId != appointmentId &&
                a.PatientId == patientId &&
                a.DoctorId == doctorId &&
                a.ScheduledDate == date &&
                a.Status != AppointmentStatus.Cancelled);
        }

        public async Task<bool> ExistsSamePatientSameSlotSameDateAsync(int patientId, DateOnly date, int timeSlot, int appointmentId)
        {
            return await _context.Appointments.AnyAsync(a =>
                a.AppointmentId != appointmentId &&
                a.PatientId == patientId &&
                a.ScheduledDate == date &&
                (int)a.TimeSlot == timeSlot &&
                a.Status != AppointmentStatus.Cancelled);
        }

        public async Task<bool> ExistsSameDoctorSameSlotSameDateAsync(int doctorId, DateOnly date, int timeSlot, int appointmentId)
        {
            return await _context.Appointments.AnyAsync(a =>
                a.AppointmentId != appointmentId &&
                a.DoctorId == doctorId &&
                a.ScheduledDate == date &&
                (int)a.TimeSlot == timeSlot &&
                a.Status != AppointmentStatus.Cancelled);
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
        }

        public Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Appointments.AnyAsync(a => a.AppointmentId == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}