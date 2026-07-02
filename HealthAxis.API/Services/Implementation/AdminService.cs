using AutoMapper;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Data;
using HealthAxis.API.Repositories.Implementations;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO;
using HealthAxis.Shared.DTO.AdminDtos;
using HealthAxis.Shared.DTO.CommonDtos;
using HealthAxis.Shared.DTO.DoctorDtos;
using HealthAxis.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HealthAxis.API.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private const int DoctorPageSize = 6;
        private const int UserPageSize = 6;

        public AdminService(
     IDoctorRepository doctorRepository,
     IAppointmentRepository appointmentRepository,
     UserManager<IdentityUser> userManager,
     RoleManager<IdentityRole> roleManager,
     IMapper mapper,
     ApplicationDbContext context)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();

            return _mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorCreatedDto> AddDoctorAsync(CreateDoctorDto doctorDto)
        {
            if (string.IsNullOrWhiteSpace(doctorDto.FullName))
            {
                throw new ValidationException("Doctor name is required");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch( doctorDto.FullName, @"^[A-Za-z ]+$"))
            {
                throw new ValidationException(
                    "Doctor name should contain only alphabets and spaces");
            }

            if (string.IsNullOrWhiteSpace(doctorDto.Email))
            {
                throw new ValidationException("Doctor email is required");
            }

            //if (!doctorDto.Email.EndsWith( "@gmail.com",
            //        StringComparison.OrdinalIgnoreCase))
            //{
            //    throw new ValidationException("Only Gmail address is allowed");
            //}

            if (!Enum.IsDefined( typeof(Specialisation), doctorDto.Specialisation))
            {
                throw new ValidationException("Invalid specialisation");
            }

            if (doctorDto.YearsOfExperience < 0 ||
                doctorDto.YearsOfExperience > 60)
            {
                throw new ValidationException(
                    "Years of experience must be between 0 and 60");
            }

            if (doctorDto.ConsultationFee < 100 ||
                doctorDto.ConsultationFee > 10000)
            {
                throw new ValidationException(
                    "Consultation fee must be between 100 and 10000");
            }

            var existingUser =
                await _userManager.FindByEmailAsync(doctorDto.Email);

            if (existingUser != null)
            {
                throw new BusinessRuleException("Doctor email already exists");
            }

            var roleExists = _roleManager.RoleExistsAsync("Doctor").GetAwaiter().GetResult();
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));
            }

            var temporaryPassword = GenerateTemporaryPassword();

            var user = new IdentityUser
            {
                UserName = doctorDto.Email,
                Email = doctorDto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, temporaryPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ",  result.Errors.Select(error => error.Description));

                throw new ValidationException(errors);
            }

            await _userManager.AddToRoleAsync(user, "Doctor");

            var doctor = new Doctor
            {
                FullName = doctorDto.FullName,
                Specialisation = doctorDto.Specialisation,
                YearsOfExperience = doctorDto.YearsOfExperience,
                ConsultationFee = doctorDto.ConsultationFee,
                IsActive = doctorDto.IsActive,
                UserId = user.Id
            };

            var savedDoctor = await _doctorRepository.AddAsync(doctor);

            await _userManager.AddClaimAsync(user, new Claim("DoctorId", savedDoctor.DoctorId.ToString()));

            return new DoctorCreatedDto
            {
                DoctorId = savedDoctor.DoctorId,
                FullName = savedDoctor.FullName,
                Email = doctorDto.Email,
                Specialisation = savedDoctor.Specialisation,
                YearsOfExperience = savedDoctor.YearsOfExperience,
                ConsultationFee = savedDoctor.ConsultationFee,
                IsActive = savedDoctor.IsActive,
                TemporaryPassword = temporaryPassword
            };
        }

        public async Task<DoctorDto> UpdateDoctorAsync( int id, UpdateDoctorDto doctorDto)
        {
            var existingDoctor = await _doctorRepository.GetByIdAsync(id);

            if (existingDoctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            if (string.IsNullOrWhiteSpace(doctorDto.FullName))
            {
                throw new ValidationException("Doctor full name is required");
            }

            if (doctorDto.YearsOfExperience < 0 ||
                doctorDto.YearsOfExperience > 60)
            {
                throw new ValidationException(
                    "Years of experience must be between 0 and 60");
            }

            if (doctorDto.ConsultationFee <= 0)
            {
                throw new ValidationException(
                    "Consultation fee must be greater than 0");
            }

            existingDoctor.FullName = doctorDto.FullName;
            existingDoctor.Specialisation = doctorDto.Specialisation;
            existingDoctor.YearsOfExperience = doctorDto.YearsOfExperience;
            existingDoctor.ConsultationFee = doctorDto.ConsultationFee;
            existingDoctor.IsActive = doctorDto.IsActive;

            var updatedDoctor = await _doctorRepository.UpdateAsync(id, existingDoctor);

            return _mapper.Map<DoctorDto>(updatedDoctor);
        }

        public async Task<List<AdminDto>> GetAppointmentReportsAsync()
        {
            var appointments =  await _appointmentRepository.GetAllAsync();

            var reports = appointments
                .GroupBy(a => a.ScheduledDate.Date)
                .Select(group => new AdminDto
                {
                    Date = group.Key,

                    ConfirmedCount = group.Count(a =>
                        a.Status == AppointmentStatus.Confirmed),

                    CancelledCount = group.Count(a =>
                        a.Status == AppointmentStatus.Cancelled),

                    CompletedCount = group.Count(a =>
                        a.Status == AppointmentStatus.Completed)
                })
                .OrderBy(report => report.Date)
                .ToList();

            return reports;
        }
        public async Task<PagedResponseDto<DoctorDto>> GetDoctorsPagedAsync(
    PaginationQueryDto paginationQuery)
        {
            ArgumentNullException.ThrowIfNull(paginationQuery);

            var pageNumber = Math.Max(paginationQuery.PageNumber, 1);

            var normalizedQuery = new PaginationQueryDto
            {
                PageNumber = pageNumber,
                PageSize = DoctorPageSize
            };

            var totalRecords = await _doctorRepository.CountAsync();

            var doctors = await _doctorRepository.GetPagedAsync(
                normalizedQuery,
                doctor => doctor.DoctorId,
                descending: true);

            var doctorDtos = new List<DoctorDto>();

            foreach (var doctor in doctors)
            {
                doctorDtos.Add(await MapDoctorForAdminAsync(doctor));
            }

            return new PagedResponseDto<DoctorDto>
            {
                Items = doctorDtos,
                PageNumber = pageNumber,
                PageSize = DoctorPageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<List<AdminUserDto>> GetUsersAsync()
        {
            var identityUsers = await _userManager.Users
                .OrderBy(user => user.Email)
                .ToListAsync();

            return await BuildAdminUserDtosAsync(identityUsers);
        }

        public async Task<PagedResponseDto<AdminUserDto>> GetUsersPagedAsync(
            AdminUserQueryDto queryDto)
        {
            ArgumentNullException.ThrowIfNull(queryDto);

            var pageNumber = Math.Max(queryDto.PageNumber, 1);

            var identityUsers = await _userManager.Users
                .OrderBy(user => user.Email)
                .ToListAsync();

            var mappedUsers = await BuildAdminUserDtosAsync(identityUsers);

            var filteredUsers = mappedUsers
                .Where(user => MatchesSearch(user, queryDto.SearchText))
                .Where(user => MatchesRole(user, queryDto.Role))
                .OrderBy(user => user.Email)
                .ToList();

            var totalRecords = filteredUsers.Count;

            var pagedUsers = filteredUsers
                .Skip((pageNumber - 1) * UserPageSize)
                .Take(UserPageSize)
                .ToList();

            return new PagedResponseDto<AdminUserDto>
            {
                Items = pagedUsers,
                PageNumber = pageNumber,
                PageSize = UserPageSize,
                TotalRecords = totalRecords
            };
        }

        private async Task<List<AdminUserDto>> BuildAdminUserDtosAsync(
            List<IdentityUser> identityUsers)
        {
            var doctors = await _doctorRepository.GetAllAsync();

            var patients = await _context.Patients
                .AsNoTracking()
                .ToListAsync();

            var result = new List<AdminUserDto>();

            foreach (var user in identityUsers)
            {
                var adminUser = await MapUserForAdminAsync(
                    user,
                    doctors,
                    patients);

                result.Add(adminUser);
            }

            return result;
        }

        private async Task<AdminUserDto> MapUserForAdminAsync(
            IdentityUser user,
            IEnumerable<Doctor> doctors,
            IEnumerable<Patient> patients)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? string.Empty;

            var fullName = user.Email ?? string.Empty;
            var phoneNumber = user.PhoneNumber ?? "Not available";
            DateTime? createdDate = null;
            var isActive = true;

            if (string.Equals(role, "Doctor", StringComparison.OrdinalIgnoreCase))
            {
                var doctor = doctors.FirstOrDefault(doctorRecord =>
                    doctorRecord.UserId == user.Id);

                if (doctor != null)
                {
                    fullName = doctor.FullName;
                    isActive = doctor.IsActive;
                }
            }

            if (string.Equals(role, "Patient", StringComparison.OrdinalIgnoreCase))
            {
                var patient = patients.FirstOrDefault(patientRecord =>
                    patientRecord.UserId == user.Id);

                if (patient != null)
                {
                    fullName = patient.FullName;
                    phoneNumber = patient.PhoneNumber;
                    createdDate = patient.CreatedDate;
                }
            }

            return new AdminUserDto
            {
                UserId = user.Id,
                FullName = fullName,
                Email = user.Email ?? string.Empty,
                Role = role,
                IsActive = isActive,
                PhoneNumber = phoneNumber,
                CreatedDate = createdDate
            };
        }

        private static bool MatchesSearch(
            AdminUserDto user,
            string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return true;
            }

            return ContainsIgnoreCase(user.FullName, searchText) ||
                ContainsIgnoreCase(user.Email, searchText) ||
                ContainsIgnoreCase(user.Role, searchText) ||
                ContainsIgnoreCase(user.PhoneNumber, searchText);
        }

        private static bool MatchesRole(
            AdminUserDto user,
            string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return true;
            }

            return string.Equals(
                user.Role,
                role,
                StringComparison.OrdinalIgnoreCase);
        }

        private static bool ContainsIgnoreCase(
            string? value,
            string searchText)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                value.Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }
        private static string GenerateTemporaryPassword()
        {
            return "Doctor@" + Random.Shared.Next(1000, 9999);
        }
        private static void ValidateDoctor(DoctorDto doctorDto)
        {
            if (string.IsNullOrWhiteSpace(doctorDto.FullName))
            {
                throw new ValidationException("Doctor full name is required");
            }

            if (!Enum.IsDefined(typeof(Specialisation), doctorDto.Specialisation))
            {
                throw new ValidationException("Invalid specialisation");
            }

            if (doctorDto.YearsOfExperience < 0 ||
                doctorDto.YearsOfExperience > 60)
            {
                throw new ValidationException(
                    "Years of experience must be between 0 and 60");
            }

            if (doctorDto.ConsultationFee <= 0)
            {
                throw new ValidationException(
                    "Consultation fee must be greater than 0");
            }
        }
        private async Task<DoctorDto> MapDoctorForAdminAsync(Doctor doctor)
        {
            var email = string.Empty;

            if (!string.IsNullOrWhiteSpace(doctor.UserId))
            {
                var user = await _userManager.FindByIdAsync(doctor.UserId);
                email = user?.Email ?? string.Empty;
            }

            return new DoctorDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                Email = email,
                Specialisation = doctor.Specialisation,
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = doctor.IsActive
            };
       
        }

        public async Task<AdminAppointmentDetailDto> UpdateAppointmentStatusByAdminAsync(
    int appointmentId,
    AdminUpdateAppointmentStatusDto statusDto)
        {
            ArgumentNullException.ThrowIfNull(statusDto);

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(appointmentRecord =>
                    appointmentRecord.AppointmentId == appointmentId);

            if (appointment == null)
            {
                throw new ValidationException("Appointment not found.");
            }

            if (!Enum.TryParse<AppointmentStatus>(
                    statusDto.Status,
                    true,
                    out var newStatus))
            {
                throw new ValidationException("Invalid appointment status.");
            }

            if (appointment.Status == AppointmentStatus.Completed ||
                appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new ValidationException(
                    "Completed or cancelled appointment status cannot be changed.");
            }

            if (appointment.Status == AppointmentStatus.Pending &&
                newStatus != AppointmentStatus.Confirmed &&
                newStatus != AppointmentStatus.Cancelled)
            {
                throw new ValidationException(
                    "Pending appointment can only be confirmed or cancelled.");
            }

            if (appointment.Status == AppointmentStatus.Confirmed &&
                newStatus != AppointmentStatus.Completed &&
                newStatus != AppointmentStatus.Cancelled)
            {
                throw new ValidationException(
                    "Confirmed appointment can only be completed or cancelled.");
            }

            appointment.Status = newStatus;

            await _context.SaveChangesAsync();

            return await GetAppointmentDetailByIdAsync(appointmentId);
        }

        private async Task<AdminAppointmentDetailDto> GetAppointmentDetailByIdAsync(
            int appointmentId)
        {
            var appointmentDetail = await _context.Appointments
                .AsNoTracking()
                .Where(appointment => appointment.AppointmentId == appointmentId)
                .Join(
                    _context.Patients.AsNoTracking(),
                    appointment => appointment.PatientId,
                    patient => patient.PatientId,
                    (appointment, patient) => new
                    {
                        appointment,
                        patient
                    })
                .Join(
                    _context.Doctors.AsNoTracking(),
                    data => data.appointment.DoctorId,
                    doctor => doctor.DoctorId,
                    (data, doctor) => new AdminAppointmentDetailDto
                    {
                        AppointmentId = data.appointment.AppointmentId,
                        PatientId = data.patient.PatientId,
                        PatientName = data.patient.FullName,
                        DoctorId = doctor.DoctorId,
                        DoctorName = doctor.FullName,
                        Specialisation = doctor.Specialisation.ToString(),
                        ScheduledDate = data.appointment.ScheduledDate,
                        TimeSlot = data.appointment.TimeSlot,
                        Status = data.appointment.Status.ToString()
                    })
                .FirstOrDefaultAsync();

            return appointmentDetail ??
                throw new ValidationException("Appointment details not found.");
        }
        public async Task<PagedResponseDto<AdminUserDto>> GetUsersPagedAsync(
    PaginationQueryDto paginationQuery)
        {
            ArgumentNullException.ThrowIfNull(paginationQuery);

            var pageNumber = Math.Max(paginationQuery.PageNumber, 1);

            var totalRecords = await _userManager.Users.CountAsync();

            var pagedUsers = await _userManager.Users
                .OrderBy(user => user.Email)
                .Skip((pageNumber - 1) * UserPageSize)
                .Take(UserPageSize)
                .ToListAsync();

            var doctors = await _doctorRepository.GetAllAsync();

            var result = new List<AdminUserDto>();

            foreach (var user in pagedUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? string.Empty;

                string fullName = user.Email ?? string.Empty;
                bool isActive = true;

                if (string.Equals(role, "Doctor", StringComparison.OrdinalIgnoreCase))
                {
                    var doctor = doctors.FirstOrDefault(d => d.UserId == user.Id);

                    if (doctor != null)
                    {
                        fullName = doctor.FullName;
                        isActive = doctor.IsActive;
                    }
                }

                result.Add(new AdminUserDto
                {
                    UserId = user.Id,
                    FullName = fullName,
                    Email = user.Email ?? string.Empty,
                    Role = role,
                    IsActive = isActive
                });
            }

            return new PagedResponseDto<AdminUserDto>
            {
                Items = result,
                PageNumber = pageNumber,
                PageSize = UserPageSize,
                TotalRecords = totalRecords
            };
        }
        public async Task<List<AdminAppointmentDetailDto>> GetAppointmentDetailsAsync()
        {
            var appointmentDetails = await _context.Appointments
                .AsNoTracking()
                .Join(
                    _context.Patients.AsNoTracking(),
                    appointment => appointment.PatientId,
                    patient => patient.PatientId,
                    (appointment, patient) => new
                    {
                        appointment,
                        patient
                    })
                .Join(
                    _context.Doctors.AsNoTracking(),
                    data => data.appointment.DoctorId,
                    doctor => doctor.DoctorId,
                    (data, doctor) => new AdminAppointmentDetailDto
                    {
                        AppointmentId = data.appointment.AppointmentId,
                        PatientId = data.patient.PatientId,
                        PatientName = data.patient.FullName,
                        DoctorId = doctor.DoctorId,
                        DoctorName = doctor.FullName,
                        Specialisation = doctor.Specialisation.ToString(),
                        ScheduledDate = data.appointment.ScheduledDate,
                        TimeSlot = data.appointment.TimeSlot,
                        Status = data.appointment.Status.ToString()
                    })
                .OrderByDescending(report => report.ScheduledDate)
                .ThenBy(report => report.TimeSlot)
                .ToListAsync();

            return appointmentDetails;
        }

        public async Task<AdminProfileDto> GetAdminProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("Admin user not found");
            }

            return new AdminProfileDto
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty
            };
        }

        public async Task<AdminProfileDto> UpdateAdminProfileAsync(
            string userId,
            UpdateAdminProfileDto profileDto)
        {
            ArgumentNullException.ThrowIfNull(profileDto);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("Admin user not found");
            }

            user.Email = profileDto.Email;
            user.UserName = profileDto.Email;
            user.PhoneNumber = profileDto.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(error => error.Description));

                throw new ValidationException(errors);
            }

            return new AdminProfileDto
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty
            };
        }

        public async Task ChangeAdminPasswordAsync(
            string userId,
            ChangePasswordDto passwordDto)
        {
            ArgumentNullException.ThrowIfNull(passwordDto);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("Admin user not found");
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                passwordDto.CurrentPassword,
                passwordDto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(error => error.Description));

                throw new ValidationException(errors);
            }
        }
        public async Task<List<AdminPatientDto>> GetPatientsAsync()
        {
            var patients = await _context.Patients
                .AsNoTracking()
                .OrderBy(patient => patient.FullName)
                .ToListAsync();

            return patients.Select(MapPatientForAdmin).ToList();
        }

        public async Task<AdminPatientDto> UpdatePatientAsync(
            int patientId,
            UpdateAdminPatientDto patientDto)
        {
            ArgumentNullException.ThrowIfNull(patientDto);

            var patient = await _context.Patients
                .FirstOrDefaultAsync(patientRecord => patientRecord.PatientId == patientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found.");
            }

            patient.FullName = patientDto.FullName;
            patient.DateOfBirth = patientDto.DateOfBirth;
            patient.PhoneNumber = patientDto.PhoneNumber;
            patient.Email = patientDto.Email;

            if (Enum.TryParse(patient.Gender.GetType(), patientDto.Gender, true, out var genderValue))
            {
                patient.Gender = (dynamic)genderValue;
            }

            SetPatientAddress(patient, patientDto.Address);

            if (!string.IsNullOrWhiteSpace(patient.UserId))
            {
                var user = await _userManager.FindByIdAsync(patient.UserId);

                if (user != null)
                {
                    user.Email = patientDto.Email;
                    user.UserName = patientDto.Email;
                    user.PhoneNumber = patientDto.PhoneNumber;

                    await _userManager.UpdateAsync(user);
                }
            }

            await _context.SaveChangesAsync();

            return MapPatientForAdmin(patient);
        }

        public async Task<List<AdminPatientAppointmentDto>> GetPatientAppointmentsAsync(
            int patientId)
        {
            var appointments = await _context.Appointments
                .AsNoTracking()
                .Where(appointment => appointment.PatientId == patientId)
                .Join(
                    _context.Doctors.AsNoTracking(),
                    appointment => appointment.DoctorId,
                    doctor => doctor.DoctorId,
                    (appointment, doctor) => new AdminPatientAppointmentDto
                    {
                        AppointmentId = appointment.AppointmentId,
                        DoctorName = doctor.FullName,
                        Specialisation = doctor.Specialisation.ToString(),
                        ScheduledDate = appointment.ScheduledDate,
                        TimeSlot = appointment.TimeSlot,
                        Status = appointment.Status.ToString()
                    })
                .OrderByDescending(appointment => appointment.ScheduledDate)
                .ToListAsync();

            return appointments;
        }

        private static AdminPatientDto MapPatientForAdmin(Patient patient)
        {
            return new AdminPatientDto
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender.ToString(),
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                Address = GetPatientAddress(patient),
                CreatedDate = patient.CreatedDate
            };
        }

        private static string GetPatientAddress(Patient patient)
        {
            var addressProperty = patient.GetType().GetProperty("Address");

            return addressProperty?.GetValue(patient)?.ToString() ?? string.Empty;
        }

        private static void SetPatientAddress(
            Patient patient,
            string address)
        {
            var addressProperty = patient.GetType().GetProperty("Address");

            if (addressProperty?.CanWrite == true)
            {
                addressProperty.SetValue(patient, address);
            }
        }
    }
}