using S3_HealthAxisApi.Models;

namespace S3_HealthAxisApi.Repository.Interface
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task<IEnumerable<Patient>> SearchByNameAsync(string name);
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync();
    }
}
