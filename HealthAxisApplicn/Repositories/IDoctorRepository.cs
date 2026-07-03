using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Repositories
{
    public interface IDoctorRepository: IRepository<Doctor>
    {
        Task<List<Doctor>> SearchByNameAsync(string name, CancellationToken ct = default);
        Task<List<Doctor>> GetActiveDoctorsAsync(CancellationToken ct = default);
        Task<List<Doctor>> SearchBySpecialisationAsync(string specialisation, CancellationToken ct = default);
        Task<List<Doctor>> SearchAsync(string query, CancellationToken ct = default);
        Task<Doctor?> GetByUserIdAsync(string userId);
        Task<List<Doctor>> FilterAsync(string? name, string? specialization);

    }
}
