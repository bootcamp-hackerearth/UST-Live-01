using HealthCare.Shared;
using HealthCare.Shared.DTOs.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HealthCare.Web.Services.Interfaces
{
    public interface IPatientService
    {
        Task<PagedResult<PatientDto>> GetPatientsAsync(string searchTerm, int pageNumber, int pageSize);
        Task<PatientDto> GetByIdAsync(int id);
        Task<bool> CreateAsync(PatientDto patient);
        Task<bool> UpdateAsync(PatientDto patient);
        Task<bool> DeleteAsync(int id);

    }
}