using HealthApp.API.Models.DTOs;

namespace HealthApp.API.Service.Interface
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto> GetByIdAsync(int id);
        Task<PatientDto> AddPatientAsync(PatientDto entity);
        Task<PatientDto> UpdatePatientAsync(int id, PatientDto entity);
    }
}
