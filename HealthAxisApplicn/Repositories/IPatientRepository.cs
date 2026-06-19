using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Repositories
{
    public interface IPatientRepository: IRepository<Patient>
    {
        Task<List<Patient>> SearchByNameAsync(string name, CancellationToken ct = default);
        Task<Patient?> SearchByPhoneNumberAsync(string phoneNumber, CancellationToken ct = default);
        Task<Patient?> SearchByEmailAsync(string email, CancellationToken ct = default);
    }
}
