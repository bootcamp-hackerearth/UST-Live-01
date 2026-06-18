using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Interfaces;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class PatientService(
        IPatientRepository repository,
        IMapper mapper
    ) : IPatientService
    {
        public async Task<List<PatientDto>> GetAllAsync(
            CancellationToken ct = default
        ) =>
            mapper.Map<List<PatientDto>>(
                await repository.GetAllAsync(ct)
            );

        public async Task<PatientDto> GetByIdAsync(
            int id,
            CancellationToken ct = default
        ) =>
            mapper.Map<PatientDto>(
                await repository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException(
                    "Patient not found"
                )
            );

        public async Task<PatientDto> UpdatePatientAsync(
            int id,
            UpdatePatientDto request,
            CancellationToken ct = default
        )
        {
            var patient = await repository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException(
                    "Patient not found"
                );

            mapper.Map(request, patient);

            var updated = await repository.UpdateAsync(
                id,
                patient,
                ct
            ) ?? throw new NotFoundException(
                "Patient not found"
            );

            return mapper.Map<PatientDto>(updated);
        }

        public async Task<List<HealthRecordDto>> GetHealthRecordsAsync(
            int id,
            CancellationToken ct = default
        ) =>
            mapper.Map<List<HealthRecordDto>>(
                await repository.GetHealthRecordsAsync(
                    id,
                    ct
                )
            );
    }
}