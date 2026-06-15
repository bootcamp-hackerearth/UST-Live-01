using S3_HealthAxisApi.Models;

namespace S3_HealthAxisApi.Repository.Interface
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync(string? sortBy, int? specialisation);
        Task<IEnumerable<Doctor>> GetActiveBySpecialisationAsync(int specialisation);
        Task<Doctor?> GetByIdAsync(int id);
        Task AddAsync(Doctor doctor);
        Task UpdateAsync(Doctor doctor);
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync();
    }
}
