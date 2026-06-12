using HealthApp.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.Service.Interface
{
    public interface IPatientApiService
    {
        Task<List<PatientDto>> GetAll();
        Task<PatientDto> GetById(int id);
        Task Create(PatientDto dto);
        Task Update(int id, PatientDto dto);
    }
}