using HealthCareApp.Models;

namespace HealthCareApp.Repository.Interface
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<bool> IsDuplicatePatientAsync(
            string patientName,
            string email,
            string phoneNumber,
            DateTime dateOfBirth,
            int? excludePatientId = null,
            CancellationToken ct = default);
    }
}