using HealthCare.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HealthCareApi.Services.Interfaces
{
    public interface IPatientService
    {
        Task<Patient> GetPatientByIdAsync(int id);
        Task<PagedResult<Patient>> GetPaginatedPatientAsync(
           string searchTerm = null,
           int pageNumber = 1,
           int pageSize = 10);
        Task<Patient> AddPatientAsync(Patient patient);
        Task<Patient> UpdatePatientAsync(Patient updatedPatient);
        Task<bool> DeletePatientAsync(int id);

    }
}