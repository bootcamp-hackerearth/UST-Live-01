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
        private readonly IAppointmentRepository _repo;
        private readonly IMapper _mapper;

        private const string NOT_FOUND = "Appointment not found";
        private const string ALREADY_CANCELLED = "Already cancelled";
        private const string ALREADY_CONFIRMED = "Already confirmed";
        private const string ALREADY_COMPLETED = "Already completed";
        private const string INVALID_CANCEL = "Cannot cancel completed appointment";
        private const string ONLY_CONFIRMED = "Only confirmed appointment can be completed";
        private const string INVALID_SLOT = "Invalid slot";
        private const string SLOT_OVER = "Slot already over";
        private const string SLOT_BOOKED = "Slot already booked";
        private const string INVALID_DATE_RANGE = "Invalid date range";

        public AppointmentService(IAppointmentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task Add(AppointmentDto dto)
        {
            if (dto == null)
                throw new ArgumentException("Invalid appointment");

            if (dto.ScheduledDate < DateTime.Today)
                throw new ArgumentException("Cannot book past date");

            var doctor = await _repo.GetDoctorById(dto.DoctorId);

            if (doctor == null || (bool)!doctor.IsActive)
                throw new ArgumentException("Doctor not found");

            if (!TimeSlots.Slots.Contains(dto.TimeSlot))
                throw new ArgumentException(INVALID_SLOT);

            // ✅ FIXED: Culture info added (Sonar fix)
            if (dto.ScheduledDate.Date == DateTime.Today)
            {
                var slotTime = DateTime.ParseExact(
                    dto.TimeSlot,
                    "hh:mm tt",
                    CultureInfo.InvariantCulture);

                var finalTime = dto.ScheduledDate.Date + slotTime.TimeOfDay;

                if (finalTime < DateTime.Now)
                    throw new ArgumentException(SLOT_OVER);
            }

            bool alreadyBooked = await _repo.IsSlotBooked(
                dto.DoctorId,
                dto.ScheduledDate,
                dto.TimeSlot);

            if (alreadyBooked)
                throw new ArgumentException(SLOT_BOOKED);

            var appointment = _mapper.Map<Appointment>(dto);
            appointment.Status = AppointmentStatus.Pending;

            await _repo.Add(appointment);
        }

        public async Task<List<AppointmentDto>> GetAllAppointments()
        {
            var data = await _repo.GetAll();
            return _mapper.Map<List<AppointmentDto>>(data);
        }

        public async Task<AppointmentDto> GetAppointmentById(int id)
        {
            var data = await _repo.GetById(id);

            if (data == null)
                throw new ArgumentException(NOT_FOUND);

            return _mapper.Map<AppointmentDto>(data);
        }

        public async Task CancelAppointment(int id, string reason)
        {
            var appt = await _repo.GetById(id);

            if (appt == null)
                throw new ArgumentException(NOT_FOUND);

            if (appt.Status == AppointmentStatus.Cancelled)
                throw new ArgumentException(ALREADY_CANCELLED);

            if (appt.Status == AppointmentStatus.Completed)
                throw new ArgumentException(INVALID_CANCEL);

            appt.Status = AppointmentStatus.Cancelled;
            appt.CancellationReason = reason;

            await _repo.Update(appt);
        }

        public async Task ConfirmAppointment(int id)
        {
            var appt = await _repo.GetById(id);

            if (appt == null)
                throw new ArgumentException(NOT_FOUND);

            if (appt.Status == AppointmentStatus.Confirmed)
                throw new ArgumentException(ALREADY_CONFIRMED);

            if (appt.Status == AppointmentStatus.Cancelled)
                throw new ArgumentException("Cancelled appointment cannot be confirmed");

            if (appt.Status == AppointmentStatus.Completed)
                throw new ArgumentException(ALREADY_COMPLETED);

            appt.Status = AppointmentStatus.Confirmed;

            await _repo.Update(appt);
        }

        public async Task CompleteAppointment(int id)
        {
            var appt = await _repo.GetById(id);

            if (appt == null)
                throw new ArgumentException(NOT_FOUND);

            if (appt.Status != AppointmentStatus.Confirmed)
                throw new ArgumentException(ONLY_CONFIRMED);

            appt.Status = AppointmentStatus.Completed;

            await _repo.Update(appt);
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByPatient(int patientId)
            => _mapper.Map<List<AppointmentDto>>(await _repo.GetByPatient(patientId));

        public async Task<List<AppointmentDto>> GetUpcomingAppointmentsByDoctor(
            int doctorId, DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate)
                throw new ArgumentException(INVALID_DATE_RANGE);

            return _mapper.Map<List<AppointmentDto>>(
                await _repo.GetUpcomingByDoctor(doctorId, fromDate, toDate));
        }

        public async Task<List<AppointmentDto>> GetPendingAppointmentsByDoctor(int doctorId)
            => _mapper.Map<List<AppointmentDto>>(await _repo.GetPendingByDoctor(doctorId));

        public async Task<List<string>> CheckDoctorAvailability(int doctorId, DateTime date)
        {
            var booked = await _repo.GetBookedSlots(doctorId, date);
            return TimeSlots.Slots.Except(booked).ToList();
        }
    }
}