using HealthApp.Api.Models;

namespace HealthApp.Api.Repositories.Interfaces
{
    public interface IHealthRecordRepository : IRepository<HealthRecord>
    {

        Task<IEnumerable<HealthRecord>> GetHealthRecordsAsync(
            int? patientId = null,
            int? appointmentId = null);
    }
}
