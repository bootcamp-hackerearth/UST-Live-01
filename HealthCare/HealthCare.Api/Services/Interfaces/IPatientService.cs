using Healthcare.Shared.DTOs;
using Healthcare.Shared.DTOs.Patient;
using HealthCare.Api.Models;
namespace HealthCare.Api.Services.Interfaces
{
    public interface IPatientService
    {
        Task<PatientListDto> GetByIdAsync(int id);
        Task<PagedResult<PatientListDto>> GetAllAsync(PatientFilter filter);
        Task AddAsync(CreatePatientDto dto);
        Task UpdateAsync(int id,UpdatePatientDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<PatientListDto>> SearchByNameAsync(string name);
        Task UpdateStatusAsync(int id, bool isActive);
       
    }
}
