using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Repositories
{
    public interface IPatientRepository: IRepository<Patient>
    {
        Task<List<Patient>> SearchByNameAsync(string name, CancellationToken ct = default);
        Task<Patient?> SearchByPhoneNumberAsync(string phoneNumber, CancellationToken ct = default);
        Task<Patient?> SearchByEmailAsync(string email, CancellationToken ct = default);
        Task<List<Patient>> SearchAsync(string? name, string? phone, CancellationToken ct = default);
        Task<Patient?> GetByUserIdAsync(string userId);
    }
}
