using AutoMapper;
using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.DTOs.DoctorDtos;
using HealthAxisHealth.API.DTOs.ReportDtos;
using HealthAxisHealth.API.DTOs.UserDtos;
using HealthAxisHealth.API.Enums;
using HealthAxisHealth.API.Exceptions;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Services.Interfaces;
using HealthAxisHealth.API.UnitOfWork;

namespace HealthAxisHealth.API.Services.Implementations
{
    public class AdminService : IAdminService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public AdminService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<PagedResultDto<DoctorDto>> GetDoctorsAsync(
            PaginationParams pagination)
        {
            PagedResultDto<Doctor> result =
                await _unitOfWork.Doctors
                    .GetPagedAsync(pagination);

            return new PagedResultDto<DoctorDto>
            {
                Items = _mapper.Map<List<DoctorDto>>(result.Items),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalRecords = result.TotalRecords
            };
        }

        public async Task<int> CreateDoctorAsync(
            CreateDoctorDto dto)
        {
            User? existingUser =
                await _unitOfWork.Users
                    .GetByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                throw new BadRequestException(
                    "Email is already registered.");
            }

            User user = new User
            {
                Email = dto.Email,
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = UserRole.Doctor,
                IsActive = true
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            Doctor doctor = new Doctor
            {
                UserId = user.UserId,
                FullName = dto.FullName,
                Specialisation = dto.Specialisation,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = true
            };

            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.CommitAsync();

            return doctor.DoctorId;
        }

        public async Task UpdateDoctorAsync(
            int doctorId,
            UpdateDoctorDto dto)
        {
            Doctor? doctor =
                await _unitOfWork.Doctors
                    .GetDoctorWithAppointmentsAsync(doctorId);

            if (doctor == null)
            {
                throw new NotFoundException(
                    "Doctor not found.");
            }

            // Prevent deactivating a doctor with upcoming appointments
            if (!dto.IsActive &&
                doctor.GetUpcomingAppointmentCount() > 0)
            {
                throw new BadRequestException(
                    "Cannot deactivate a doctor with upcoming appointments.");
            }

            _mapper.Map(dto, doctor);

            _unitOfWork.Doctors.Update(doctor);

            // Synchronize User.IsActive with Doctor.IsActive
            User? user =
                await _unitOfWork.Users
                    .GetByIdAsync(doctor.UserId);

            if (user == null)
            {
                throw new NotFoundException(
                    "Associated user not found.");
            }

            user.IsActive = dto.IsActive;

            _unitOfWork.Users.Update(user);

            await _unitOfWork.CommitAsync();
        }

        public async Task<PagedResultDto<UserDto>> GetUsersAsync(
            PaginationParams pagination,
            string? role)
        {
            UserRole? parsedRole = null;

            if (!string.IsNullOrWhiteSpace(role) &&
                Enum.TryParse(
                    role,
                    true,
                    out UserRole userRole))
            {
                parsedRole = userRole;
            }

            PagedResultDto<User> result =
                await _unitOfWork.Users
                    .GetPagedAsync(
                        pagination,
                        parsedRole);

            return new PagedResultDto<UserDto>
            {
                Items = _mapper.Map<List<UserDto>>(result.Items),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalRecords = result.TotalRecords
            };
        }

        public async Task<IEnumerable<AppointmentReportDto>>
            GetAppointmentReportAsync()
        {
            IEnumerable<Appointment> appointments =
                await _unitOfWork.Appointments
                    .GetAllAsync();

            return appointments
                .GroupBy(a => a.ScheduledDate.Date)
                .Select(g => new AppointmentReportDto
                {
                    Date = g.Key,

                    ConfirmedCount = g.Count(a =>
                        a.Status == AppointmentStatus.Confirmed),

                    CancelledCount = g.Count(a =>
                        a.Status == AppointmentStatus.Cancelled),

                    CompletedCount = g.Count(a =>
                        a.Status == AppointmentStatus.Completed)
                })
                .OrderBy(r => r.Date)
                .ToList();
        }

        #endregion
    }
}
