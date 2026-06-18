using HealthCare.Api.Models;


namespace HealthCare.Api.Repositories.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<Doctor?> GetByUserIdAsync(string userId);
        Task<List<string>> GetSlots(int doctorId);
    }
}
