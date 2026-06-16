using HealthAxisCore_Api.DTOs.Patient;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientResponseDTO>> GetAllAsync();

        Task<PatientResponseDTO?> GetByIdAsync(int id);

        Task<PatientResponseDTO> CreateAsync(CreatePatientDTO dto);

        Task<bool> UpdateAsync(int id, UpdatePatientDTO dto);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<PatientResponseDTO>> SearchAsync(string? name, string? email);
    }
}