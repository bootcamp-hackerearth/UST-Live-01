using SharedClasses.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthcareWeb.Services
{
    public interface IPatientApiService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto> GetByIdAsync(int id);
        Task<PatientDto> AddAsync(CreatePatientDto dto);
        Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto);
        Task<PatientDto> DeleteAsync(int id);
    }
}