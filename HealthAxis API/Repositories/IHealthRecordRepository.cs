using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories
{
    public interface IHealthRecordRepository : IRepository<HealthRecord>
    {
        Task<List<HealthRecord>> GetByPatientIdAsync(
            int patientId,
            CancellationToken ct = default);
    }
}
