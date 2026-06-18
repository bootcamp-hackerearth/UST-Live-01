using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.DTOs;
using HealthCare.Api.DTOs.Doctor;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using System.Linq.Expressions;

namespace HealthCare.Api.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<Doctor> _repository;
        private readonly HealthCareDbContext _context;
        private readonly IMapper _mapper;

        public DoctorService(IRepository<Doctor> repository, HealthCareDbContext context, IMapper mapper)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }

        public async Task AddAsync(CreateDoctorDto dto)
        {
            var doctor = _mapper.Map<Doctor>(dto);
            await _repository.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateDoctorDto dto)
        {
            var doctor = await _repository.GetByIdAsync(id);
            if (doctor == null)
                throw new DoctorNotFoundException(id);
            _mapper.Map(dto, doctor);

            await _repository.UpdateAsync(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);
            if (doctor == null)
                throw new DoctorNotFoundException(id);
            await _repository.DeleteAsync(id);
            await _context.SaveChangesAsync();
        }

        public async Task<DoctorListDto?> GetByIdAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);
            return doctor == null ? null : _mapper.Map<DoctorListDto?>(doctor);
        }

        public async Task<PagedResult<DoctorListDto>> GetAllAsync(DoctorFilter filter)
        {
            // Build predicate (filtering)
            Expression<Func<Doctor, bool>>? predicate = null;

            if (!string.IsNullOrWhiteSpace(filter.Specialisation) && filter.MinExperience.HasValue)
            {
                predicate = d => d.Specialisation == filter.Specialisation
                              && d.YearsOfExperience >= filter.MinExperience.Value;
            }
            else if (!string.IsNullOrWhiteSpace(filter.Specialisation))
            {
                predicate = d => d.Specialisation == filter.Specialisation;
            }
            else if (filter.MinExperience.HasValue)
            {
                predicate = d => d.YearsOfExperience >= filter.MinExperience.Value;
            }

            // Ordering (by experience)
            Func<IQueryable<Doctor>, IOrderedQueryable<Doctor>> orderBy =
                q => q.OrderByDescending(d => d.YearsOfExperience);

            // Call repository
            var pagedResult = await _repository.GetAllAsync(
                filter.PageNumber,
                filter.PageSize,
                predicate,
                orderBy
            );

            // Map result
            return new PagedResult<DoctorListDto>
            {
                Items = _mapper.Map<IEnumerable<DoctorListDto>>(pagedResult.Items),
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount
            };
        }


    }
}
