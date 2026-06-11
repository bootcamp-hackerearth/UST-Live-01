using AutoMapper;
using HealthApp.API.Data;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constant;
using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApp.API.Service.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly HealthAppEntities _db;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository repo,
                                  HealthAppEntities db,
                                  IMapper mapper)
        {
            _repo = repo;
            _db = db;
            _mapper = mapper;
        }

        // ✅ CREATE
        public async Task Add(AppointmentDto dto)
        {
            if (dto.ScheduledDate < DateTime.Today)
                throw new Exception("Cannot book past date.");

            var doctor = await _db.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == dto.DoctorId);

            if (doctor == null || !(doctor.IsActive))
                throw new Exception("Doctor unavailable.");

            if (!TimeSlots.Slots.Contains(dto.TimeSlot))
                throw new Exception("Invalid slot.");

            // ✅ Time validation
            if (dto.ScheduledDate == DateTime.Today)
            {
                DateTime slotTime = DateTime.ParseExact(
                    dto.TimeSlot,
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);

                DateTime finalTime = dto.ScheduledDate + slotTime.TimeOfDay;

                if (finalTime < DateTime.Now)
                    throw new Exception("Slot already over.");
            }

            // ✅ Check duplicate booking (DB-side)
            bool alreadyBooked = await _db.Appointments.AnyAsync(a =>
                a.DoctorId == dto.DoctorId &&
                a.ScheduledDate == dto.ScheduledDate &&
                a.TimeSlot == dto.TimeSlot &&
                a.Status != AppointmentStatus.Cancelled
            );

            if (alreadyBooked)
                throw new Exception("Slot already booked.");

            var appointment = _mapper.Map<Appointment>(dto);
            appointment.Status = AppointmentStatus.Pending;

            _db.Appointments.Add(appointment);
            await _db.SaveChangesAsync();
        }

        // ✅ GET ALL
        public async Task<List<AppointmentDto>> GetAllAppointments()
        {
            var list = await _db.Appointments.ToListAsync();
            return _mapper.Map<List<AppointmentDto>>(list);
        }

        // ✅ GET BY ID
        public async Task<AppointmentDto> GetAppointmentById(int id)
        {
            var appointment = await _db.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
                throw new Exception($"Appointment with id {id} not found");

            return _mapper.Map<AppointmentDto>(appointment);
        }

        // ✅ CANCEL
        public async Task CancelAppointment(int appointmentId, string reason)
        {
            var appointment = await _db.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found");

            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new Exception("Already cancelled");

            if (appointment.Status == AppointmentStatus.Completed)
                throw new Exception("Cannot cancel completed appointment");

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.CancellationReason = reason;

            await _db.SaveChangesAsync();
        }

        // ✅ CONFIRM
        public async Task ConfirmAppointment(int appointmentId)
        {
            var appointment = await _db.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found");

            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new Exception("Cancelled appointment cannot be confirmed");

            if (appointment.Status == AppointmentStatus.Completed)
                throw new Exception("Already completed");

            if (appointment.Status == AppointmentStatus.Confirmed)
                throw new Exception("Already confirmed");

            appointment.Status = AppointmentStatus.Confirmed;

            await _db.SaveChangesAsync();
        }

        // ✅ PATIENT APPOINTMENTS
        public async Task<List<AppointmentDto>> GetAppointmentsByPatient(int patientId)
        {
            var list = await _db.Appointments
                .Where(a => a.PatientId == patientId)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();

            return _mapper.Map<List<AppointmentDto>>(list);
        }

        // ✅ DOCTOR UPCOMING
        public async Task<List<AppointmentDto>> GetUpcomingAppointmentsByDoctor(
            int doctorId, DateTime fromDate, DateTime toDate)
        {
            if (fromDate < DateTime.Today)
                throw new Exception("From date cannot be in the past");

            if (fromDate > toDate)
                throw new Exception("Invalid date range");

            var list = await _db.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.ScheduledDate >= fromDate &&
                            a.ScheduledDate <= toDate &&
                            a.Status == AppointmentStatus.Confirmed)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();

            return _mapper.Map<List<AppointmentDto>>(list);
        }

        // ✅ DOCTOR PENDING
        public async Task<List<AppointmentDto>> GetPendingAppointmentsByDoctor(int doctorId)
        {
            var list = await _db.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.Status == AppointmentStatus.Pending)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();

            return _mapper.Map<List<AppointmentDto>>(list);
        }

        // ✅ CHECK AVAILABILITY
        public async Task<List<string>> CheckDoctorAvailability(int doctorId, DateTime date)
        {
            if (date < DateTime.Today)
                throw new Exception("Date already passed");

            if (date > DateTime.Today.AddDays(90))
                throw new Exception("Only next 90 days allowed");

            var bookedSlots = await _db.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.ScheduledDate == date &&
                            a.Status != AppointmentStatus.Cancelled)
                .Select(a => a.TimeSlot)
                .ToListAsync();

            var availableSlots = TimeSlots.Slots
                .Except(bookedSlots)
                .ToList();

            if (!availableSlots.Any())
                throw new Exception("No slots available");

            return availableSlots;
        }

        // ✅ COMPLETE
        public async Task CompleteAppointment(int appointmentId)
        {
            var appointment = await _db.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found");

            if (appointment.Status != AppointmentStatus.Confirmed)
                throw new Exception("Only confirmed appointment can be completed");

            appointment.Status = AppointmentStatus.Completed;

            await _db.SaveChangesAsync();
        }
    }
}