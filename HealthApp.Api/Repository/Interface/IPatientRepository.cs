using HealthApp.Api.Model;

namespace HealthApp.Api.Repository.Interface
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient?> GetByIdentityUserIdAsync( string identityUserId, CancellationToken cd = default);

        Task<bool> EmailExistsAsync(string email,int? excludeId = null,CancellationToken cd = default);

        Task<(List<Patient> Items, int TotalCount)> GetPagedPatientsAsync(int pageNumber,
            int pageSize,string? search = null,CancellationToken cd = default);
    }
}