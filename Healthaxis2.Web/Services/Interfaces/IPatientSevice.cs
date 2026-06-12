using System.Collections.Generic;
using System.Threading.Tasks;
using Healthaxis2.Shared.DTOs;

namespace Healthaxis2.Web.Services.Interfaces
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAll();
        Task<PatientDto> GetById(int id);
        Task<bool> Create(PatientDto dto);
        Task<bool> Update(int id, PatientDto dto);
        Task<bool> Delete(int id);
        Task<PatientDto> CreateAndReturn(PatientDto dto);
    }
}
