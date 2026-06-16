using HealthApp.Api.Dtos;
using HealthApp.Api.Enums;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IDoctorService
    {
            Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync(CancellationToken ct = default);
            Task<DoctorDto?> GetDoctorByIdAsync(int id, CancellationToken ct = default);
            Task<DoctorDto> CreateDoctorAsync(DoctorCreateDto dto, CancellationToken ct = default);
            Task<DoctorDto?> UpdateDoctorAsync(int id, DoctorCreateDto dto, CancellationToken ct = default);

            Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(string? search, SpecialisationType? specialization, bool? isActive, CancellationToken ct = default);

            Task<bool> ChangeDoctorStatusAsync(int id, bool isActive, CancellationToken ct = default);
    }
}
