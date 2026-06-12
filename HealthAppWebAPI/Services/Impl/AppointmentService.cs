using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using HealthAppWebAPI.Enums;
using HealthAppWebAPI.Models.Dtos;
using HealthAppWebAPI.Repositories.Interfaces;
using HealthAppWebAPI.Services.Interfaces;
using System.Threading.Tasks;
using HealthAppWebAPI.Constants;

namespace HealthAppWebAPI.Services.Impl
{


    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        {
            var appointments = await _repo.GetAllAsync();
            return _mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<AppointmentDto>> GetUpcomingAppointmentsForDoctorAsync(int doctorId)
        {
            var appointments = await _repo
                .GetUpcomingAppointmentsByDoctorAsync(doctorId);

            return _mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<AppointmentDto>> GetAppointmentsForPatientAsync(int patientId)
        {
            var appointments = await _repo
                .GetAppointmentsByPatientAsync(patientId);

            return _mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task BookAppointmentAsync(CreateAppointmentDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("Appointment data is required.");
            }

            if (dto.PatientId <= 0)
            {
                throw new ArgumentException("Please select a valid patient.");
            }

            if (dto.DoctorId <= 0)
            {
                throw new ArgumentException("Please select a valid doctor.");
            }

            if (dto.ScheduledDate == default(DateTime))
            {
                throw new ArgumentException("Please select appointment date.");
            }

            if (string.IsNullOrWhiteSpace(dto.TimeSlot))
            {
                throw new ArgumentException("Please select time slot.");
            }

            dto.TimeSlot = dto.TimeSlot.Trim();

            if (dto.ScheduledDate.Date < DateTime.Today)
            {
                throw new InvalidOperationException("Past date not allowed.");
            }

            bool validSlot =
                TimeSlots.Slots.Any(s =>
                    string.Equals(
                        s.Trim(),
                        dto.TimeSlot,
                        StringComparison.OrdinalIgnoreCase));

            if (!validSlot)
            {
                throw new InvalidOperationException("Invalid time slot selected.");
            }

            if (dto.ScheduledDate.Date == DateTime.Today)
            {
                DateTime slotDateTime =
                    GetSlotDateTime(dto.ScheduledDate, dto.TimeSlot);

                if (slotDateTime < DateTime.Now)
                {
                    throw new InvalidOperationException("Cannot book a past time slot.");
                }
            }

            if (await _repo.IsDoctorSlotBookedAsync(
                dto.DoctorId,
                dto.ScheduledDate,
                dto.TimeSlot))
            {
                throw new InvalidOperationException(
                    "Doctor is already booked for this time slot.");
            }

            if (await _repo.HasPatientSlotConflictAsync(
                dto.PatientId,
                dto.ScheduledDate,
                dto.TimeSlot))
            {
                throw new InvalidOperationException(
                    "Patient already has an appointment for this time slot.");
            }

            if (await _repo.HasAppointmentWithDoctorOnSameDayAsync(
                dto.PatientId,
                dto.DoctorId,
                dto.ScheduledDate))
            {
                throw new InvalidOperationException(
                    "Patient already has an appointment with this doctor on the selected date.");
            }

            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ScheduledDate = dto.ScheduledDate,
                TimeSlot = dto.TimeSlot,
                Status = AppointmentStatus.Pending.ToString()
            };

            await _repo.AddAsync(appointment);
        }

        public async Task ConfirmAppointmentAsync(int id)
        {
            var appointment = await _repo.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found.");
            }

            appointment.Status = AppointmentStatus.Confirmed.ToString();

            await _repo.UpdateAsync(appointment);
        }


        public async Task CancelAppointmentAsync(int id, string reason)
        {
            var appointment = await _repo.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found.");
            }

            if (appointment.Status == AppointmentStatus.Completed.ToString())
            {
                throw new InvalidOperationException(
                    "Cannot cancel completed appointment.");
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException("Cancellation reason is required.");
            }

            appointment.Status = AppointmentStatus.Cancelled.ToString();
            appointment.CancellationReason = reason;

            await _repo.UpdateAsync(appointment);
        }


        private DateTime GetSlotDateTime(DateTime date, string slot)
        {
            DateTime parsedTime;

            bool parsed =
                DateTime.TryParse(slot, out parsedTime);

            if (!parsed)
            {
                throw new ArgumentException("Invalid time slot format.");
            }

            return new DateTime(
                date.Year,
                date.Month,
                date.Day,
                parsedTime.Hour,
                parsedTime.Minute,
                0);
        }
    }
}