using AutoMapper;
using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.Enums;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Interfaces;

namespace HealthAxisCore_Api.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository repository,
            IDoctorRepository doctorRepository,
            IMapper mapper)
        {
            _repository = repository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentResponseDTO>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<AppointmentResponseDTO>>(data);
        }

        public async Task<PagedResponseDTO<AppointmentResponseDTO>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageSize > 100)
            {
                pageSize = 100;
            }

            var appointments = await _repository.GetAllAsync();

            var query = appointments.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(a =>
                    a.AppointmentId.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    a.PatientId.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    a.DoctorId.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    FormatAppointmentTimeForSearch(a.TimeSlot).Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (status.HasValue)
            {
                query = query.Where(a => a.Status == status.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(a => a.ScheduledDate.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(a => a.ScheduledDate.Date <= endDate.Value.Date);
            }

            var totalCount = query.Count();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedAppointments = query
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => FormatAppointmentTimeForSearch(a.TimeSlot))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var appointmentDtos = _mapper.Map<List<AppointmentResponseDTO>>(pagedAppointments);

            return new PagedResponseDTO<AppointmentResponseDTO>
            {
                Items = appointmentDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        public async Task<AppointmentResponseDTO?> GetByIdAsync(int id)
        {
            var appt = await _repository.GetByIdAsync(id);

            if (appt == null)
            {
                throw new EntityNotFoundException("Appointment not found");
            }

            return _mapper.Map<AppointmentResponseDTO>(appt);
        }

        public async Task<AppointmentResponseDTO> CreateAsync(CreateAppointmentDTO dto)
        {
            if (dto == null)
            {
                throw new AppointmentRuleException("Appointment details are required.");
            }

            if (string.IsNullOrWhiteSpace(dto.TimeSlot))
            {
                throw new AppointmentRuleException("Time slot is required.");
            }

            var requestedTimeSlot = NormalizeTimeSlot(dto.TimeSlot);

            if (string.IsNullOrWhiteSpace(requestedTimeSlot))
            {
                throw new AppointmentRuleException("Invalid time slot format.");
            }

            var selectedDateTime = BuildAppointmentDateTime(dto.ScheduledDate, dto.TimeSlot);

            if (selectedDateTime <= DateTime.Now)
            {
                throw new AppointmentRuleException("Previous date or past time slot cannot be booked.");
            }

            var today = DateTime.Today;
            var maxAllowedDate = today.AddDays(30);

            if (dto.ScheduledDate.Date > maxAllowedDate)
            {
                throw new AppointmentRuleException("Appointments can only be booked up to 30 days in advance.");
            }

            bool isAvailable = await _doctorRepository.IsDoctorAvailable(
                dto.DoctorId,
                dto.ScheduledDate
            );

            if (!isAvailable)
            {
                throw new AppointmentRuleException("Doctor not available for the selected date");
            }

            var existingAppointments = await _repository.GetAllAsync();

            var requestedDate = dto.ScheduledDate.Date;

            var activeAppointments = existingAppointments
                .Where(a => IsActiveAppointmentStatus(a.Status))
                .ToList();

            bool doctorSlotAlreadyBooked = activeAppointments.Any(a =>
                a.DoctorId == dto.DoctorId &&
                a.ScheduledDate.Date == requestedDate &&
                NormalizeTimeSlot(a.TimeSlot) == requestedTimeSlot
            );

            if (doctorSlotAlreadyBooked)
            {
                throw new AppointmentRuleException(
                    "This doctor is already booked for the selected date and time slot. Please choose another slot."
                );
            }

            var patientActiveAppointmentsSameDate = activeAppointments
                .Where(a =>
                    a.PatientId == dto.PatientId &&
                    a.ScheduledDate.Date == requestedDate)
                .ToList();

            if (patientActiveAppointmentsSameDate.Any())
            {
                var sameDoctorAppointment = patientActiveAppointmentsSameDate
                    .FirstOrDefault(a => a.DoctorId == dto.DoctorId);

                if (sameDoctorAppointment != null)
                {
                    if (NormalizeTimeSlot(sameDoctorAppointment.TimeSlot) == requestedTimeSlot)
                    {
                        throw new AppointmentRuleException(
                            "You already have an active appointment with this doctor on the same date and time slot."
                        );
                    }

                    throw new AppointmentRuleException(
                        "You already have an active appointment with this doctor on the selected date."
                    );
                }

                throw new AppointmentRuleException(
                    "You already have an active appointment on this date. Please choose another date."
                );
            }

            var appointment = _mapper.Map<Appointment>(dto);

            appointment.ScheduledDate = dto.ScheduledDate.Date;
            appointment.TimeSlot = requestedTimeSlot;
            appointment.Status = AppointmentStatus.Pending;
            appointment.CreatedDate = DateTime.Now;

            await _repository.AddAsync(appointment);

            return _mapper.Map<AppointmentResponseDTO>(appointment);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _repository.Exists(id);

            if (!exists)
            {
                throw new EntityNotFoundException("Appointment not found");
            }

            await _repository.DeleteAsync(id);

            return true;
        }

        public async Task<IEnumerable<AppointmentResponseDTO>> GetByDoctorAsync(int doctorId)
        {
            var data = await _repository.GetByDoctor(doctorId);

            if (data == null || !data.Any())
            {
                throw new EntityNotFoundException("No appointments found for this doctor");
            }

            return _mapper.Map<IEnumerable<AppointmentResponseDTO>>(data);
        }

        public async Task<IEnumerable<AppointmentResponseDTO>> GetByPatientAsync(int patientId)
        {
            var data = await _repository.GetByPatient(patientId);

            if (data == null || !data.Any())
            {
                throw new EntityNotFoundException("No appointments found for this patient");
            }

            return _mapper.Map<IEnumerable<AppointmentResponseDTO>>(data);
        }

        public async Task<IEnumerable<AppointmentResponseDTO>> FilterAsync(
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            var data = await _repository.FilterAppointments(status, startDate, endDate);

            if (data == null || !data.Any())
            {
                throw new EntityNotFoundException("No appointments found for given criteria");
            }

            return _mapper.Map<IEnumerable<AppointmentResponseDTO>>(data);
        }

        public async Task<bool> CancelAsync(int id, string reason)
        {
            var appt = await _repository.GetByIdAsync(id);

            if (appt == null)
            {
                throw new EntityNotFoundException("Appointment not found");
            }

            if (appt.Status == AppointmentStatus.Cancelled)
            {
                throw new AppointmentRuleException("Appointment already cancelled");
            }

            if (appt.Status == AppointmentStatus.Completed)
            {
                throw new AppointmentRuleException("Cannot cancel a completed appointment");
            }

            await _repository.CancelAppointment(id, reason);

            return true;
        }

        public async Task<bool> ConfirmAsync(int id)
        {
            var appt = await _repository.GetByIdAsync(id);

            if (appt == null)
            {
                throw new EntityNotFoundException("Appointment not found");
            }

            if (appt.Status == AppointmentStatus.Completed)
            {
                throw new AppointmentRuleException("Cannot confirm a completed appointment");
            }

            if (appt.Status == AppointmentStatus.Cancelled)
            {
                throw new AppointmentRuleException("Cannot confirm a cancelled appointment");
            }

            if (appt.Status == AppointmentStatus.Confirmed)
            {
                throw new AppointmentRuleException("Appointment is already confirmed");
            }

            await _repository.ConfirmAppointment(id);

            return true;
        }

        public async Task<bool> CompleteAsync(int id)
        {
            var appt = await _repository.GetByIdAsync(id);

            if (appt == null)
            {
                throw new EntityNotFoundException("Appointment not found");
            }

            if (appt.Status == AppointmentStatus.Completed)
            {
                throw new AppointmentRuleException("Appointment is already completed");
            }

            if (appt.Status == AppointmentStatus.Cancelled)
            {
                throw new AppointmentRuleException("Cannot complete a cancelled appointment");
            }

            if (appt.Status == AppointmentStatus.Pending)
            {
                throw new AppointmentRuleException("Only confirmed appointments can be completed");
            }

            if (appt.Status != AppointmentStatus.Confirmed)
            {
                throw new AppointmentRuleException("Only confirmed appointments can be completed");
            }

            await _repository.CompleteAppointment(id);

            return true;
        }

        public async Task<IEnumerable<string>> GetBookedSlotsAsync(int doctorId, DateTime date)
        {
            if (doctorId <= 0)
            {
                throw new AppointmentRuleException("Doctor ID is required.");
            }

            if (date == default)
            {
                throw new AppointmentRuleException("Appointment date is required.");
            }

            var bookedSlots = await _repository.GetBookedSlotsAsync(doctorId, date);

            return bookedSlots
                .Select(slot => NormalizeTimeSlot(slot))
                .Where(slot => !string.IsNullOrWhiteSpace(slot))
                .Distinct()
                .OrderBy(slot => slot)
                .ToList();
        }

        private bool IsActiveAppointmentStatus(AppointmentStatus status)
        {
            return status == AppointmentStatus.Pending ||
                   status == AppointmentStatus.Confirmed;
        }

        private DateTime BuildAppointmentDateTime(DateTime scheduledDate, string timeSlot)
        {
            if (string.IsNullOrWhiteSpace(timeSlot))
            {
                throw new AppointmentRuleException("Time slot is required.");
            }

            if (TimeSpan.TryParse(timeSlot, out var time))
            {
                return scheduledDate.Date.Add(time);
            }

            if (DateTime.TryParse(timeSlot, out var parsedDateTime))
            {
                return scheduledDate.Date.Add(parsedDateTime.TimeOfDay);
            }

            throw new AppointmentRuleException("Invalid time slot format.");
        }

        private string NormalizeTimeSlot(object? timeSlot)
        {
            if (timeSlot == null)
            {
                return string.Empty;
            }

            if (timeSlot is TimeOnly timeOnly)
            {
                return timeOnly.ToString("HH:mm");
            }

            if (timeSlot is TimeSpan timeSpan)
            {
                return timeSpan.ToString(@"hh\:mm");
            }

            if (timeSlot is DateTime dateTime)
            {
                return dateTime.ToString("HH:mm");
            }

            var value = timeSlot.ToString();

            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            if (value.Contains("-"))
            {
                value = value.Split('-')[0].Trim();
            }

            if (TimeSpan.TryParse(value, out var parsedTimeSpan))
            {
                return parsedTimeSpan.ToString(@"hh\:mm");
            }

            if (DateTime.TryParse(value, out var parsedDateTime))
            {
                return parsedDateTime.ToString("HH:mm");
            }

            return value.Trim();
        }

        private string FormatAppointmentTimeForSearch(object? timeSlot)
        {
            if (timeSlot == null)
            {
                return string.Empty;
            }

            if (timeSlot is TimeOnly timeOnly)
            {
                return timeOnly.ToString("hh:mm tt");
            }

            if (timeSlot is TimeSpan timeSpan)
            {
                return DateTime.Today.Add(timeSpan).ToString("hh:mm tt");
            }

            if (timeSlot is DateTime dateTime)
            {
                return dateTime.ToString("hh:mm tt");
            }

            return timeSlot.ToString() ?? string.Empty;
        }
    }
}