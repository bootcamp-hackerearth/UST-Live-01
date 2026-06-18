using HealthAxisCore_Api.Models;

namespace HealthAxisCore_Api.Repositories.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<List<Doctor>> GetDoctorsAsync(string? specialisation, CancellationToken ct = default);
        Task<List<string>> GetAvailableSlotsAsync(int doctorId, DateTime date, CancellationToken ct = default);
    }
}
