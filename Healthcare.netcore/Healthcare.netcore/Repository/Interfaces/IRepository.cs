namespace HealthAxis.API.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(int id, T entity, CancellationToken cancellationToken);
        Task DeleteAsync(int id);
    }
}
