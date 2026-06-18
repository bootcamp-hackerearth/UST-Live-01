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

        // ✅ Get All
        public async Task<IEnumerable<DoctorDto>> GetAllAsync(CancellationToken ct)
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        // ✅ Get By Id
        public async Task<DoctorDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
                throw new NotFoundException("Doctor not found");

            return _mapper.Map<DoctorDto>(doctor);
        }

        // ✅ Get Available Doctors
        public async Task<IEnumerable<DoctorDto>> GetAvailableDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAvailableDoctorsAsync();
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        // ✅ Add Doctor
        public async Task<DoctorDto> AddAsync(CreateDoctorDto dto)
        {
            var doctor = _mapper.Map<Doctor>(dto);

            await _doctorRepository.AddAsync(doctor);

            return _mapper.Map<DoctorDto>(doctor);
        }

        // ✅ Update Doctor
        public async Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto dto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
                throw new NotFoundException("Doctor not found");

            _mapper.Map(dto, doctor);

            await _doctorRepository.UpdateAsync(id, doctor, CancellationToken.None);

            return _mapper.Map<DoctorDto>(doctor);
        }

        // ✅ NEW — Search by Name
        public async Task<IEnumerable<DoctorDto>> SearchByNameAsync(string name)
        {
            var doctors = await _doctorRepository.SearchByNameAsync(name);
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        // ✅ NEW — Filter by Specialisation
        public async Task<IEnumerable<DoctorDto>> GetBySpecialisationAsync(string specialization)
        {
            var doctors = await _doctorRepository.GetBySpecialisationAsync(specialization);
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }
    }
}