using HealthApp.Api.Models;
using HealthApp.Shared.Enums;

namespace HealthApp.Api.Repositories.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<(IEnumerable<Doctor> Items, int TotalCount)> SearchDoctorsAsync(
            string? search,
            SpecialisationType? specialisation,
            bool? isActive,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<bool> ChangeStatusAsync(int id, bool isActive, CancellationToken ct = default);

        Task<bool> ExistsByEmailAsync(string email);
    }
}