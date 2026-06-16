using HealthAxisCore_Api.Models;

namespace HealthAxisCore_Api.Repositories
{
    public interface IHealthRecordRepository : IGenericRepository<HealthRecord>
    {
        Task<IEnumerable<HealthRecord>> GetByPatient(int patientId);
    }
}