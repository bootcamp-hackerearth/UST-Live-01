using AutoMapper;
using HealthApp.Api.Dto;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;
using HealthApp.Api.Exceptions;

namespace HealthApp.Api.Service.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repo;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<DoctorDto> AddDoctorAsync(DoctorDto dto)
        {
            ValidateDoctorDto(dto);

            var doctors = await _repo.getallAsync();

            bool exists = doctors != null &&
                          doctors.Any(d =>
                              !string.IsNullOrWhiteSpace(d.Email) &&
                              d.Email.ToLower() == dto.Email.ToLower());

            if (exists)
            {
                throw new ConflictException("A doctor with the same email already exists.");
            }

            var doctor = _mapper.Map<Doctor>(dto);

            var savedDoctor = await _repo.addAsync(doctor);

            if (savedDoctor == null)
            {
                throw new BusinessRuleException("Unable to create doctor.");
            }

            return _mapper.Map<DoctorDto>(savedDoctor);
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await _repo.getallAsync();

            return _mapper.Map<List<DoctorDto>>(doctors ?? new List<Doctor>());
        }

        public async Task<List<DoctorDto>> GetAllActiveDoctorAsync()
        {
            var doctors = await _repo.getAllActiveAsync();

            return _mapper.Map<List<DoctorDto>>(doctors ?? new List<Doctor>());
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(int id)
        {
            ValidateDoctorId(id);

            var doctor = await _repo.getbyidAsync(id);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", id);
            }

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<List<DoctorDto>> SearchBySpecialisationAsync(string specialisation)
        {
            if (string.IsNullOrWhiteSpace(specialisation))
            {
                throw new BusinessRuleException("Specialisation is required.");
            }

            var doctors = await _repo.searchbyspecialisationAsync(specialisation);

            return _mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> UpdateDoctorByIdAsync(int id, DoctorDto doctorDto)
        {
            ValidateDoctorId(id);
            ValidateDoctorDto(doctorDto);

            var existingDoctor = await _repo.getbyidAsync(id);

            if (existingDoctor == null)
            {
                throw new EntityNotFoundException("Doctor", id);
            }

            var doctors = await _repo.getallAsync();

            bool duplicate = doctors != null &&
                             doctors.Any(d =>
                                 d.DoctorId != id &&
                                 !string.IsNullOrWhiteSpace(d.Email) &&
                                 d.Email.ToLower() == doctorDto.Email.ToLower());

            if (duplicate)
            {
                throw new ConflictException("Another doctor with the same email already exists.");
            }

            var doctor = _mapper.Map<Doctor>(doctorDto);
            doctor.DoctorId = id;

            var updatedDoctor = await _repo.updateAsync(id, doctor);

            if (updatedDoctor == null)
            {
                throw new EntityNotFoundException("Doctor", id);
            }

            return _mapper.Map<DoctorDto>(updatedDoctor);
        }

        // Validation methods

        private void ValidateDoctorId(int id)
        {
            if (id <= 0)
            {
                throw new BusinessRuleException("Invalid doctor id.");
            }
        }

        private void ValidateDoctorDto(DoctorDto dto)
        {
            if (dto == null)
            {
                throw new BusinessRuleException("Doctor data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                throw new BusinessRuleException("Doctor name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new BusinessRuleException("Doctor email is required.");
            }
        }
    }
}