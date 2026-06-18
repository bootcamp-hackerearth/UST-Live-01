using HealthAxis.API.DTO.HealthRecordDtos;
using HealthAxis.API.DTO.PatientDtos;

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