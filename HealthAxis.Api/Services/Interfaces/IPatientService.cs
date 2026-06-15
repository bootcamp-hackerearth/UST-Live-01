using HealthAxisCore_Api.Models.DTOs;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto> GetByIdAsync(int id);
        Task<PatientDto> AddPatientAsync(PatientDto entity);
        Task<PatientDto> UpdatePatientAsync(int id,PatientDto entity);
    }
}
