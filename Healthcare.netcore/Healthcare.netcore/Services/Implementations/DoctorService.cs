using AutoMapper;
using HealthAxis.API.Data;
using Microsoft.Extensions.DependencyInjection;
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
using ValidationException = HealthAxis.API.Exceptions.ValidationException;

namespace HealthAxis.API.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly HealthAxisDbContext? _context;
        private readonly IDoctorRepository? _doctorRepository;
        private readonly UserManager<ApplicationUser>? _userManager;
        private readonly IMapper? _mapper;

        // Used by API runtime

        [ActivatorUtilitiesConstructor]
        public DoctorService(HealthAxisDbContext context)
        {
            _context = context;
        }


        // Used by unit tests
        public DoctorService(
            IDoctorRepository doctorRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        // Backward-compatible method used by old test cases
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
            if (_context == null)
            {
                var doctors = await GetAllAsync();

                var pageNumber = paginationParams.PageNumber <= 0 ? 1 : paginationParams.PageNumber;
                var pageSize = paginationParams.PageSize <= 0 ? 10 : paginationParams.PageSize;

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
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
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
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize,
                TotalRecords = total,
                TotalPages = (int)Math.Ceiling(total / (double)paginationParams.PageSize)
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

        public async Task<object> GetAvailabilityAsync(
            int id,
            CancellationToken ct = default)
        {
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

            return new
            {
                doctor.DoctorId,
                doctor.FullName,
                doctor.IsActive
            };
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

            if (_context == null)
            {
                throw new InvalidOperationException("Database context is not available.");
            }

            var dbDoctor = new Doctor
            {
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

            return MapToDto(doctor);
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
                UpcomingAppointmentCount = doctor.Appointments.Count(a =>
                    a.ScheduledDate.Date >= DateTime.Today &&
                    a.Status != AppointmentStatus.Cancelled)
            };
        }
    }
}