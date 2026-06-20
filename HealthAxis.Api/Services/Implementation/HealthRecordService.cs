using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Extensions;
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
            CancellationToken ct = default)
        {
            var records = await healthRecordRepository.GetByPatientIdAsync(
                patientId,
                ct);

            if (user.IsAdmin())
            {
                return mapper.Map<List<HealthRecordDto>>(records);
            }

            if (user.IsPatient())
            {
                var loggedInPatientId = user.GetPatientId()
                    ?? throw new UnauthorizedException("PatientId claim missing");

                if (loggedInPatientId != patientId)
                {
                    throw new UnauthorizedException("You can view only your own health records");
                }

                return mapper.Map<List<HealthRecordDto>>(records);
            }

            if (user.IsDoctor())
            {
                var doctorId = user.GetDoctorId()
                    ?? throw new UnauthorizedException("DoctorId claim missing");

                var doctorRecords = records
                    .Where(record => record.DoctorId == doctorId)
                    .ToList();

                return mapper.Map<List<HealthRecordDto>>(doctorRecords);
            }

            throw new UnauthorizedException("Unauthorized access");
        }

        public async Task<HealthRecordDto> GetByIdAsync(
            int id,
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            var record = await healthRecordRepository.GetDetailsAsync(id, ct)
                ?? throw new NotFoundException("Health record not found");

            if (user.IsAdmin())
            {
                return mapper.Map<HealthRecordDto>(record);
            }

            EnsureCanAccessHealthRecord(record, user);

            return mapper.Map<HealthRecordDto>(record);
        }

        public async Task<HealthRecordDto> CreateAsync(
            CreateHealthRecordDto request,
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            var doctorId = user.GetDoctorId()
                ?? throw new UnauthorizedException("DoctorId claim missing");

            var appt = await appointmentRepository.GetDetailsAsync(
                request.AppointmentId,
                ct)
                ?? throw new NotFoundException("Appointment not found");

            if (appt.DoctorId != doctorId)
            {
                throw new UnauthorizedException("Cannot complete another doctor's appointment");
            }

            var record = mapper.Map<HealthRecord>(request);

            record.DoctorId = doctorId;
            record.VisitDate = DateTime.UtcNow;

            appt.Status = "Completed";

            await appointmentRepository.UpdateAsync(
                appt.AppointmentId,
                appt,
                ct);

            var saved = await healthRecordRepository.CreateAsync(
                record,
                ct);

            return mapper.Map<HealthRecordDto>(
                await healthRecordRepository.GetDetailsAsync(
                    saved.HealthRecordId,
                    ct) ?? saved);
        }

        private static void EnsureCanAccessHealthRecord(
            HealthRecord record,
            ClaimsPrincipal user)
        {
            if (user.IsPatient())
            {
                var patientId = user.GetPatientId()
                    ?? throw new UnauthorizedException("PatientId claim missing");

                if (record.PatientId != patientId)
                {
                    throw new UnauthorizedException("You can view only your own health records");
                }

                return;
            }

            if (user.IsDoctor())
            {
                var doctorId = user.GetDoctorId()
                    ?? throw new UnauthorizedException("DoctorId claim missing");

                if (record.DoctorId != doctorId)
                {
                    throw new UnauthorizedException("You can view only health records written by you");
                }

                return;
            }

            throw new UnauthorizedException("Unauthorized access");
        }
    }
}