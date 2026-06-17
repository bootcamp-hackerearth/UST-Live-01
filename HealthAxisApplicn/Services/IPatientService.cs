using HealthAxisApplicn.Models;
using HealthAxisApplicn.Models.Dto;

namespace HealthAxisApplicn.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto?>> GetAllAsync();
        Task<PatientDto?> GetByIdAsync(int id);
        Task<PatientDto> CreateAsync(PatientDto entity);
        Task<PatientDto?> UpdatebyAsync(int id, PatientDto entity);
        Task<PatientDto?> SearchByPatientNameAsync(string name);
        Task<PatientDto?> SearchByPhoneNumberAsync(string phoneNumber);
        Task<PatientDto?> SearchByEmailAsync(string email);
        Task<PatientDto> DeactivatePatientAsync(int id);
    }
}
