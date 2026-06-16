using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Enums;

namespace HealthAxisCore_Api.Repositories
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> GetDoctors(string? name, SpecialisationType? specialization, bool? isActive);

        Task SetStatus(int doctorId, bool status);

        Task<bool> IsDoctorAvailable(int doctorId, DateTime date);
    }
}