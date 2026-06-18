using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.DTOs.Patient;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HealthCare.Api.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _repository;
        private readonly HealthCareDbContext _context;
        private readonly IMapper _mapper;

        public PatientService(IRepository<Patient> repository, HealthCareDbContext context, IMapper mapper)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }

        public async Task AddAsync(CreatePatientDto dto)
        {
            var patient=_mapper.Map<Patient>(dto);
            await _repository.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdatePatientDto dto)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null)
                throw new PatientNotFoundException(id);
            _mapper.Map(dto,patient);

            await _repository.UpdateAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _context.SaveChangesAsync();
        }

        public async Task<PatientListDto>GetByIdAsync(int id)
        {
            var patient= await _repository.GetByIdAsync(id);
            return patient == null ? null : _mapper.Map<PatientListDto>(patient);
        }

        public async Task <IEnumerable<PatientListDto>>GetAllAsync()
        {
            var patients = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientListDto>>(patients);

        }
    }
}
