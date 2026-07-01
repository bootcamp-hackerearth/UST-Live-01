using HealthApp.Api.Model;

namespace HealthApp.Api.Repository.Interface
{
    public interface IHealthRecordRepository : IGenericRepository<HealthRecord>
    {
        Task<List<HealthRecord?>> GetHealthRecordsByDoctorAndPatientAsync(int? doctorId, int? patientId);

        Task<HealthRecord?> getbyidAsync(int id);
        Task<List<HealthRecord>?> getallAsync();


        Task<List<HealthRecord>> GetByPatientIdAsync(int patientId);
        Task<List<HealthRecord>> GetByDoctorIdAsync(int doctorId);
    }
}
