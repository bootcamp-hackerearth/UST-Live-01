using AutoMapper;
using HealthAxis.API.DTOs.Admin;
using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.DTOs.CommonDtos;
using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.DTOs.Patients;
using HealthAxis.API.Enums;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HealthAxis.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IHealthRecordRepository _healthRecordRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public AdminService(
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IHealthRecordRepository healthRecordRepository,
            IAppointmentRepository appointmentRepository,
            UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _healthRecordRepository = healthRecordRepository;
            _appointmentRepository = appointmentRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<PatientReadDto>> GetPatientsAsync(
            PaginationQueryDto pagination,
            CancellationToken ct = default)
        {
            List<Patient> patients =
                await _patientRepository.GetAllAsync(ct);

            List<Patient> orderedPatients =
                patients
                    .OrderBy(patient => patient.PatientId)
                    .ToList();

            int totalCount =
                orderedPatients.Count;

            List<Patient> pagedPatients =
                orderedPatients
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToList();

            List<PatientReadDto> patientDtos =
                _mapper.Map<List<PatientReadDto>>(pagedPatients);

            return new PagedResultDto<PatientReadDto>
            {
                Items = patientDtos,
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<PatientReadDto> UpdatePatientAsync(
            int patientId,
            PatientUpdateDto dto,
            CancellationToken ct = default)
        {
            Patient? patient =
                await _patientRepository.GetByIdAsync(patientId, ct);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found.");
            }

            _mapper.Map(dto, patient);

            await _patientRepository.SaveChangesAsync(ct);

            return _mapper.Map<PatientReadDto>(patient);
        }

        public async Task<PagedResultDto<DoctorReadDto>> GetDoctorsAsync(
            PaginationQueryDto pagination,
            CancellationToken ct = default)
        {
            List<Doctor> doctors =
                await _doctorRepository.GetAllAsync(ct);

            List<Doctor> orderedDoctors =
                doctors
                    .OrderBy(doctor => doctor.DoctorId)
                    .ToList();

            int totalCount =
                orderedDoctors.Count;

            List<Doctor> pagedDoctors =
                orderedDoctors
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToList();

            List<DoctorReadDto> doctorDtos =
                _mapper.Map<List<DoctorReadDto>>(pagedDoctors);

            return new PagedResultDto<DoctorReadDto>
            {
                Items = doctorDtos,
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<DoctorReadDto> CreateDoctorAsync(
            AdminDoctorCreateDto dto,
            CancellationToken ct = default)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                throw new BadRequestException(
                    "Password and confirm password do not match.");
            }

            IdentityUser? existingUser =
                await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                throw new ConflictException(
                    "Email already exists.");
            }

            Doctor doctor = new()
            {
                FullName = dto.FullName,
                Specialisation = dto.Specialisation,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = dto.IsActive
            };

            Doctor createdDoctor =
                await _doctorRepository.CreateAsync(doctor, ct);

            IdentityUser doctorUser = new()
            {
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true
            };

            IdentityResult createUserResult =
                await _userManager.CreateAsync(
                    doctorUser,
                    dto.Password);

            if (!createUserResult.Succeeded)
            {
                await _doctorRepository.DeleteAsync(
                    createdDoctor.DoctorId,
                    ct);

                string errors = string.Join(
                    ", ",
                    createUserResult.Errors.Select(error =>
                        error.Description));

                throw new BadRequestException(errors);
            }

            IdentityResult roleClaimResult =
                await _userManager.AddClaimAsync(
                    doctorUser,
                    new Claim(ClaimTypes.Role, "Doctor"));

            if (!roleClaimResult.Succeeded)
            {
                await _userManager.DeleteAsync(doctorUser);

                await _doctorRepository.DeleteAsync(
                    createdDoctor.DoctorId,
                    ct);

                string errors = string.Join(
                    ", ",
                    roleClaimResult.Errors.Select(error =>
                        error.Description));

                throw new BadRequestException(errors);
            }

            IdentityResult referenceClaimResult =
                await _userManager.AddClaimAsync(
                    doctorUser,
                    new Claim(
                        "ReferenceId",
                        createdDoctor.DoctorId.ToString()));

            if (!referenceClaimResult.Succeeded)
            {
                await _userManager.DeleteAsync(doctorUser);

                await _doctorRepository.DeleteAsync(
                    createdDoctor.DoctorId,
                    ct);

                string errors = string.Join(
                    ", ",
                    referenceClaimResult.Errors.Select(error =>
                        error.Description));

                throw new BadRequestException(errors);
            }

            return _mapper.Map<DoctorReadDto>(createdDoctor);
        }

        public async Task<DoctorReadDto> UpdateDoctorAsync(
            int doctorId,
            DoctorUpdateDto dto,
            CancellationToken ct = default)
        {
            Doctor? doctor =
                await _doctorRepository.GetByIdAsync(doctorId, ct);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found.");
            }

            _mapper.Map(dto, doctor);

            await _doctorRepository.SaveChangesAsync(ct);

            return _mapper.Map<DoctorReadDto>(doctor);
        }

        public async Task<HealthRecordReadDto> UpdateHealthRecordAsync(
            int recordId,
            HealthRecordUpdateDto dto,
            CancellationToken ct = default)
        {
            HealthRecord? healthRecord =
                await _healthRecordRepository.GetByIdAsync(recordId, ct);

            if (healthRecord == null)
            {
                throw new NotFoundException("Health record not found.");
            }

            _mapper.Map(dto, healthRecord);

            await _healthRecordRepository.SaveChangesAsync(ct);

            return _mapper.Map<HealthRecordReadDto>(healthRecord);
        }

        public async Task<PagedResultDto<AppointmentReportDto>> GetAppointmentReportAsync(
            PaginationQueryDto pagination,
            CancellationToken ct = default)
        {
            List<Appointment> appointments =
                await _appointmentRepository.GetAllAsync(ct);

            List<AppointmentReportDto> appointmentReports =
                appointments
                    .GroupBy(appointment =>
                        appointment.ScheduledDate.Date)
                    .Select(group => new AppointmentReportDto
                    {
                        Date = group.Key,

                        TotalCount = group.Count(),

                        ScheduledCount = group.Count(appointment =>
                            appointment.Status == AppointmentStatus.Scheduled),

                        ConfirmedCount = group.Count(appointment =>
                            appointment.Status == AppointmentStatus.Confirmed),

                        CancelledCount = group.Count(appointment =>
                            appointment.Status == AppointmentStatus.Cancelled),

                        CompletedCount = group.Count(appointment =>
                            appointment.Status == AppointmentStatus.Completed)
                    })
                    .OrderBy(report =>
                        report.Date)
                    .ToList();

            int totalCount =
                appointmentReports.Count;

            List<AppointmentReportDto> pagedReports =
                appointmentReports
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToList();

            return new PagedResultDto<AppointmentReportDto>
            {
                Items = pagedReports,
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
    }
}
