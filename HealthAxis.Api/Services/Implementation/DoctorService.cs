using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Interfaces;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class DoctorService(
        IDoctorRepository repository,
        IMapper mapper
    ) : IDoctorService
    {
        public async Task<List<DoctorDto>> GetDoctorsAsync(
            string? specialisation,
            CancellationToken ct = default
        ) =>
            mapper.Map<List<DoctorDto>>(
                await repository.GetDoctorsAsync(
                    specialisation,
                    ct
                )
            );

        public async Task<DoctorDto> GetByIdAsync(
            int id,
            CancellationToken ct = default
        ) =>
            mapper.Map<DoctorDto>(
                await repository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException(
                    "Doctor not found"
                )
            );

        public async Task<List<string>> GetAvailabilityAsync(
            int id,
            DateTime date,
            CancellationToken ct = default
        )
        {
            if (date.Date < DateTime.UtcNow.Date)
            {
                throw new InvalidException(
                    "Cannot check past date"
                );
            }

            return await repository.GetAvailableSlotsAsync(
                id,
                date,
                ct
            );
        }
    }
}