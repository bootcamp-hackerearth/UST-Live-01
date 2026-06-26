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

            IEnumerable<Patient> query =
                patients
                    .OrderBy(patient => patient.PatientId);

            if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
            {
                string searchTerm =
                    pagination.SearchTerm.Trim();

                query = query.Where(patient =>
                    patient.PatientId.ToString().Contains(searchTerm) ||
                    patient.FullName.Contains(
                        searchTerm,
                        StringComparison.OrdinalIgnoreCase));
            }

            List<Patient> filteredPatients =
                query.ToList();

            int totalCount =
                filteredPatients.Count;

            List<Patient> pagedPatients =
                filteredPatients
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

            IEnumerable<Doctor> query =
                doctors.OrderBy(doctor => doctor.DoctorId);

            if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
            {
                string searchTerm =
                    pagination.SearchTerm.Trim();

                query = query.Where(doctor =>
                    doctor.DoctorId.ToString().Contains(searchTerm) ||
                    doctor.FullName.Contains(
                        searchTerm,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (pagination.Specialisation.HasValue &&
                Enum.IsDefined(
                    typeof(Specialisation),
                    pagination.Specialisation.Value))
            {
                Specialisation selectedSpecialisation =
                    (Specialisation)pagination.Specialisation.Value;

                query = query.Where(doctor =>
                    doctor.Specialisation == selectedSpecialisation);
            }

            List<Doctor> filteredDoctors =
                query.ToList();

            int totalCount =
                filteredDoctors.Count;

            List<Doctor> pagedDoctors =
                filteredDoctors
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

        public async Task<List<AppointmentDetailDto>> GetAppointmentDetailsByDateAsync(
            DateTime date,
            CancellationToken ct = default)
        {
            List<Appointment> appointments =
                await _appointmentRepository.GetAllAsync(ct);

            List<Patient> patients =
                await _patientRepository.GetAllAsync(ct);

            List<Doctor> doctors =
                await _doctorRepository.GetAllAsync(ct);

            List<AppointmentDetailDto> details =
                appointments
                    .Where(appointment =>
                        appointment.ScheduledDate.Date == date.Date)
                    .OrderBy(appointment =>
                        appointment.TimeSlot)
                    .Select(appointment =>
                    {
                        Patient? patient =
                            patients.FirstOrDefault(patient =>
                                patient.PatientId == appointment.PatientId);

                        Doctor? doctor =
                            doctors.FirstOrDefault(doctor =>
                                doctor.DoctorId == appointment.DoctorId);

                        return new AppointmentDetailDto
                        {
                            AppointmentId = appointment.AppointmentId,
                            Date = appointment.ScheduledDate.Date,
                            TimeSlot = appointment.TimeSlot,
                            Status = appointment.Status,
                            CancellationReason = appointment.CancellationReason,
                            PatientId = appointment.PatientId,
                            PatientName = patient?.FullName ?? "Unknown Patient",
                            DoctorId = appointment.DoctorId,
                            DoctorName = doctor?.FullName ?? "Unknown Doctor"
                        };
                    })
                    .ToList();

            return details;
        }

        public async Task<AppointmentDetailDto> ConfirmAppointmentAsync(
            int appointmentId,
            CancellationToken ct = default)
        {
            Appointment? appointment =
                await _appointmentRepository.GetByIdAsync(
                    appointmentId,
                    ct);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found.");
            }

            if (appointment.Status != AppointmentStatus.Scheduled)
            {
                throw new BadRequestException(
                    "Only scheduled appointments can be confirmed.");
            }

            appointment.Status = AppointmentStatus.Confirmed;
            appointment.CancellationReason = string.Empty;

            await _appointmentRepository.SaveChangesAsync(ct);

            List<AppointmentDetailDto> details =
                await GetAppointmentDetailsByDateAsync(
                    appointment.ScheduledDate.Date,
                    ct);

            return details.First(detail =>
                detail.AppointmentId == appointment.AppointmentId);
        }

        public async Task<AppointmentDetailDto> CancelAppointmentAsync(
            int appointmentId,
            CancelAppointmentDto dto,
            CancellationToken ct = default)
        {
            Appointment? appointment =
                await _appointmentRepository.GetByIdAsync(
                    appointmentId,
                    ct);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found.");
            }

            if (appointment.Status != AppointmentStatus.Scheduled)
            {
                throw new BadRequestException(
                    "Only scheduled appointments can be cancelled.");
            }

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.CancellationReason = dto.CancellationReason;

            await _appointmentRepository.SaveChangesAsync(ct);

            List<AppointmentDetailDto> details =
                await GetAppointmentDetailsByDateAsync(
                    appointment.ScheduledDate.Date,
                    ct);

            return details.First(detail =>
                detail.AppointmentId == appointment.AppointmentId);
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
