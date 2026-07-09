using AutoMapper;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Doctor;
using HealthAxis.Shared.Enums;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HealthAxisCore_Api.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DoctorService> _logger;

        public DoctorService(
            IDoctorRepository repository,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            ICacheService cacheService,
            ILogger<DoctorService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<PagedResponseDto<DoctorResponseDto>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            string? specialisation,
            string? status)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageSize > 100)
            {
                pageSize = 100;
            }

            var doctors = await _repository.GetAllAsync();

            var query = doctors.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d =>
                    (!string.IsNullOrWhiteSpace(d.DoctorName) &&
                     d.DoctorName.Contains(search, StringComparison.OrdinalIgnoreCase)) ||

                    (!string.IsNullOrWhiteSpace(d.Email) &&
                     d.Email.Contains(search, StringComparison.OrdinalIgnoreCase)) ||

                    d.Specialisation.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(specialisation) && specialisation != "All")
            {
                query = query.Where(d =>
                    d.Specialisation.ToString().Equals(
                        specialisation,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
                if (status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(d => d.IsActive);
                }
                else if (status.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(d => !d.IsActive);
                }
            }

            var totalCount = query.Count();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedDoctors = query
                .OrderBy(d => d.DoctorName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var doctorDtos = _mapper.Map<List<DoctorResponseDto>>(pagedDoctors);

            return new PagedResponseDto<DoctorResponseDto>
            {
                Items = doctorDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        public async Task<IEnumerable<DoctorResponseDto>> GetAllAsync()
        {
            var doctors = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<DoctorResponseDto>>(doctors);
        }

        public async Task<DoctorResponseDto?> GetByIdAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor not found");
            }

            return _mapper.Map<DoctorResponseDto>(doctor);
        }

        public async Task<CreateDoctorResultDto> CreateAsync(CreateDoctorDto dto)
        {
            var doctor = _mapper.Map<Doctor>(dto);
            doctor.CreatedDate = DateTime.Now;

            await _repository.AddAsync(doctor);

            var tempPassword = "Temp@" + new Random().Next(1000, 9999);

            var user = new ApplicationUser
            {
                UserName = doctor.Email,
                Email = doctor.Email,
                Role = "Doctor",
                ReferenceId = doctor.DoctorId,
                IsFirstLogin = true,
                TemporaryPassword = tempPassword
            };

            var result = await _userManager.CreateAsync(user, tempPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                throw new BusinessRuleException(errors);
            }

            await _userManager.AddToRoleAsync(user, "Doctor");

            await _cacheService.RemoveAsync("available-doctors");

            _logger.LogInformation(
                "CACHE INVALIDATED - Key: available-doctors. Doctor created.");

            return new CreateDoctorResultDto
            {
                DoctorId = doctor.DoctorId,
                DoctorName = doctor.DoctorName,
                Specialisation = doctor.Specialisation,
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = doctor.IsActive,
                Email = doctor.Email,
                TemporaryPassword = tempPassword
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateDoctorDto dto)
        {
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor not found");
            }

            _mapper.Map(dto, doctor);

            await _repository.UpdateAsync(doctor);

            await _cacheService.RemoveAsync("available-doctors");

            _logger.LogInformation(
                "CACHE INVALIDATED - Key: available-doctors. Doctor updated.");

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _repository.Exists(id);

            if (!exists)
            {
                throw new EntityNotFoundException("Doctor not found");
            }

            await _repository.DeleteAsync(id);

            await _cacheService.RemoveAsync("available-doctors");

            _logger.LogInformation(
                "CACHE INVALIDATED - Key: available-doctors. Doctor deleted.");

            return true;
        }

        public async Task<IEnumerable<DoctorResponseDto>> FilterAsync(
            string? name,
            SpecialisationType? specialization,
            bool? isActive)
        {
            if (name == null &&
                specialization == null &&
                isActive == true)
            {
                const string cacheKey = "available-doctors";

                var cachedDoctors =
                    await _cacheService.GetAsync<List<DoctorResponseDto>>(cacheKey);

                if (cachedDoctors != null)
                {
                    _logger.LogInformation(
                        "CACHE HIT - Key: {CacheKey}",
                        cacheKey);

                    return cachedDoctors;
                }

                _logger.LogInformation(
                    "CACHE MISS - Key: {CacheKey}. Loading available doctors from database.",
                    cacheKey);

                var doctors =
                    await _repository.GetDoctors(name, specialization, isActive);

                var doctorDtos =
                    _mapper.Map<List<DoctorResponseDto>>(doctors);

                await _cacheService.SetAsync(
                    cacheKey,
                    doctorDtos,
                    TimeSpan.FromMinutes(5));

                _logger.LogInformation(
                    "CACHE SET - Key: {CacheKey}. Cached for 5 minutes.",
                    cacheKey);

                return doctorDtos;
            }

            var filteredDoctors =
                await _repository.GetDoctors(name, specialization, isActive);

            return _mapper.Map<IEnumerable<DoctorResponseDto>>(filteredDoctors);
        }

        public async Task<bool> SetStatusAsync(int doctorId, bool status)
        {
            var doctor = await _repository.GetByIdAsync(doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor not found");
            }

            await _repository.SetStatus(doctorId, status);

            await _cacheService.RemoveAsync("available-doctors");

            _logger.LogInformation(
                "CACHE INVALIDATED - Key: available-doctors. Doctor status changed.");

            return true;
        }
    }
}