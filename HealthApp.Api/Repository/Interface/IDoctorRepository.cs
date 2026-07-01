using HealthApp.Api.Model;

namespace HealthApp.Api.Repository.Interface
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {

        Task<Doctor?> GetByIdentityUserIdAsync(string identityUserId);


        Task<(List<Doctor> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);

        Task<(List<Doctor> Items, int TotalCount)> GetActivePagedAsync(int pageNumber,int pageSize);

        Task<(List<Doctor> Items, int TotalCount)> SearchBySpecialisationPagedAsync
            ( string specialisation,int pageNumber,int pageSize);
    }
}
