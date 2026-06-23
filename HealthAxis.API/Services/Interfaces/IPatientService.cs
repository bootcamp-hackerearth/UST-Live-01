using HealthAxis.Shared.DTO.HealthRecordDtos;
using HealthAxis.Shared.DTO.PatientDtos;

namespace HealthAxis.API.Services
{
    public interface IPatientService
    {
   
        Task<List<PatientDto>> GetAllAsync();

        Task<PatientDto?> GetByIdAsync(int id);
        Task<PatientDto?> GetByUserIdAsync(string userId);
        Task<PatientDto?> UpdateAsync( int id, UpdatePatientDto patientDto);
        Task<List<PatientDto>> GetPatientsForDoctorAsync(int doctorId);
        Task<PatientDto?> GetPatientForDoctorAsync(int doctorId, int patientId);
        Task<List<HealthRecordDto>> GetHealthRecordsByPatientIdAsync(int patientid);
    }
}