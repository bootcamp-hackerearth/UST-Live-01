using HealthAxisApplicn.Models;
using HealthAxisApplicn.Models.Dto;

namespace HealthAxisApplicn.Services
{
    public interface IHealthRecordService
    {
        Task<List<HealthRecordDto?>> GetAllAsync();
        Task<HealthRecordDto?> GetByIdAsync(int id);
        Task<HealthRecordDto> CreateAsync(HealthRecordDto entity);
        Task<HealthRecordDto?> UpdatebyAsync(int id, HealthRecordDto entity);
        Task<List<HealthRecordDto>> GetRecordByPatientIDAsync(int patientId);
        Task<List<HealthRecordDto>> GetRecordsByDoctorIDAsync(int doctorId);
        Task<List<HealthRecordDto>> GetRecordsByPatientNameAsync(string patientName);
        Task<List<HealthRecordDto>> GetRecordsByDoctorNameAsync(string doctorName);

    }
}
