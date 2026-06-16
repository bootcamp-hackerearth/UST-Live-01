using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Repositories
{
    public interface IDoctorRepository: IRepository<Doctor>
    {
        Task<List<Doctor>> SearchDoctorByNameAsync(string name, CancellationToken ct = default);
        Task<List<Doctor>> GetAvailableDoctorsAsync(CancellationToken ct = default);
        Task<List<Doctor>> SearchBySpecialisationAsync(string specialisation, CancellationToken ct = default);
    }
}
