using HealthCare.Shared;
using HealthCare.Shared.DTOs.Patient;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace HealthCare.Web.Services.Interfaces
{
    public interface IPatientService
    {
        Task<PagedResult<PatientDto>> GetPatientsAsync(string searchTerm, int pageNumber, int pageSize);
        Task<PagedResult<PatientDto>> GetByIdAsync(int id);
        Task<bool> CreateAsync(PatientDto patient);
        Task<bool> UpdateAsync(PatientDto dto);
        Task<bool> DeleteAsync(int id);

    }
}