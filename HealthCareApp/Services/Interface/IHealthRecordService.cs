using HealthCareApp.Dtos;
using SharedClasses.Dtos;

namespace HealthCareApp.Services
{
    public interface IHealthRecordService
    {
        Task<List<HealthRecordDto>> GetAllHealthRecordsAsync();

        Task<HealthRecordDto> GetHealthRecordByIdAsync(int healthRecordId);

        Task<List<HealthRecordDto>> GetHealthRecordsByPatientIdAsync(int patientId);

        Task<List<HealthRecordDto>> GetHealthRecordsByDoctorIdAsync(int doctorId);

        Task<List<HealthRecordDto>> GetHealthRecordsByAppointmentIdAsync(int appointmentId);

        Task<HealthRecordDto> AddHealthRecordAsync(AddHealthRecordDto dto);

        Task<HealthRecordDto> UpdateHealthRecordAsync(int healthRecordId, UpdateHealthRecordDto dto);

        Task<HealthRecordDto> DeleteHealthRecordAsync(int healthRecordId);

        Task<List<HealthRecordDto>> GetMyHealthRecordsForPatientAsync(string identityUserId);

        Task<HealthRecordDto> GetHealthRecordByIdForPatientAsync(int healthRecordId,string identityUserId);
    }
}