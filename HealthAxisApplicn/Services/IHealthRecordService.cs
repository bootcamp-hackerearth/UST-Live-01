using HealthAxisApplicn.Dto.HealthRecords;
using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Services
{
    public interface IHealthRecordService
    {
        Task<List<HealthRecordDto>> GetAllAsync();
        Task<HealthRecordDto?> GetByIdAsync(int id);
        Task<HealthRecordDto> CreateAsync(CreateHealthRecordDto entity);
        Task<List<HealthRecordDto>> GetRecordsByPatientIdAsync(int patientId);
        Task<List<HealthRecordDto>> GetRecordsByDoctorIdAsync(int doctorId);
        Task<List<HealthRecordDto>> GetRecordsByPatientNameAsync(string patientName);
        Task<List<HealthRecordDto>> GetRecordsByDoctorNameAsync(string doctorName);

    }
}
