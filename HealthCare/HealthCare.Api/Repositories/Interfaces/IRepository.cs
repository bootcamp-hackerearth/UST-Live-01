namespace HealthCare.Api.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task <T> CreateAsync(T entity);
        Task <T> UpdateAsync(int id,T entity);
        Task <int> DeleteAsync(int id);
        Task <T> GetByIdAsync(int id);
        Task <T> GetByNameAsync(string name);

        Task<T> GetAllAsync();

    }
}
