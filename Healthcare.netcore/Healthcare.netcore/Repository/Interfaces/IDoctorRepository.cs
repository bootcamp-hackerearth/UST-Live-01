using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories.Interfaces;

public interface IDoctorRepository : IRepository<Doctor>
{
    Task<List<Doctor>> GetAvailableDoctorsAsync();
}