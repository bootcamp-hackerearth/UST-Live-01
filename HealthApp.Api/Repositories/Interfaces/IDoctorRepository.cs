using HealthApp.Api.Models;
using HealthApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repositories.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {

        Task<IEnumerable<Doctor>> SearchDoctorsAsync(
            string? search,
            SpecialisationType? specialisation,
            bool? isActive,
            CancellationToken ct=default);
        Task<bool> ChangeStatusAsync(int id, bool isActive, CancellationToken ct = default);
        Task<bool> ExistsByEmailAsync(string email);

    }
}
