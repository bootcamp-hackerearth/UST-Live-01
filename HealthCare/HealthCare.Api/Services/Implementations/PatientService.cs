using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.DTOs;
using HealthCare.Api.DTOs.Patient;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using HealthCare.Shared.DTOs;
using HealthCare.Shared.DTOs.Patient;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Api.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _repository;
        private readonly IPatientRepository _patientRepository;
        private readonly HealthCareDbContext _context;
        private readonly IMapper _mapper;

        private const string NotFoundExceptionMessage = "Patient not found.";

        public PatientService(
            IRepository<Patient> repository,
            IPatientRepository patientRepository,
            HealthCareDbContext context,
            IMapper mapper)
        {
            _repository = repository;
            _patientRepository = patientRepository;
            _context = context;
            _mapper = mapper;
        }

        
        public async Task<PatientListDto> GetByIdAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);

            if (patient is null)
                throw new InvalidOperationException(NotFoundExceptionMessage);

            return _mapper.Map<PatientListDto>(patient);
        }

        public async Task<PatientProfileDto?> GetMyProfileAsync(int patientId)
        {
            var profile = await (
                from patient in _context.Patients
                join user in _context.Users
                    on patient.UserId equals user.Id
                where patient.PatientId == patientId
                select new PatientProfileDto
                {
                    PatientId = patient.PatientId,
                    FullName = patient.FullName,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = patient.PhoneNumber,
                    DateOfBirth = patient.DateOfBirth,
                    Gender = patient.Gender,
                    InsuranceId = patient.InsuranceId
                }
            ).FirstOrDefaultAsync();

            return profile;
        }

        public async Task<PagedResult<PatientListDto>> GetAllAsync(PatientFilter filter)
        {
            var query = _patientRepository.GetQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(p =>
                    p.FullName.Contains(filter.Search));
            }

            if (filter.HasInsurance == true)
            {
                query = query.Where(p => p.InsuranceId != null);
            }
            else if (filter.HasInsurance == false)
            {
                query = query.Where(p => p.InsuranceId == null);
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(p => p.IsActive == filter.IsActive.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<PatientListDto>
            {
                Items = _mapper.Map<IEnumerable<PatientListDto>>(items),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task AddAsync(CreatePatientDto dto)
        {
            var patient = _mapper.Map<Patient>(dto);

            await _repository.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdatePatientDto dto)
        {
            var patient = await _repository.GetByIdAsync(id);

            if (patient is null)
                throw new InvalidOperationException(NotFoundExceptionMessage);

            _mapper.Map(dto, patient);

            await _repository.UpdateAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int id, bool isActive)
        {
            var patient = await _repository.GetByIdAsync(id);

            if (patient is null)
                throw new InvalidOperationException(NotFoundExceptionMessage);

            patient.IsActive = isActive;

            await _repository.UpdateAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);

            if (patient is null)
                throw new InvalidOperationException(NotFoundExceptionMessage);

            try
            {
                await _repository.DeleteAsync(id);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(
                    "Failed to delete patient. It may be referenced by existing appointments or health records.",
                    ex);
            }
        }

        public async Task<PatientSummaryDto> GetSummaryAsync()
        {
            return await _patientRepository.GetSummaryAsync();
        }
    }
}