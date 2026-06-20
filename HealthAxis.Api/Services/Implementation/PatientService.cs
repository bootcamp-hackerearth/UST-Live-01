using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Extensions;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Interfaces;
using System.Security.Claims;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class PatientService(
        IPatientRepository repository,
        IAppointmentRepository appointmentRepository,
        IHealthRecordRepository healthRecordRepository,
        IMapper mapper
    ) : IPatientService
    {
        public async Task<List<PatientDto>> GetAllAsync(
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            if (!user.IsAdmin())
            {
                throw new UnauthorizedException("Only admin can view all patients");
            }

            return mapper.Map<List<PatientDto>>(
                await repository.GetAllAsync(ct));
        }

        public async Task<PatientDto> GetByIdAsync(
            int id,
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            await EnsureCanAccessPatientProfileAsync(id, user, ct);

            var patient = await repository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Patient not found");

            return mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> UpdatePatientAsync(
            int id,
            UpdatePatientDto request,
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            if (!user.IsPatient() || user.GetPatientId() != id)
            {
                throw new UnauthorizedException("You can update only your own profile");
            }

            var patient = await repository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Patient not found");

            mapper.Map(request, patient);

            var updated = await repository.UpdateAsync(id, patient, ct)
                ?? throw new NotFoundException("Patient not found");

            return mapper.Map<PatientDto>(updated);
        }

        public async Task<List<HealthRecordDto>> GetHealthRecordsAsync(
            int id,
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            if (user.IsAdmin())
            {
                throw new UnauthorizedException("Admin cannot view health records");
            }

            if (user.IsPatient())
            {
                var patientId = user.GetPatientId()
                    ?? throw new UnauthorizedException("PatientId claim missing");

                if (patientId != id)
                {
                    throw new UnauthorizedException("You can view only your own health records");
                }

                var ownRecords = await repository.GetHealthRecordsAsync(id, ct);

                return mapper.Map<List<HealthRecordDto>>(ownRecords);
            }

            if (user.IsDoctor())
            {
                var doctorId = user.GetDoctorId()
                    ?? throw new UnauthorizedException("DoctorId claim missing");

                var records = await repository.GetHealthRecordsAsync(id, ct);

                var doctorRecords = records
                    .Where(record => record.DoctorId == doctorId)
                    .ToList();

                return mapper.Map<List<HealthRecordDto>>(doctorRecords);
            }

            throw new UnauthorizedException("Unauthorized access");
        }

        private async Task EnsureCanAccessPatientProfileAsync(
            int patientId,
            ClaimsPrincipal user,
            CancellationToken ct)
        {
            if (user.IsAdmin())
            {
                return;
            }

            if (user.IsPatient())
            {
                var loggedInPatientId = user.GetPatientId()
                    ?? throw new UnauthorizedException("PatientId claim missing");

                if (loggedInPatientId != patientId)
                {
                    throw new UnauthorizedException("You can view only your own patient profile");
                }

                return;
            }

            if (user.IsDoctor())
            {
                var doctorId = user.GetDoctorId()
                    ?? throw new UnauthorizedException("DoctorId claim missing");

                var appointments = await appointmentRepository.GetAppointmentsAsync(
                    patientId,
                    doctorId,
                    null,
                    ct);

                var healthRecords = await healthRecordRepository.GetByPatientIdAsync(
                    patientId,
                    ct);

                var hasTreatedPatient =
                    appointments.Any(appointment => appointment.DoctorId == doctorId) ||
                    healthRecords.Any(record => record.DoctorId == doctorId);

                if (!hasTreatedPatient)
                {
                    throw new UnauthorizedException("You can view only patients treated by you");
                }

                return;
            }

            throw new UnauthorizedException("Unauthorized access");
        }
    }
}