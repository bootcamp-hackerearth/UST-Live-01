using S3_HealthAxis.Shared.DTOs.Doctor;

namespace S3_HealthAxisApi.Services.Interface
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllAsync(string? sortBy, int? specialisation);

        Task<IEnumerable<DoctorDto>> GetActiveBySpecialisationAsync(int specialisation);

        Task<DoctorDto?> GetByIdAsync(int id);

        Task<DoctorDto> CreateAsync(CreateDoctorDto dto);

        Task UpdateAsync(int id, UpdateDoctorDto dto);
        Task<IEnumerable<int>> GetAvailabilityAsync(int doctorId, DateOnly date);
        Task<DoctorCreationResultDto> CreateDoctorWithAccountAsync(CreateDoctorDto dto);

        Task ActivateAsync(int id);

        Task DeactivateAsync(int id);
    }
}