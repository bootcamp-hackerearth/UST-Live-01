using HealthAxisCore_Api.Models;

namespace HealthAxisCore_Api.Repositories.Interfaces
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient?> GetByEmailAsync(string email, CancellationToken ct = default);

        Task<List<HealthRecord>> GetHealthRecordsAsync(int patientId, CancellationToken ct = default);
    }
}