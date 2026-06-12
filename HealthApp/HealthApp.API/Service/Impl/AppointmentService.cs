using AutoMapper;
using HealthApp.API.Data;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constant;
using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApp.API.Service.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IPatientRepository _patientRepo;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository appointmentRepo,
            IDoctorRepository doctorRepo,
            IPatientRepository patientRepo,
            IMapper mapper)
        {
            _appointmentRepo = appointmentRepo;
            _doctorRepo = doctorRepo;
            _patientRepo = patientRepo;
            _mapper = mapper;
        }

        // CREATE
        public async Task Add(AppointmentDto dto)
        {
            if (dto.ScheduledDate < DateTime.Today)
                throw new Exception("Cannot book past date.");

            var patient = await _patientRepo.GetByIdAsync(dto.PatientId);

            if (patient == null)
                throw new Exception("Invalid Patient Id.");

            var doctor = await _doctorRepo.GetByIdAsync(dto.DoctorId);

            if (doctor == null || !doctor.IsActive)
                throw new Exception("Doctor unavailable.");

            if (!TimeSlots.Slots.Contains(dto.TimeSlot))
                throw new Exception("Invalid slot.");

            // Time validation for today's booking
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

            bool samePatientBooking =
                await _appointmentRepo.ExistsPatientBookingAsync(
                    dto.PatientId,
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot);

            if (samePatientBooking)
                throw new Exception("You have already booked this doctor for the same slot.");

            bool alreadyBooked =
                await _appointmentRepo.IsSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot);

            if (alreadyBooked)
                throw new Exception("Slot already booked.");

            var appointment = _mapper.Map<Appointment>(dto);
            appointment.Status = AppointmentStatus.Pending;

            await _appointmentRepo.AddAsync(appointment);
        }

        // GET ALL
        public async Task<List<AppointmentDto>> GetAllAppointments()
        {
            var list = await _appointmentRepo.GetAllAsync();

            return _mapper.Map<List<AppointmentDto>>(list);
        }

        // GET BY ID
        public async Task<AppointmentDto> GetAppointmentById(int id)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(id);

            if (appointment == null)
                throw new Exception($"Appointment with id {id} not found");

            return _mapper.Map<AppointmentDto>(appointment);
        }

        // CANCEL
        public async Task CancelAppointment(int appointmentId, string reason)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found");

            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new Exception("Already cancelled");

            if (appointment.Status == AppointmentStatus.Completed)
                throw new Exception("Cannot cancel completed appointment");

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.CancellationReason = reason;

            await _appointmentRepo.SaveAsync();
        }

        // CONFIRM
        public async Task ConfirmAppointment(int appointmentId)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found");

            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new Exception("Cancelled appointment cannot be confirmed");

            if (appointment.Status == AppointmentStatus.Completed)
                throw new Exception("Already completed");

            if (appointment.Status == AppointmentStatus.Confirmed)
                throw new Exception("Already confirmed");

            appointment.Status = AppointmentStatus.Confirmed;

            await _appointmentRepo.SaveAsync();
        }

        // PATIENT APPOINTMENTS
        public async Task<List<AppointmentDto>> GetAppointmentsByPatient(int patientId)
        {
            var list = await _appointmentRepo
                .GetAppointmentsByPatientAsync(patientId);

            return _mapper.Map<List<AppointmentDto>>(list);
        }

        // DOCTOR UPCOMING
        public async Task<List<AppointmentDto>> GetUpcomingAppointmentsByDoctor(
            int doctorId,
            DateTime fromDate,
            DateTime toDate)
        {
            if (fromDate < DateTime.Today)
                throw new Exception("From date cannot be in the past");

            if (fromDate > toDate)
                throw new Exception("Invalid date range");

            var list = await _appointmentRepo
                .GetUpcomingAppointmentsByDoctorAsync(
                    doctorId,
                    fromDate,
                    toDate);

            return _mapper.Map<List<AppointmentDto>>(list);
        }

        // DOCTOR PENDING
        public async Task<List<AppointmentDto>> GetPendingAppointmentsByDoctor(int doctorId)
        {
            var list = await _appointmentRepo
                .GetPendingAppointmentsByDoctorAsync(doctorId);

            return _mapper.Map<List<AppointmentDto>>(list);
        }

        // CHECK AVAILABILITY
        public async Task<List<string>> CheckDoctorAvailability(
            int doctorId,
            DateTime date)
        {
            if (date < DateTime.Today)
                throw new Exception("Date already passed");

            if (date > DateTime.Today.AddDays(90))
                throw new Exception("Only next 90 days allowed");

            var bookedSlots = await _appointmentRepo
                .GetBookedSlotsAsync(doctorId, date);

            var availableSlots = TimeSlots.Slots
                .Except(bookedSlots)
                .ToList();

            if (!availableSlots.Any())
                throw new Exception("No slots available");

            return availableSlots;
        }

        // COMPLETE
        public async Task CompleteAppointment(int appointmentId)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found");

            if (appointment.Status != AppointmentStatus.Confirmed)
                throw new Exception("Only confirmed appointment can be completed");

            appointment.Status = AppointmentStatus.Completed;

            await _appointmentRepo.SaveAsync();
        }
    }
}
