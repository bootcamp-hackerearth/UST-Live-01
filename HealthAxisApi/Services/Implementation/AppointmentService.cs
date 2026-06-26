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
            bool isAvailable = await _doctorRepository.IsDoctorAvailable(
                dto.DoctorId,
                dto.ScheduledDate
            );

            if (!isAvailable)
            {
                throw new AppointmentRuleException("Doctor not available for the selected date");
            }

            var appointment = _mapper.Map<Appointment>(dto);

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

            await _repository.ConfirmAppointment(id);

            return true;
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