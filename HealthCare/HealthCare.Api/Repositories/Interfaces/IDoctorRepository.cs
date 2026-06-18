using HealthCare.Api.Models;


namespace HealthCare.Api.Repositories.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<List<string>> GetSlots(int doctorId);
    }
}
