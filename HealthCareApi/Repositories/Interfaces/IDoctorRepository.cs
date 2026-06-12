using HealthCare.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCareApi.Repositories.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<PagedResult<Doctor>> GetDoctorsAsync(
            string specialization = null,
            string searchTerm = null,
            bool orderByDescending = false,
            int pageNumber = 1,
            int pageSize = 10
        );
        Task<List<Doctor>> GetBySpecializationAsync(string specialization);
        Task AddRangeAsync(List<DoctorAvailableSlot> slots);
    }
}