using S3_HealthAxisApi.DTOs.Doctor;

namespace S3_HealthAxisApi.Services.Interface
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllAsync(string? sortBy, int? specialisation);

        Task<IEnumerable<DoctorDto>> GetActiveBySpecialisationAsync(int specialisation);

        Task<DoctorDto?> GetByIdAsync(int id);

        Task<DoctorDto> CreateAsync(CreateDoctorDto dto);

        Task UpdateAsync(int id, UpdateDoctorDto dto);

        Task ActivateAsync(int id);

        Task DeactivateAsync(int id);
    }
}