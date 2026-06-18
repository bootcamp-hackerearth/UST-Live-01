using AutoMapper;
using HealthCareApp.Dtos;
using HealthCareApp.Enums;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;

namespace HealthCareApp.Services
{
    public class DoctorService(IDoctorRepository repository, IMapper mapper) : IDoctorService
    {
        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await repository.GetAllAsync();

            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<List<DoctorDto>> GetAllActiveDoctorsAsync()
        {
            var doctors = await repository.GetAllActiveAsync();

            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(int doctorId)
        {
            ValidateDoctorId(doctorId);

            var doctor = await repository.GetByIdAsync(doctorId);

            if (doctor is null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return mapper.Map<DoctorDto>(doctor);
        }

        public async Task<List<DoctorDto>> GetDoctorsBySpecialisationAsync(SpecialisationType specialisation)
        {
            var doctors = await repository.GetBySpecialisationAsync(specialisation);

            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<List<DoctorDto>> GetActiveDoctorsBySpecialisationAsync(SpecialisationType specialisation)
        {
            var doctors = await repository.GetActiveBySpecialisationAsync(specialisation);

            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto)
        {
            ValidateCreateDoctorDto(dto);

            var doctor = mapper.Map<Doctor>(dto);

            doctor.IsActive = true;

            doctor.YearsOfExperience = DateTime.Today.Year - dto.PracticeStartDate.Year;

            if (dto.PracticeStartDate.Date > DateTime.Today.AddYears(-doctor.YearsOfExperience))
            {
                doctor.YearsOfExperience--;
            }

            var savedDoctor = await repository.CreateAsync(doctor);

            return mapper.Map<DoctorDto>(savedDoctor);
        }

        public async Task<DoctorDto> UpdateDoctorAsync(int doctorId, UpdateDoctorDto dto)
        {
            ValidateDoctorId(doctorId);

            ValidateUpdateDoctorDto(dto);

            var existingDoctor = await repository.GetByIdAsync(doctorId);

            if (existingDoctor is null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            var doctor = mapper.Map<Doctor>(dto);

            doctor.DoctorId = doctorId;

            doctor.YearsOfExperience = DateTime.Today.Year - dto.PracticeStartDate.Year;

            if (dto.PracticeStartDate.Date > DateTime.Today.AddYears(-doctor.YearsOfExperience))
            {
                doctor.YearsOfExperience--;
            }

            doctor.CreatedDate = existingDoctor.CreatedDate;

            var updatedDoctor = await repository.UpdateAsync(doctorId, doctor);

            if (updatedDoctor is null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return mapper.Map<DoctorDto>(updatedDoctor);
        }
        public async Task<DoctorDto> DeleteDoctorAsync(int doctorId)
        {
            ValidateDoctorId(doctorId);

            var deletedDoctor = await repository.DeleteAsync(doctorId);

            if (deletedDoctor is null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return mapper.Map<DoctorDto>(deletedDoctor);
        }

        private void ValidateDoctorId(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new BusinessRuleException("Please provide a valid doctor reference.");
            }
        }

        private void ValidateCreateDoctorDto(CreateDoctorDto dto)
        {
            if (dto is null)
            {
                throw new BusinessRuleException("Doctor details are required.");
            }

            ValidateDoctorCommonFields(
                dto.FullName,
                dto.PracticeStartDate,
                dto.ConsultationFee
            );
        }

        private void ValidateUpdateDoctorDto(UpdateDoctorDto dto)
        {
            if (dto is null)
            {
                throw new BusinessRuleException("Doctor details are required.");
            }

            ValidateDoctorCommonFields(
                dto.FullName,
                dto.PracticeStartDate,
                dto.ConsultationFee
            );
        }

        private void ValidateDoctorCommonFields(
            string fullName,
            DateTime practiceStartDate,
            decimal consultationFee)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new BusinessRuleException("Doctor full name is required.");
            }

            if (practiceStartDate.Date > DateTime.Today)
            {
                throw new BusinessRuleException("Practice start date cannot be in the future.");
            }

            if (consultationFee < 0)
            {
                throw new BusinessRuleException("Consultation fee cannot be negative.");
            }
        }

        public async Task<DoctorDto> SubmitDoctorRegistrationRequestAsync(DoctorRegistrationRequestDto dto)
        {
            if (dto is null)
            {
                throw new BusinessRuleException("Doctor registration details are required.");
            }

            if (string.IsNullOrWhiteSpace(dto.DoctorName))
            {
                throw new BusinessRuleException("Doctor name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new BusinessRuleException("Doctor email is required.");
            }

            if (dto.PracticeStartDate.Date > DateTime.Today)
            {
                throw new BusinessRuleException("Practice start date cannot be in the future.");
            }

            if (dto.ConsultationFee < 0)
            {
                throw new BusinessRuleException("Consultation fee cannot be negative.");
            }

            var emailExists = await repository.ExistsByEmailAsync(dto.Email.Trim().ToLower());

            if (emailExists)
            {
                throw new ConflictException("A doctor registration request with this email already exists.");
            }

            var doctor = mapper.Map<Doctor>(dto);

            doctor.Email = dto.Email.Trim().ToLower();
            doctor.DoctorName = dto.DoctorName.Trim();
            doctor.YearsOfExperience = CalculateYearsOfExperience(dto.PracticeStartDate);
            doctor.IsActive = false;
            doctor.VerificationStatus = DoctorVerificationStatus.Pending;
            doctor.IdentityUserId = null;
            doctor.CreatedDate = DateTime.Now;

            var savedDoctor = await repository.CreateAsync(doctor);

            return mapper.Map<DoctorDto>(savedDoctor);
        }
        public async Task<List<DoctorDto>> GetPendingDoctorsAsync()
        {
            var doctors = await repository.GetPendingDoctorsAsync();

            return mapper.Map<List<DoctorDto>>(doctors);
        }
        private int CalculateYearsOfExperience(DateTime practiceStartDate)
        {
            int years = DateTime.Today.Year - practiceStartDate.Year;

            if (practiceStartDate.Date > DateTime.Today.AddYears(-years))
            {
                years--;
            }

            return years;
        }

    }
}