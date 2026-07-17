using HealthAxis.API.Data;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO;
using HealthAxis.Shared.DTO.AdminDtos;
using HealthAxis.Shared.DTO.CommonDtos;
using HealthAxis.Shared.DTO.DoctorDtos;
using HealthAxis.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace HealthAxis.API.Services.Implementation
{
    public partial class AdminService : IAdminService
    {
        private const string DoctorRole = "Doctor";
        private const string PatientRole = "Patient";
        private const string AdminRole = "Admin";
        private const string AdminDisplayName = "Admin";

        private const int DoctorPageSize = 6;
        private const int UserPageSize = 6;

        [GeneratedRegex(@"^[A-Za-z ]+$")]
        private static partial Regex DoctorNameRegex();

        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminService(
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            var doctorDtos = new List<DoctorDto>();

            foreach (var doctor in doctors)
            {
                doctorDtos.Add(await MapDoctorForAdminAsync(doctor));
            }

            return doctorDtos;
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

        public async Task<DoctorCreatedDto> AddDoctorAsync(
            CreateDoctorDto doctorDto)
        {
            ArgumentNullException.ThrowIfNull(doctorDto);

            if (string.IsNullOrWhiteSpace(doctorDto.FullName))
            {
                throw new ValidationException("Doctor name is required.");
            }

            if (!DoctorNameRegex().IsMatch(doctorDto.FullName))
            {
                throw new ValidationException(
                    "Doctor name should contain only alphabets and spaces.");
            }

            if (string.IsNullOrWhiteSpace(doctorDto.Email))
            {
                throw new ValidationException("Doctor email is required.");
            }

            if (!Enum.IsDefined(doctorDto.Specialisation))
            {
                throw new ValidationException("Invalid specialisation.");
            }

            if (doctorDto.YearsOfExperience < 0 ||
                doctorDto.YearsOfExperience > 60)
            {
                throw new ValidationException(
                    "Years of experience must be between 0 and 60.");
            }

            if (doctorDto.ConsultationFee < 100 ||
                doctorDto.ConsultationFee > 10000)
            {
                throw new ValidationException(
                    "Consultation fee must be between 100 and 10000.");
            }

            var existingUser = await _userManager.FindByEmailAsync(
                doctorDto.Email);

            if (existingUser != null)
            {
                throw new BusinessRuleException("Doctor email already exists.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(DoctorRole);

            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(DoctorRole));
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
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(error => error.Description));

                throw new ValidationException(errors);
            }

            await _userManager.AddToRoleAsync(user, DoctorRole);

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

            await _userManager.AddClaimAsync(
                user,
                new Claim("DoctorId", savedDoctor.DoctorId.ToString()));

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

        public async Task<DoctorDto> UpdateDoctorAsync(
            int id,
            UpdateDoctorDto doctorDto)
        {
            ArgumentNullException.ThrowIfNull(doctorDto);

            var existingDoctor = await _doctorRepository.GetByIdAsync(id);

            if (existingDoctor == null)
            {
                throw new NotFoundException("Doctor not found.");
            }

            if (string.IsNullOrWhiteSpace(doctorDto.FullName))
            {
                throw new ValidationException("Doctor full name is required.");
            }

            if (doctorDto.YearsOfExperience < 0 ||
                doctorDto.YearsOfExperience > 60)
            {
                throw new ValidationException(
                    "Years of experience must be between 0 and 60.");
            }

            if (doctorDto.ConsultationFee <= 0)
            {
                throw new ValidationException(
                    "Consultation fee must be greater than 0.");
            }

            existingDoctor.FullName = doctorDto.FullName;
            existingDoctor.Specialisation = doctorDto.Specialisation;
            existingDoctor.YearsOfExperience = doctorDto.YearsOfExperience;
            existingDoctor.ConsultationFee = doctorDto.ConsultationFee;
            existingDoctor.IsActive = doctorDto.IsActive;

            var updatedDoctor = await _doctorRepository.UpdateAsync(
                id,
                existingDoctor);

            if (updatedDoctor == null)
            {
                throw new NotFoundException("Doctor not found after update.");
            }

            return await MapDoctorForAdminAsync(updatedDoctor);
        }

        public async Task<List<AdminDto>> GetAppointmentReportsAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();

            return appointments
                .GroupBy(appointment => appointment.ScheduledDate.Date)
                .Select(group => new AdminDto
                {
                    Date = group.Key,
                    ConfirmedCount = group.Count(appointment =>
                        appointment.Status == AppointmentStatus.Confirmed),
                    CancelledCount = group.Count(appointment =>
                        appointment.Status == AppointmentStatus.Cancelled),
                    CompletedCount = group.Count(appointment =>
                        appointment.Status == AppointmentStatus.Completed)
                })
                .OrderBy(report => report.Date)
                .ToList();
        }

        public async Task<List<AdminAppointmentDetailDto>> GetAppointmentDetailsAsync()
        {
            var appointments = await _context.Appointments
                .AsNoTracking()
                .Include(appointment => appointment.Patient)
                .Include(appointment => appointment.Doctor)
                .OrderByDescending(appointment => appointment.ScheduledDate)
                .ThenBy(appointment => appointment.TimeSlot)
                .ToListAsync();

            return appointments
                .Select(MapAppointmentDetailForAdmin)
                .ToList();
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

            if (newStatus == AppointmentStatus.Cancelled)
            {
                appointment.CancellationReason = GetAdminCancellationReason(
                    statusDto.CancellationReason);
            }
            else
            {
                appointment.CancellationReason = null;
            }

            await _context.SaveChangesAsync();

            return await GetAppointmentDetailByIdAsync(appointmentId);
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

        public async Task<PagedResponseDto<AdminUserDto>> GetUsersPagedAsync(
            PaginationQueryDto paginationQuery)
        {
            ArgumentNullException.ThrowIfNull(paginationQuery);

            var pageNumber = Math.Max(paginationQuery.PageNumber, 1);

            var identityUsers = await _userManager.Users
                .OrderBy(user => user.Email)
                .ToListAsync();

            var mappedUsers = await BuildAdminUserDtosAsync(identityUsers);

            var totalRecords = mappedUsers.Count;

            var pagedUsers = mappedUsers
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

        public async Task<AdminProfileDto> GetAdminProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("Admin user not found.");
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
                throw new NotFoundException("Admin user not found.");
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
                throw new NotFoundException("Admin user not found.");
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

            return patients
                .Select(MapPatientForAdmin)
                .ToList();
        }

        public async Task<AdminPatientDto> UpdatePatientAsync(
            int patientId,
            UpdateAdminPatientDto patientDto)
        {
            ArgumentNullException.ThrowIfNull(patientDto);

            var patient = await _context.Patients
                .FirstOrDefaultAsync(patientRecord =>
                    patientRecord.PatientId == patientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found.");
            }

            patient.FullName = patientDto.FullName;
            patient.DateOfBirth = patientDto.DateOfBirth;
            patient.PhoneNumber = patientDto.PhoneNumber;
            patient.Email = patientDto.Email;

            if (Enum.TryParse<Gender>(
                    patientDto.Gender,
                    true,
                    out var genderValue))
            {
                patient.Gender = genderValue;
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

        private async Task<AdminAppointmentDetailDto> GetAppointmentDetailByIdAsync(
            int appointmentId)
        {
            var appointment = await _context.Appointments
                .AsNoTracking()
                .Include(appointmentRecord => appointmentRecord.Patient)
                .Include(appointmentRecord => appointmentRecord.Doctor)
                .FirstOrDefaultAsync(appointmentRecord =>
                    appointmentRecord.AppointmentId == appointmentId);

            return appointment == null
                ? throw new ValidationException("Appointment details not found.")
                : MapAppointmentDetailForAdmin(appointment);
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

            var fullName = string.Equals(
                role,
                AdminRole,
                StringComparison.OrdinalIgnoreCase)
                ? AdminDisplayName
                : user.Email ?? string.Empty;
            var phoneNumber = user.PhoneNumber ?? "Not available";
            var specialisation = string.Empty;
            DateTime? createdDate = null;
            var isActive = true;

            if (string.Equals(
                    role,
                    DoctorRole,
                    StringComparison.OrdinalIgnoreCase))
            {
                var doctor = doctors.FirstOrDefault(doctorRecord =>
                    doctorRecord.UserId == user.Id);

                if (doctor != null)
                {
                    fullName = doctor.FullName;
                    isActive = doctor.IsActive;
                    specialisation = doctor.Specialisation.ToString();
                }
            }

            if (string.Equals(
                    role,
                    PatientRole,
                    StringComparison.OrdinalIgnoreCase))
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
                CreatedDate = createdDate,
                Specialisation = specialisation
            };
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

        private static AdminAppointmentDetailDto MapAppointmentDetailForAdmin(
            Appointment appointment)
        {
            return new AdminAppointmentDetailDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient.FullName,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor.FullName,
                Specialisation = appointment.Doctor.Specialisation.ToString(),
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = appointment.TimeSlot,
                Status = appointment.Status.ToString(),
                CancelledBy = GetCancelledBy(
                    appointment.Status,
                    appointment.CancellationReason),
                CancellationReason = GetCancellationReason(
                    appointment.Status,
                    appointment.CancellationReason)
            };
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

        private static string GetAdminCancellationReason(string? reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                return "Cancelled by admin.";
            }

            return $"Cancelled by admin. Reason: {reason.Trim()}";
        }

        private static string GetCancelledBy(
            AppointmentStatus status,
            string? cancellationReason)
        {
            if (status != AppointmentStatus.Cancelled)
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(cancellationReason))
            {
                return "Not specified";
            }

            var cleanReason = cancellationReason.Trim();

            if (cleanReason.Contains(
                    "cancelled by patient",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "Patient";
            }

            if (cleanReason.Contains(
                    "cancelled by doctor",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "Doctor";
            }

            if (cleanReason.Contains(
                    "cancelled by admin",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "Admin";
            }

            return "Not specified";
        }

        private static string GetCancellationReason(
            AppointmentStatus status,
            string? cancellationReason)
        {
            if (status != AppointmentStatus.Cancelled)
            {
                return string.Empty;
            }

            return string.IsNullOrWhiteSpace(cancellationReason)
                ? "No cancellation reason available."
                : cancellationReason.Trim();
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
                ContainsIgnoreCase(user.PhoneNumber, searchText) ||
                ContainsIgnoreCase(user.Specialisation, searchText);
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