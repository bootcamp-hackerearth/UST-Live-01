using HealthCare.Api.Models;

namespace HealthCare.Api.Repositories.Interfaces
{
    public interface IHealthRecordRepository : IRepository<HealthRecord>
    {
        Task<List<HealthRecord>> GetHealthRecordByPatient(int id);
        Task<List<HealthRecord>> GetHealthRecordByAppointment(int id);
    }
}
