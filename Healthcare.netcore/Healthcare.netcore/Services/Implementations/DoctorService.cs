using AutoMapper;
using HealthAxis.API.DTOs;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using CustomValidationException = HealthAxis.API.Exceptions.ValidationException;

namespace HealthAxis.API.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository repository, IMapper mapper)
        {
            _doctorRepository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllAsync(CancellationToken ct)
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
                throw new NotFoundException("Doctor not found");

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<IEnumerable<DoctorDto>> GetAvailableDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAvailableDoctorsAsync();
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }
    }
}