using AutoMapper;
using HealthAxis.API.Data;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Models.Auth;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Doctor;
using HealthAxis.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using ValidationException = HealthAxis.API.Exceptions.ValidationException;

namespace HealthAxis.API.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly HealthAxisDbContext? _context;
        private readonly IDoctorRepository? _doctorRepository;
        private readonly UserManager<ApplicationUser>? _userManager;
        private readonly IMapper? _mapper;
        private readonly IDistributedCache? _cache;
        private readonly ILogger<DoctorService>? _logger;

        private static readonly DistributedCacheEntryOptions AvailabilityCacheOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        private static readonly List<string> AllSlots = new()
        {
            "09:00",
            "09:30",
            "10:00",
            "10:30",
            "11:00",
            "11:30",
            "14:00",
            "14:30",
            "15:00",
            "15:30",
            "16:00",
            "16:30"
        };

        [ActivatorUtilitiesConstructor]
        public DoctorService(
            HealthAxisDbContext context,
            UserManager<ApplicationUser> userManager,
            IDistributedCache cache,
            ILogger<DoctorService> logger)
        {
            _context = context;
            _userManager = userManager;
            _cache = cache;
            _logger = logger;
        }

        public DoctorService(HealthAxisDbContext context)
        {
            _context = context;
        }

        public DoctorService(
            IDoctorRepository doctorRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllAsync()
        {
            if (_doctorRepository != null && _mapper != null)
            {
                var doctors = await _doctorRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
            }

            if (_context == null)
            {
                return Enumerable.Empty<DoctorDto>();
            }

            var dbDoctors = await _context.Doctors
                .Include(d => d.Appointments)
                .AsNoTracking()
                .ToListAsync();

            return dbDoctors.Select(MapToDto);
        }

        public async Task<PagedResponse<DoctorDto>> GetAllAsync(
            PaginationParams paginationParams,
            CancellationToken ct = default)
        {
            var pageNumber = paginationParams.PageNumber <= 0
                ? 1
                : paginationParams.PageNumber;

            var pageSize = paginationParams.PageSize <= 0
                ? 10
                : paginationParams.PageSize;

            if (_context == null)
            {
                var doctors = await GetAllAsync();

                var totalRecords = doctors.Count();

                var pagedDoctors = doctors
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return new PagedResponse<DoctorDto>
                {
                    Items = pagedDoctors,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords,
                    TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
                };
            }

            var query = _context.Doctors
                .Include(d => d.Appointments)
                .AsNoTracking()
                .AsQueryable();

            var total = await query.CountAsync(ct);

            var doctorsList = await query
                .OrderBy(d => d.DoctorId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DoctorDto
                {
                    DoctorId = d.DoctorId,
                    FullName = d.FullName,
                    Specialisation = d.Specialisation,
                    YearsOfExperience = d.YearsOfExperience,
                    ConsultationFee = d.ConsultationFee,
                    IsActive = d.IsActive,
                    UpcomingAppointmentCount = d.Appointments.Count(a =>
                        a.ScheduledDate.Date >= DateTime.Today &&
                        a.Status != AppointmentStatus.Cancelled)
                })
                .ToListAsync(ct);

            return new PagedResponse<DoctorDto>
            {
                Items = doctorsList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = total,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize)
            };
        }

        public async Task<DoctorDto?> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            if (_doctorRepository != null && _mapper != null)
            {
                var doctorFromRepo = await _doctorRepository.GetByIdAsync(id);

                if (doctorFromRepo == null)
                {
                    throw new NotFoundException("Doctor not found");
                }

                return _mapper.Map<DoctorDto>(doctorFromRepo);
            }

            if (_context == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            var doctor = await _context.Doctors
                .Include(d => d.Appointments)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DoctorId == id, ct);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            return MapToDto(doctor);
        }

        public async Task<DoctorDto?> GetByUserIdAsync(
            string userId,
            CancellationToken ct = default)
        {
            if (_context != null)
            {
                var doctor = await _context.Doctors
                    .Include(d => d.Appointments)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.UserId == userId, ct);

                if (doctor == null)
                {
                    throw new NotFoundException("Doctor profile not found.");
                }

                return MapToDto(doctor);
            }

            if (_doctorRepository != null && _mapper != null)
            {
                var doctors = await _doctorRepository.GetAllAsync();

                var doctor = doctors.FirstOrDefault(d => d.UserId == userId);

                if (doctor == null)
                {
                    throw new NotFoundException("Doctor profile not found.");
                }

                return _mapper.Map<DoctorDto>(doctor);
            }

            throw new NotFoundException("Doctor profile not found.");
        }

        public async Task<object> GetAvailabilityAsync(
            int id,
            CancellationToken ct = default)
        {
            return await GetAvailabilityAsync(id, DateTime.Today, ct);
        }

        public async Task<DoctorAvailabilityDto> GetAvailabilityAsync(
            int id,
            DateTime date,
            CancellationToken ct = default)
        {
            var availabilityDate = date.Date;
            var cacheKey = $"doctors:{id}:availability:{availabilityDate:yyyy-MM-dd}";

            if (_cache != null)
            {
                var cachedAvailability = await _cache.GetStringAsync(cacheKey, ct);

                if (!string.IsNullOrWhiteSpace(cachedAvailability))
                {
                    var cachedDto = JsonSerializer.Deserialize<DoctorAvailabilityDto>(cachedAvailability);

                    if (cachedDto != null)
                    {
                        _logger?.LogInformation(
                            "{CacheHitMessage}",
                            BuildCacheHitMessage(id, availabilityDate, cacheKey));

                        return cachedDto;
                    }
                }

                _logger?.LogInformation(
                    "{CacheMissMessage}",
                    BuildCacheMissMessage(id, availabilityDate, cacheKey));
            }

            Doctor? doctor;

            if (_doctorRepository != null)
            {
                doctor = await _doctorRepository.GetByIdAsync(id);
            }
            else if (_context != null)
            {
                doctor = await _context.Doctors
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.DoctorId == id, ct);
            }
            else
            {
                doctor = null;
            }

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            var bookedSlots = new List<string>();

            if (_context != null)
            {
                bookedSlots = await _context.Appointments
                    .AsNoTracking()
                    .Where(a =>
                        a.DoctorId == id &&
                        a.ScheduledDate.Date == availabilityDate &&
                        a.Status != AppointmentStatus.Cancelled)
                    .Select(a => a.TimeSlot)
                    .ToListAsync(ct);
            }

            var availableSlots = doctor.IsActive
                ? AllSlots.Except(bookedSlots).ToList()
                : new List<string>();

            var availability = new DoctorAvailabilityDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                IsActive = doctor.IsActive,
                Date = availabilityDate,
                AvailableSlots = availableSlots
            };

            if (_cache != null)
            {
                await _cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(availability),
                    AvailabilityCacheOptions,
                    ct);

                _logger?.LogInformation(
                    "{CacheStoredMessage}",
                    BuildCacheStoredMessage(
                        id,
                        availabilityDate,
                        cacheKey,
                        availableSlots.Count));
            }

            return availability;
        }

        public async Task<DoctorDto> AddAsync(
            CreateDoctorDto dto,
            CancellationToken ct = default)
        {
            if (_doctorRepository != null && _userManager != null && _mapper != null)
            {
                var existingUser = await _userManager.FindByEmailAsync(dto.Email);

                if (existingUser != null)
                {
                    throw new BusinessRuleException("Doctor login already exists with this email.");
                }

                var user = new ApplicationUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    MustChangePassword = true
                };

                var createResult = await _userManager.CreateAsync(user, dto.TemporaryPassword);

                if (!createResult.Succeeded)
                {
                    var errorMessage = createResult.Errors.FirstOrDefault()?.Description
                        ?? "Doctor login creation failed.";

                    throw new ValidationException(errorMessage);
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "Doctor");

                if (!roleResult.Succeeded)
                {
                    var errorMessage = roleResult.Errors.FirstOrDefault()?.Description
                        ?? "Doctor role assignment failed.";

                    throw new ValidationException(errorMessage);
                }

                var doctor = new Doctor
                {
                    UserId = user.Id,
                    FullName = dto.FullName,
                    Specialisation = dto.Specialisation,
                    YearsOfExperience = dto.YearsOfExperience,
                    ConsultationFee = dto.ConsultationFee,
                    IsActive = true
                };

                var savedDoctor = await _doctorRepository.AddAsync(doctor);

                return _mapper.Map<DoctorDto>(savedDoctor);
            }

            if (_context == null || _userManager == null)
            {
                throw new InvalidOperationException("Database context or user manager is not available.");
            }

            var existingDbUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingDbUser != null)
            {
                throw new BusinessRuleException("Doctor login already exists with this email.");
            }

            var dbUser = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                MustChangePassword = true
            };

            var dbCreateResult = await _userManager.CreateAsync(dbUser, dto.TemporaryPassword);

            if (!dbCreateResult.Succeeded)
            {
                var errorMessage = dbCreateResult.Errors.FirstOrDefault()?.Description
                    ?? "Doctor login creation failed.";

                throw new ValidationException(errorMessage);
            }

            var dbRoleResult = await _userManager.AddToRoleAsync(dbUser, "Doctor");

            if (!dbRoleResult.Succeeded)
            {
                var errorMessage = dbRoleResult.Errors.FirstOrDefault()?.Description
                    ?? "Doctor role assignment failed.";

                throw new ValidationException(errorMessage);
            }

            var dbDoctor = new Doctor
            {
                UserId = dbUser.Id,
                FullName = dto.FullName,
                Specialisation = dto.Specialisation,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = true
            };

            _context.Doctors.Add(dbDoctor);
            await _context.SaveChangesAsync(ct);

            return MapToDto(dbDoctor);
        }

        public async Task<DoctorDto> UpdateAsync(
            int id,
            UpdateDoctorDto dto,
            CancellationToken ct = default)
        {
            if (_doctorRepository != null && _mapper != null)
            {
                var existingDoctor = await _doctorRepository.GetByIdAsync(id);

                if (existingDoctor == null)
                {
                    throw new NotFoundException("Doctor not found");
                }

                _mapper.Map(dto, existingDoctor);

                var updatedDoctor = await _doctorRepository.UpdateAsync(id, existingDoctor, ct);

                await RemoveAvailabilityCacheAsync(id, DateTime.Today, ct);

                return _mapper.Map<DoctorDto>(updatedDoctor);
            }

            if (_context == null)
            {
                throw new InvalidOperationException("Database context is not available.");
            }

            var doctor = await _context.Doctors
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.DoctorId == id, ct);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            doctor.FullName = dto.FullName;
            doctor.Specialisation = dto.Specialisation;
            doctor.YearsOfExperience = dto.YearsOfExperience;
            doctor.ConsultationFee = dto.ConsultationFee;
            doctor.IsActive = dto.IsActive;

            await _context.SaveChangesAsync(ct);

            await RemoveAvailabilityCacheAsync(id, DateTime.Today, ct);

            return MapToDto(doctor);
        }

        private async Task RemoveAvailabilityCacheAsync(
            int doctorId,
            DateTime date,
            CancellationToken ct)
        {
            if (_cache == null)
            {
                return;
            }

            var cacheKey = $"doctors:{doctorId}:availability:{date:yyyy-MM-dd}";

            await _cache.RemoveAsync(cacheKey, ct);

            _logger?.LogInformation(
                "{CacheRemovedMessage}",
                BuildCacheRemovedMessage(doctorId, date, cacheKey));
        }

        private static string BuildCacheHitMessage(
            int doctorId,
            DateTime date,
            string cacheKey)
        {
            return
                Environment.NewLine +
                "========================================" + Environment.NewLine +
                "CACHE HIT - DOCTOR AVAILABILITY" + Environment.NewLine +
                "========================================" + Environment.NewLine +
                $"Doctor Id : {doctorId}" + Environment.NewLine +
                $"Date      : {date:yyyy-MM-dd}" + Environment.NewLine +
                $"Key       : {cacheKey}" + Environment.NewLine +
                "Source    : Garnet Cache" + Environment.NewLine +
                "========================================";
        }

        private static string BuildCacheMissMessage(
            int doctorId,
            DateTime date,
            string cacheKey)
        {
            return
                Environment.NewLine +
                "========================================" + Environment.NewLine +
                "CACHE MISS - DOCTOR AVAILABILITY" + Environment.NewLine +
                "========================================" + Environment.NewLine +
                $"Doctor Id : {doctorId}" + Environment.NewLine +
                $"Date      : {date:yyyy-MM-dd}" + Environment.NewLine +
                $"Key       : {cacheKey}" + Environment.NewLine +
                "Source    : SQL Server Required" + Environment.NewLine +
                "========================================";
        }

        private static string BuildCacheStoredMessage(
            int doctorId,
            DateTime date,
            string cacheKey,
            int slotCount)
        {
            return
                Environment.NewLine +
                "========================================" + Environment.NewLine +
                "CACHE STORED IN GARNET" + Environment.NewLine +
                "========================================" + Environment.NewLine +
                $"Doctor Id : {doctorId}" + Environment.NewLine +
                $"Date      : {date:yyyy-MM-dd}" + Environment.NewLine +
                $"Key       : {cacheKey}" + Environment.NewLine +
                "TTL       : 5 Minutes" + Environment.NewLine +
                $"Slots     : {slotCount}" + Environment.NewLine +
                "========================================";
        }

        private static string BuildCacheRemovedMessage(
            int doctorId,
            DateTime date,
            string cacheKey)
        {
            return
                Environment.NewLine +
                "========================================" + Environment.NewLine +
                "CACHE REMOVED - DOCTOR AVAILABILITY" + Environment.NewLine +
                "========================================" + Environment.NewLine +
                $"Doctor Id : {doctorId}" + Environment.NewLine +
                $"Date      : {date:yyyy-MM-dd}" + Environment.NewLine +
                $"Key       : {cacheKey}" + Environment.NewLine +
                "Reason    : Doctor schedule/status updated" + Environment.NewLine +
                "========================================";
        }

        private static DoctorDto MapToDto(Doctor doctor)
        {
            return new DoctorDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                Specialisation = doctor.Specialisation,
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = doctor.IsActive,
                UpcomingAppointmentCount = doctor.Appointments?.Count(a =>
                    a.ScheduledDate.Date >= DateTime.Today &&
                    a.Status != AppointmentStatus.Cancelled) ?? 0
            };
        }
    }
}