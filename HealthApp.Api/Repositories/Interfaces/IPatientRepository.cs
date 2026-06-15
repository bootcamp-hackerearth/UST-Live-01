using HealthApp.Api.Models;

namespace HealthApp.Api.Repositories.Interfaces
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<IEnumerable<Patient>> GetPatientsAsync(string Name, string email, CancellationToken ct = default);

        Task<bool> IsDuplicatePatient(string name, DateTime dob, string email, CancellationToken ct = default);
    }
}
