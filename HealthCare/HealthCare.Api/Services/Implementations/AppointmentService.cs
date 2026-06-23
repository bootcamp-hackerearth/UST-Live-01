using AutoMapper;
using HealthCare.Api.Data;
using Healthcare.Shared.DTOs;
using Healthcare.Shared.DTOs.Appointment;
using Healthcare.Shared.DTOs.Appointments;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using System.Linq.Expressions;

namespace HealthCare.Api.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;
        private readonly HealthCareDbContext _context;

        public AppointmentService(IAppointmentRepository repository, IDoctorService doctorService, HealthCareDbContext context, IMapper mapper)
        {
            _repository = repository;
            _doctorService = doctorService;
            _context = context;
            _mapper = mapper;
        }

        public async Task AddAsync(CreateAppointmentDto dto,int id)
        {
            var appointment = _mapper.Map<Appointment>(dto);
            await _repository.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateAppointmentDto dto)
        {
            var appointment = await _repository.GetByIdAsync(id);
            if (appointment == null)
                throw new AppointmentNotFoundException(id);
            _mapper.Map(dto, appointment);

            await _repository.UpdateAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _repository.GetByIdAsync(id);
            if (appointment == null)
                throw new AppointmentNotFoundException(id);
            await _repository.DeleteAsync(id);
            await _context.SaveChangesAsync();
        }

        public async Task<AppointmentListDto?> GetByIdAsync(int id)
        {
            var appointment = await _repository.GetByIdAsync(id);
            return appointment == null ? null : _mapper.Map<AppointmentListDto?>(appointment);
        }

        public async Task<PagedResult<AppointmentListDto>> GetAllAsync(AppointmentFilter filter)
        {
            // Build predicate (filtering)
            Expression<Func<Appointment, bool>>? predicate = null;

            if (!string.IsNullOrWhiteSpace(filter.Status) && filter.ScheduledDate.HasValue)
            {
                predicate = a =>
                    a.Status == filter.Status &&
                    a.ScheduledDate == filter.ScheduledDate.Value;
            }
            else if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                predicate = a => a.Status == filter.Status;
            }
            else if (filter.ScheduledDate.HasValue)
            {
                predicate = a => a.ScheduledDate == filter.ScheduledDate.Value;
            }

            // Ordering (by scheduled date)
            Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>> orderBy =
                q => q.OrderBy(a => a.ScheduledDate);

            // Call repository
            var pagedResult = await _repository.GetAllAsync(
                filter.PageNumber,
                filter.PageSize,
                predicate,
                orderBy
            );

            // Map result
            return new PagedResult<AppointmentListDto>
            {
                Items = _mapper.Map<IEnumerable<AppointmentListDto>>(pagedResult.Items),
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount
            };
        }

        public async Task UpdateStatusAsync(int id, UpdateAppointmentDto dto)
        {
            var appointment = await _repository.GetByIdAsync(id);

            if (appointment is null)
                throw new InvalidOperationException("Patient not found.");

            appointment.Status = dto.Status;
            appointment.CancellationReason = dto.CancellationReason;

            await _repository.UpdateAsync(appointment);
            await _context.SaveChangesAsync();
        }
        public async Task<List<string>> AvailableTimeSlots(DateOnly date, int doctorId)
        {
            if (date < DateOnly.FromDateTime(DateTime.Today))
                throw new InvalidOperationException("Cannot check availability for a past date.");

            var allSlots = await _doctorService.GetSlots(doctorId);
            var bookedSlots = await _repository.AvailableTimeSlots(date, doctorId);

            var freeSlots = allSlots.Except(bookedSlots).ToList();

            return freeSlots;
        }

        public async Task<bool> IsAvailable(DateOnly date, int doctorId, string timeSlot)
        {
            var available = await _repository.IsAvailable(date, doctorId, timeSlot);

            if (!available)
                throw new InvalidOperationException("This time slot is already booked.");

            return true;
        }

        public async Task<List<AppointmentReportDto>> GetDailyReport()
        {
            var report = await _repository.GetDailyReport();
            return report.Count == 0 ? new List<AppointmentReportDto>() : report;
        }

        public async Task<List<AppointmentListDto>> GetDoctorSchedule(DateOnly date, int id)
        {
            var schedule = await _repository.GetDoctorSchedule(date, id);
            return schedule.Count == 0 ? new List<AppointmentListDto>() : schedule;
        }

        public async Task<List<AppointmentListDto>> GetPatientSchedule(DateOnly date, int id)
        {
            var schedule = await _repository.GetPatientSchedule(date, id);
            return schedule.Count == 0 ? new List<AppointmentListDto>() : schedule;
        }

        public async Task<List<AppointmentListDto>> GetAppointmentByPatient(int id)
        {
            var appointments = await _repository.GetAppointmentByPatient(id);
            return appointments.Count == 0 ? new List<AppointmentListDto>() : appointments;
        }

        public async Task<List<AppointmentListDto>> GetAppointmentByDoctor(int id)
        {
            var appointments = await _repository.GetAppointmentByDoctor(id);
            return appointments.Count == 0 ? new List<AppointmentListDto>() : appointments;
        }

        public async Task CancelAppointmentsByDoctorDate(int doctorId, DateOnly date)
        {
            await _repository.CancelAppointmentsByDoctorDate(doctorId, date);
            await _context.SaveChangesAsync();
        }
    }
}
