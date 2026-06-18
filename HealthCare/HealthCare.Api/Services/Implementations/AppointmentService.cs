using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.DTOs;
using HealthCare.Api.DTOs.Appointment;
using HealthCare.Api.DTOs.Appointments;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using System.Linq.Expressions;

namespace HealthCare.Api.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _repository;
        private readonly IMapper _mapper;
        private readonly HealthCareDbContext _context;

        public AppointmentService(IRepository<Appointment> repository, IMapper mapper, HealthCareDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public async Task AddAsync(CreateAppointmentDto dto)
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
    }
}
