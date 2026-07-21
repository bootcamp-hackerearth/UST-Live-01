using System.Security.Cryptography;

using AutoMapper;

using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Doctor;
using HealthAxis.Shared.Enums;

using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HealthAxisCore_Api.Services.Implementations
{
    public sealed class DoctorService : IDoctorService
    {
        private const string AvailableDoctorsCacheKey =
            "available-doctors";

        private const string DoctorRole = "Doctor";

        private const string AllFilterValue = "All";

        private static readonly TimeSpan AvailableDoctorsCacheDuration =
            TimeSpan.FromMinutes(5);

        private static readonly Action<ILogger, string, Exception?>
            CacheHitLog =
                LoggerMessage.Define<string>(
                    LogLevel.Information,
                    new EventId(1001, nameof(CacheHitLog)),
                    "CACHE HIT - Key: {CacheKey}");

        private static readonly Action<ILogger, string, Exception?>
            CacheMissLog =
                LoggerMessage.Define<string>(
                    LogLevel.Information,
                    new EventId(1002, nameof(CacheMissLog)),
                    "CACHE MISS - Key: {CacheKey}. " +
                    "Loading available doctors from database.");

        private static readonly Action<ILogger, string, int, Exception?>
            CacheSetLog =
                LoggerMessage.Define<string, int>(
                    LogLevel.Information,
                    new EventId(1003, nameof(CacheSetLog)),
                    "CACHE SET - Key: {CacheKey}. " +
                    "Cached for {DurationMinutes} minutes.");

        private static readonly Action<ILogger, string, string, Exception?>
            CacheInvalidatedLog =
                LoggerMessage.Define<string, string>(
                    LogLevel.Information,
                    new EventId(1004, nameof(CacheInvalidatedLog)),
                    "CACHE INVALIDATED - Key: {CacheKey}. " +
                    "Reason: {Reason}");

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

        public async Task<PagedResponseDto<DoctorResponseDto>>
            GetPagedAsync(
                int pageNumber,
                int pageSize,
                string? search,
                string? specialisation,
                string? status)
        {
            pageNumber = NormalizePageNumber(pageNumber);
            pageSize = NormalizePageSize(pageSize);

            var doctors = await _repository.GetAllAsync();

            var query = doctors.AsEnumerable();

            query = ApplySearchFilter(
                query,
                search);

            query = ApplySpecialisationFilter(
                query,
                specialisation);

            query = ApplyStatusFilter(
                query,
                status);

            var totalCount = query.Count();

            var totalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize);

            var pagedDoctors = query
                .OrderBy(doctor => doctor.DoctorName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var doctorDtos =
                _mapper.Map<List<DoctorResponseDto>>(
                    pagedDoctors);

            return new PagedResponseDto<DoctorResponseDto>
            {
                Items = doctorDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        public async Task<IEnumerable<DoctorResponseDto>>
            GetAllAsync()
        {
            var doctors =
                await _repository.GetAllAsync();

            return _mapper.Map<
                IEnumerable<DoctorResponseDto>>(
                    doctors);
        }

        public async Task<DoctorResponseDto?>
            GetByIdAsync(int id)
        {
            var doctor =
                await _repository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new EntityNotFoundException(
                    "Doctor not found");
            }

            return _mapper.Map<DoctorResponseDto>(
                doctor);
        }

        public async Task<CreateDoctorResultDto>
            CreateAsync(CreateDoctorDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var doctor =
                _mapper.Map<Doctor>(dto);

            doctor.CreatedDate = DateTime.Now;
            doctor.IsOnLeave = false;

            await _repository.AddAsync(doctor);

            var temporaryPassword =
                GenerateTemporaryPassword();

            var user = CreateDoctorUser(
                doctor,
                temporaryPassword);

            var createUserResult =
                await _userManager.CreateAsync(
                    user,
                    temporaryPassword);

            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    createUserResult.Errors.Select(
                        error => error.Description));

                throw new BusinessRuleException(errors);
            }

            var addRoleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    DoctorRole);

            if (!addRoleResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    addRoleResult.Errors.Select(
                        error => error.Description));

                throw new BusinessRuleException(errors);
            }

            await InvalidateAvailableDoctorsCacheAsync(
                "Doctor created");

            return CreateDoctorResult(
                doctor,
                temporaryPassword);
        }

        public async Task<bool> UpdateAsync(
            int id,
            CreateDoctorDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var doctor =
                await _repository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new EntityNotFoundException(
                    "Doctor not found");
            }

            _mapper.Map(dto, doctor);

            await _repository.UpdateAsync(doctor);

            await InvalidateAvailableDoctorsCacheAsync(
                "Doctor updated");

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exists =
                await _repository.Exists(id);

            if (!exists)
            {
                throw new EntityNotFoundException(
                    "Doctor not found");
            }

            await _repository.DeleteAsync(id);

            await InvalidateAvailableDoctorsCacheAsync(
                "Doctor deleted");

            return true;
        }

        public async Task<IEnumerable<DoctorResponseDto>>
            FilterAsync(
                string? name,
                SpecialisationType? specialization,
                bool? isActive)
        {
            if (IsAvailableDoctorsRequest(
                    name,
                    specialization,
                    isActive))
            {
                return await GetAvailableDoctorsAsync(
                    name,
                    specialization,
                    isActive);
            }

            var filteredDoctors =
                await _repository.GetDoctors(
                    name,
                    specialization,
                    isActive);

            return _mapper.Map<
                IEnumerable<DoctorResponseDto>>(
                    filteredDoctors);
        }

        public async Task<bool> SetStatusAsync(
            int doctorId,
            bool status)
        {
            var doctor =
                await _repository.GetByIdAsync(
                    doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException(
                    "Doctor not found");
            }

            await _repository.SetStatus(
                doctorId,
                status);

            await InvalidateAvailableDoctorsCacheAsync(
                "Doctor status changed");

            return true;
        }

        private async Task<IEnumerable<DoctorResponseDto>>
            GetAvailableDoctorsAsync(
                string? name,
                SpecialisationType? specialization,
                bool? isActive)
        {
            var cachedDoctors =
                await _cacheService.GetAsync<
                    List<DoctorResponseDto>>(
                        AvailableDoctorsCacheKey);

            if (cachedDoctors != null)
            {
                CacheHitLog(
                    _logger,
                    AvailableDoctorsCacheKey,
                    null);

                return cachedDoctors;
            }

            CacheMissLog(
                _logger,
                AvailableDoctorsCacheKey,
                null);

            var doctors =
                await _repository.GetDoctors(
                    name,
                    specialization,
                    isActive);

            var availableDoctors = doctors
                .Where(doctor =>
                    doctor.IsActive &&
                    !doctor.IsOnLeave)
                .ToList();

            var doctorDtos =
                _mapper.Map<List<DoctorResponseDto>>(
                    availableDoctors);

            await _cacheService.SetAsync(
                AvailableDoctorsCacheKey,
                doctorDtos,
                AvailableDoctorsCacheDuration);

            CacheSetLog(
                _logger,
                AvailableDoctorsCacheKey,
                (int)AvailableDoctorsCacheDuration.TotalMinutes,
                null);

            return doctorDtos;
        }

        private async Task
            InvalidateAvailableDoctorsCacheAsync(
                string reason)
        {
            await _cacheService.RemoveAsync(
                AvailableDoctorsCacheKey);

            CacheInvalidatedLog(
                _logger,
                AvailableDoctorsCacheKey,
                reason,
                null);
        }

        private static bool IsAvailableDoctorsRequest(
            string? name,
            SpecialisationType? specialization,
            bool? isActive)
        {
            return (
                name == null &&
                specialization == null &&
                isActive == true
            );
        }

        private static int NormalizePageNumber(
            int pageNumber)
        {
            return pageNumber < 1
                ? 1
                : pageNumber;
        }

        private static int NormalizePageSize(
            int pageSize)
        {
            if (pageSize < 1)
            {
                return 10;
            }

            return pageSize > 100
                ? 100
                : pageSize;
        }

        private static IEnumerable<Doctor>
            ApplySearchFilter(
                IEnumerable<Doctor> query,
                string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return query;
            }

            return query.Where(doctor =>
                DoctorMatchesSearch(
                    doctor,
                    search));
        }

        private static bool DoctorMatchesSearch(
            Doctor doctor,
            string search)
        {
            var nameMatches =
                !string.IsNullOrWhiteSpace(
                    doctor.DoctorName) &&
                doctor.DoctorName.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase);

            var emailMatches =
                !string.IsNullOrWhiteSpace(
                    doctor.Email) &&
                doctor.Email.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase);

            var specialisationMatches =
                doctor.Specialisation
                    .ToString()
                    .Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase);

            return (
                nameMatches ||
                emailMatches ||
                specialisationMatches
            );
        }

        private static IEnumerable<Doctor>
            ApplySpecialisationFilter(
                IEnumerable<Doctor> query,
                string? specialisation)
        {
            if (
                string.IsNullOrWhiteSpace(
                    specialisation) ||
                specialisation.Equals(
                    AllFilterValue,
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                return query;
            }

            return query.Where(doctor =>
                doctor.Specialisation
                    .ToString()
                    .Equals(
                        specialisation,
                        StringComparison.OrdinalIgnoreCase));
        }

        private static IEnumerable<Doctor>
            ApplyStatusFilter(
                IEnumerable<Doctor> query,
                string? status)
        {
            if (
                string.IsNullOrWhiteSpace(status) ||
                status.Equals(
                    AllFilterValue,
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                return query;
            }

            if (status.Equals(
                "Active",
                StringComparison.OrdinalIgnoreCase))
            {
                return query.Where(doctor =>
                    doctor.IsActive &&
                    !doctor.IsOnLeave);
            }

            if (status.Equals(
                "Inactive",
                StringComparison.OrdinalIgnoreCase))
            {
                return query.Where(doctor =>
                    !doctor.IsActive);
            }

            if (IsOnLeaveStatus(status))
            {
                return query.Where(doctor =>
                    doctor.IsOnLeave);
            }

            return query;
        }

        private static bool IsOnLeaveStatus(
            string status)
        {
            return (
                status.Equals(
                    "OnLeave",
                    StringComparison.OrdinalIgnoreCase) ||
                status.Equals(
                    "On Leave",
                    StringComparison.OrdinalIgnoreCase)
            );
        }

        private static string
            GenerateTemporaryPassword()
        {
            var randomNumber =
                RandomNumberGenerator.GetInt32(
                    1000,
                    10000);

            return $"Temp@{randomNumber}";
        }

        private static ApplicationUser
            CreateDoctorUser(
                Doctor doctor,
                string temporaryPassword)
        {
            return new ApplicationUser
            {
                UserName = doctor.Email,
                Email = doctor.Email,
                Role = DoctorRole,
                ReferenceId = doctor.DoctorId,
                IsFirstLogin = true,
                TemporaryPassword = temporaryPassword
            };
        }

        private static CreateDoctorResultDto
            CreateDoctorResult(
                Doctor doctor,
                string temporaryPassword)
        {
            return new CreateDoctorResultDto
            {
                DoctorId = doctor.DoctorId,
                DoctorName = doctor.DoctorName,
                Specialisation = doctor.Specialisation,
                YearsOfExperience =
                    doctor.YearsOfExperience,
                ConsultationFee =
                    doctor.ConsultationFee,
                IsActive = doctor.IsActive,
                Email = doctor.Email,
                TemporaryPassword =
                    temporaryPassword
            };
        }
    }
}
