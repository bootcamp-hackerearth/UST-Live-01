using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;
using HealthApp.Shared.Dto;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HealthApp.Api.Service.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repo;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public DoctorService(IDoctorRepository repo, IMapper mapper,IDistributedCache cache)
        {
            _repo = repo;
            _mapper = mapper;
            _cache = cache;
        }

        private const string patientCacheKeyPrefix = "patient:all";

        private  readonly DistributedCacheEntryOptions cacheOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
        };







        public async Task<DoctorDto> AddDoctorAsync(DoctorDto dto)
        {
            ValidateDoctorDto(dto);

            var doctors = await _repo.getallAsync();

            bool exists = doctors != null &&
               doctors.Any(d =>
                   !string.IsNullOrWhiteSpace(d.Email) &&
                   string.Equals(
                       d.Email,
                       dto.Email,
                       StringComparison.OrdinalIgnoreCase));

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
                     string.Equals(
                         d.Email,
                         doctorDto.Email,
                         StringComparison.OrdinalIgnoreCase));

            if (duplicate)
            {
                throw new ConflictException("Another doctor with the same email already exists.");
            }

            existingDoctor.FullName = doctorDto.FullName!;
            existingDoctor.Specialisation = doctorDto.Specialisation!;
            existingDoctor.Email = doctorDto.Email!;
            existingDoctor.PracticeStartDate = doctorDto.PracticeStartDate!.Value.Date;
            existingDoctor.ConsultationFee = doctorDto.ConsultationFee ?? 0;
            existingDoctor.DoctorPhoneNumber = doctorDto.DoctorPhoneNumber;
            existingDoctor.IsActive = doctorDto.IsActive ?? false;

            var updatedDoctor = await _repo.updateAsync(id, existingDoctor);

            if (updatedDoctor == null)
            {
                throw new EntityNotFoundException("Doctor", id);
            }

            return _mapper.Map<DoctorDto>(updatedDoctor);
        }

        private static void ValidateDoctorId(int id)
        {
            if (id <= 0)
            {
                throw new BusinessRuleException("Invalid doctor id.");
            }
        }

        private static void ValidateDoctorDto(DoctorDto dto)
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

            if (string.IsNullOrWhiteSpace(dto.Specialisation))
            {
                throw new BusinessRuleException("Specialisation is required.");
            }

            if (!dto.PracticeStartDate.HasValue)
            {
                throw new BusinessRuleException("Practice start date is required.");
            }

            if (!dto.ConsultationFee.HasValue)
            {
                throw new BusinessRuleException("Consultation fee is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.DoctorPhoneNumber))
            {
                throw new BusinessRuleException("Doctor phone number is required.");
            }
        }

        public async Task<DoctorDto> GetMyProfileAsync(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid user identity.");
            }

            var doctor = await _repo.GetByIdentityUserIdAsync(identityUserId);

            if (doctor == null)
            {
                throw new BusinessRuleException("Doctor profile not found.");
            }

            return _mapper.Map<DoctorDto>(doctor);
        }




        
        public async Task<(List<DoctorDto> Items, int TotalCount)> GetPagedDoctorsAsync(int pageNumber,int pageSize)
        {
            if (pageNumber <= 0)
                throw new BusinessRuleException("Page number must be greater than 0.");

            if (pageSize <= 0)
                throw new BusinessRuleException("Page size must be greater than 0.");

            var (doctors, totalCount) = await _repo.GetPagedAsync(pageNumber, pageSize);

            var result = _mapper.Map<List<DoctorDto>>(doctors);

            return (result, totalCount);
        }

        public async Task<(List<DoctorDto> Items, int TotalCount)> GetPagedActiveDoctorsAsync(int pageNumber,int pageSize)
        {
            var cached = await _cache.GetStringAsync(patientCacheKeyPrefix);
            if (!string.IsNullOrWhiteSpace(cached))
            {
                var doctor= JsonSerializer.Deserialize<List<DoctorDto>>(cached);
                if (doctor != null) return (doctor, doctor.Count);
            }


            if (pageNumber <= 0)
                throw new BusinessRuleException("Page number must be greater than 0.");

            if (pageSize <= 0)
                throw new BusinessRuleException("Page size must be greater than 0.");

            var (doctors, totalCount) = await _repo.GetActivePagedAsync(pageNumber, pageSize);

            var result = _mapper.Map<List<DoctorDto>>(doctors);

            await _cache.SetStringAsync(patientCacheKeyPrefix, JsonSerializer.Serialize(doctors), cacheOptions);


            return (result, totalCount);
        }

        public async Task<(List<DoctorDto> Items, int TotalCount)> SearchBySpecialisationPagedAsync
            (string specialisation,int pageNumber,int pageSize)
        {
            if (string.IsNullOrWhiteSpace(specialisation))
                throw new BusinessRuleException("Specialisation is required.");

            if (pageNumber <= 0)
                throw new BusinessRuleException("Page number must be greater than 0.");

            if (pageSize <= 0)
                throw new BusinessRuleException("Page size must be greater than 0.");

            var (doctors, totalCount) =
                await _repo.SearchBySpecialisationPagedAsync(specialisation, pageNumber, pageSize);

            var result = _mapper.Map<List<DoctorDto>>(doctors);

            return (result, totalCount);
        }

    }
}