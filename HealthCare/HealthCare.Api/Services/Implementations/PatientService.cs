using AutoMapper;
using Healthcare.Shared.DTOs;
using Healthcare.Shared.DTOs.Patient;
using HealthCare.Api.Data;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Implementations;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HealthCare.Api.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _repository;
        private readonly IPatientRepository? _patientRepository;
        private readonly HealthCareDbContext _context;
        private readonly IMapper _mapper;

        public PatientService(IRepository<Patient> repository, HealthCareDbContext context, IMapper mapper, IPatientRepository patientRepositor)
        {
            _patientRepository = patientRepositor;
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }
        public async Task AddAsync(CreatePatientDto dto)
        {
            var patient = _mapper.Map<Patient>(dto);
            await _repository.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdatePatientDto dto)
        {
            var patient = await _repository.GetProfileAsync(id);
            if (patient == null)
                throw new PatientNotFoundException("Patient Not Found");
            _mapper.Map(dto, patient);

            await _repository.UpdateAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var patient = await _repository.GetProfileAsync(id);
            if (patient == null)
                throw new PatientNotFoundException("Patient Not Found");
            await _repository.DeleteAsync(id);
            await _context.SaveChangesAsync();
        }

        public async Task<PatientListDto?> GetByIdAsync(int id)
        {
            var patient = await _patientRepository.GetMyProfileAsync(id);

            if (patient == null)
                throw new PatientNotFoundException("Patient Not Found");

            return patient;
        }

        public async Task<IEnumerable<PatientListDto>> SearchByNameAsync(string name)
        {
            var patients = await _context.Patients
                .Where(p => p.FullName.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            return _mapper.Map<IEnumerable<PatientListDto>>(patients);
        }

        public async Task<PagedResult<PatientListDto?>> GetAllAsync(PatientFilter filter)
        {
            IQueryable<Patient> query = _context.Patients.AsQueryable();

            //  Filter by Insurance
            if (filter.HasInsurance.HasValue)
            {
                if (filter.HasInsurance.Value)
                    query = query.Where(p => p.InsuranceId != null);
                else
                    query = query.Where(p => p.InsuranceId == null);
            }

            //  Filter by Name
            if (!string.IsNullOrWhiteSpace(filter.FullName))
            {
                query = query.Where(p => p.FullName.Contains(filter.FullName));
            }

            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<PatientListDto?>
            {
                Items = _mapper.Map<IEnumerable<PatientListDto>>(items),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task UpdateStatusAsync(int id, bool isActive)
        {
            var patient = await _repository.GetProfileAsync(id);

            if (patient is null)
                throw new InvalidOperationException("Patient not found.");

            patient.IsActive = isActive;

            await _repository.UpdateAsync(patient);
            await _context.SaveChangesAsync();
        }

    }
}
