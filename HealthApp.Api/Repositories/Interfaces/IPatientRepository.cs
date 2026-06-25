using HealthApp.Api.Models;

namespace HealthApp.Api.Repositories.Interfaces
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<(IEnumerable<Patient> Items, int TotalCount)> GetPatientsAsync(
            string? name,
            string? email,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<bool> IsDuplicatePatient(
            string name,
            DateTime dob,
            string email,
            CancellationToken ct = default);
    }
}