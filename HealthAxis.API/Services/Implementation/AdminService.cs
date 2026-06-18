using AutoMapper;
using HealthAxis.API.DTO;
using HealthAxis.API.Enums;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.DTO.DoctorDto;

namespace HealthAxis.API.Services.Implementation
{
    public class AdminService(
        IDoctorRepository doctorRepository,
        IAppointmentRepository appointmentRepository,
        IMapper mapper) : IAdminService
    {
        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await doctorRepository.GetAllAsync();

            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> AddDoctorAsync(DoctorDto doctorDto)
        {
            ValidateDoctor(doctorDto);

            var doctor = mapper.Map<Doctor>(doctorDto);

            doctor.DoctorId = 0;

            var savedDoctor =
                await doctorRepository.AddAsync(doctor);

            return mapper.Map<DoctorDto>(savedDoctor);
        }

        public async Task<DoctorDto> UpdateDoctorAsync(
            int id,
            DoctorDto doctorDto)
        {
            var existingDoctor =
                await doctorRepository.GetByIdAsync(id);

            if (existingDoctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            ValidateDoctor(doctorDto);

            var doctor = mapper.Map<Doctor>(doctorDto);

            doctor.DoctorId = id;

            var updatedDoctor =
                await doctorRepository.UpdateAsync(id, doctor);

            return mapper.Map<DoctorDto>(updatedDoctor);
        }

        public async Task<List<AdminDto>> GetAppointmentReportsAsync()
        {
            var appointments =
                await appointmentRepository.GetAllAsync();

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
    }
}