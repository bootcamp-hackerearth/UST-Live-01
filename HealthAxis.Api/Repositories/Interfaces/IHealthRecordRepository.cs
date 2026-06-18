using HealthAxisCore_Api.Models;

namespace HealthAxisCore_Api.Repositories.Interfaces
{
    public interface IHealthRecordRepository : IRepository<HealthRecord>
    {
        Task<List<HealthRecord>> GetByPatientIdAsync(int patientId, CancellationToken ct = default);
        Task<HealthRecord?> GetDetailsAsync(int healthRecordId, CancellationToken ct = default);
    }
}
