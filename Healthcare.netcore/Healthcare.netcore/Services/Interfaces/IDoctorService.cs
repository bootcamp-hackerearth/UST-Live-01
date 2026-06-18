using HealthAxis.API.DTOs;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllAsync(
            CancellationToken ct = default);

        Task<DoctorDto?> GetByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<IEnumerable<DoctorDto>>
            GetAvailableDoctorsAsync();

        Task<DoctorDto> AddAsync(CreateDoctorDto dto);

        Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto dto);

        Task<IEnumerable<DoctorDto>> SearchByNameAsync(string name);

        Task<IEnumerable<DoctorDto>> GetBySpecialisationAsync(string specialization);


    }
}