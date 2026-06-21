using HealthApp.API.Models;
namespace HealthApp.API.Repository.Interface;
public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<bool> IsDuplicatePatientAsync(string patientName, string email, string phoneNumber, DateTime dateOfBirth, int? excludePatientId = null, CancellationToken ct = default);
}
