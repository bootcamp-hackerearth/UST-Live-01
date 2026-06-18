using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Interfaces;
using System.Security.Claims;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class HealthRecordService(
        IHealthRecordRepository healthRecordRepository,
        IAppointmentRepository appointmentRepository,
        IMapper mapper
    ) : IHealthRecordService
    {
        public async Task<List<HealthRecordDto>> GetByPatientIdAsync(
            int patientId,
            ClaimsPrincipal user,
            CancellationToken ct = default
        ) =>
            mapper.Map<List<HealthRecordDto>>(
                await healthRecordRepository.GetByPatientIdAsync(
                    patientId,
                    ct
                )
            );

        public async Task<HealthRecordDto> GetByIdAsync(
            int id,
            ClaimsPrincipal user,
            CancellationToken ct = default
        ) =>
            mapper.Map<HealthRecordDto>(
                await healthRecordRepository.GetDetailsAsync(
                    id,
                    ct
                )
                ?? throw new NotFoundException(
                    "Health record not found"
                )
            );

        public async Task<HealthRecordDto> CreateAsync(
            CreateHealthRecordDto request,
            ClaimsPrincipal user,
            CancellationToken ct = default
        )
        {
            var doctorId = Convert.ToInt32(
                user.FindFirst("DoctorId")?.Value
            );

            var appt =
                await appointmentRepository.GetDetailsAsync(
                    request.AppointmentId,
                    ct
                )
                ?? throw new NotFoundException(
                    "Appointment not found"
                );

            if (appt.DoctorId != doctorId)
            {
                throw new UnauthorizedException(
                    "Cannot complete another doctor's appointment"
                );
            }

            var record = mapper.Map<HealthRecord>(request);

            record.DoctorId = doctorId;
            record.VisitDate = DateTime.UtcNow;

            appt.Status = "Completed";

            await appointmentRepository.UpdateAsync(
                appt.AppointmentId,
                appt,
                ct
            );

            var saved = await healthRecordRepository.CreateAsync(
                record,
                ct
            );

            return mapper.Map<HealthRecordDto>(
                await healthRecordRepository.GetDetailsAsync(
                    saved.HealthRecordId,
                    ct
                ) ?? saved
            );
        }
    }
}