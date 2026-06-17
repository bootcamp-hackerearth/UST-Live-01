using HealthApp.Api.Model;

namespace HealthApp.Api.Repository.Interface
{
    public interface IHealthRecordRepository : IGenericRepository<HealthRecord>
    {
        Task<HealthRecord?> GetHealthRecordsByDoctorAndPatientAsync(int? doctorId, int? patientId);


    }
}
