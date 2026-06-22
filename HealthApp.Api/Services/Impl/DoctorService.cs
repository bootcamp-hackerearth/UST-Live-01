using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;

namespace HealthApp.Api.Services.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new InvalidRequestException("Valid doctor id is required.");
            }

            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", id);
            }

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task AddDoctorAsync(DoctorCreateDto dto)
        {
            if (dto == null)
            {
                throw new InvalidRequestException("Doctor data is required.");
            }

            ValidateDoctor(dto);

            string email = dto.DoctorEmail.Trim();

            var allDoctors = await _doctorRepository.SearchDoctorsAsync(
                null,
                null,
                null);

            bool emailExists = allDoctors.Any(d =>
                string.Equals(
                    d.DoctorEmail,
                    email,
                    StringComparison.OrdinalIgnoreCase));

            if (emailExists)
            {
                throw new DuplicateEntityException(
                    "A doctor with this email already exists.");
            }

            var doctor = _mapper.Map<Doctor>(dto);

            doctor.FullName = dto.FullName.Trim();
            doctor.DoctorPhoneNo = dto.DoctorPhoneNo.Trim();
            doctor.DoctorEmail = email;
            doctor.Specialisation = (SpecialisationType)dto.Specialisation!;
            doctor.YearsOfExperience = dto.YearsOfExperience;
            doctor.ConsultationFee = dto.ConsultationFee;
            doctor.IsActive = true;

            await _doctorRepository.Add(doctor);
        }

        public async Task UpdateDoctorAsync(int id, DoctorCreateDto dto)
        {
            if (id <= 0)
            {
                throw new InvalidRequestException("Valid doctor id is required.");
            }

            if (dto == null)
            {
                throw new InvalidRequestException("Doctor data is required.");
            }

            ValidateDoctor(dto);

            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", id);
            }

            string email = dto.DoctorEmail.Trim();

            var allDoctors = await _doctorRepository.SearchDoctorsAsync(
                null,
                null,
                null);

            bool emailUsedByAnotherDoctor = allDoctors.Any(d =>
                d.DoctorId != id &&
                string.Equals(
                    d.DoctorEmail,
                    email,
                    StringComparison.OrdinalIgnoreCase));

            if (emailUsedByAnotherDoctor)
            {
                throw new DuplicateEntityException(
                    "Another doctor already uses this email.");
            }

            doctor.FullName = dto.FullName.Trim();
            doctor.Specialisation = (SpecialisationType)dto.Specialisation!;
            doctor.DoctorPhoneNo = dto.DoctorPhoneNo.Trim();
            doctor.DoctorEmail = email;
            doctor.YearsOfExperience = dto.YearsOfExperience;
            doctor.ConsultationFee = dto.ConsultationFee;

            await _doctorRepository.Update(id, doctor);
        }

        public async Task ChangeStatusAsync(int id, bool isActive)
        {
            if (id <= 0)
            {
                throw new InvalidRequestException("Valid doctor id is required.");
            }

            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", id);
            }

            bool updated = await _doctorRepository.ChangeStatusAsync(
                id,
                isActive);

            if (!updated)
            {
                throw new BusinessRuleViolationException(
                    "Unable to update doctor status.");
            }
        }

        public async Task<IEnumerable<DoctorDto>> GetDoctorsBySpecialisationAsync(
            SpecialisationType specialisation)
        {
            var doctors = await _doctorRepository.SearchDoctorsAsync(
                null,
                specialisation,
                true);

            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        public async Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(
            string? search,
            SpecialisationType? specialisation,
            bool? isActive)
        {
            var doctors = await _doctorRepository.SearchDoctorsAsync(
                search,
                specialisation,
                isActive);

            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        private static void ValidateDoctor(DoctorCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                throw new InvalidRequestException("Doctor name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.DoctorPhoneNo))
            {
                throw new InvalidRequestException("Doctor phone number is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.DoctorEmail))
            {
                throw new InvalidRequestException("Doctor email is required.");
            }

            if (dto.YearsOfExperience < 0 || dto.YearsOfExperience > 60)
            {
                throw new InvalidRequestException(
                    "Years of experience must be between 0 and 60.");
            }

            if (dto.ConsultationFee < 0 || dto.ConsultationFee > 100000)
            {
                throw new InvalidRequestException(
                    "Consultation fee must be between 0 and 100000.");
            }
        }
    }
}