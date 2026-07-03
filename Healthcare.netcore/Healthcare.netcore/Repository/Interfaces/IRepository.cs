namespace HealthAxis.API.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(int id, T entity, CancellationToken cancellationToken);
        Task DeleteAsync(int id);
    }
}
