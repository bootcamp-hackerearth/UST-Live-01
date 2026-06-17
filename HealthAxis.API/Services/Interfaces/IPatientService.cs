using HealthAxis.API.DTO;
using HealthAxis.DTO.HealthRecordDto;

namespace HealthAxis.API.Services
{
    public interface IPatientService
    {
   
        Task<List<PatientDto>> GetAllAsync();

        Task<PatientDto?> GetByIdAsync(int id);

        Task<PatientDto?> UpdateAsync( int id, PatientDto patientDto);
        Task<List<HealthRecordDto>> GetHealthRecordsByPatientIdAsync(int patientid);
    }
}