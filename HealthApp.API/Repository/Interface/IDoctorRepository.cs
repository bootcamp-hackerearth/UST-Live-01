using HealthApp.API.Enums;
using HealthApp.API.Models;
namespace HealthApp.API.Repository.Interface;
public interface IDoctorRepository : IRepository<Doctor>
{
    Task<List<Doctor>> GetAllActiveAsync(CancellationToken ct = default);
    Task<List<Doctor>> GetBySpecialisationAsync(SpecialisationType specialisation, CancellationToken ct = default);
    Task<List<Doctor>> GetActiveBySpecialisationAsync(SpecialisationType specialisation, CancellationToken ct = default);
    Task<Doctor?> GetByUserIdAsync(string userId, CancellationToken ct = default);
}
