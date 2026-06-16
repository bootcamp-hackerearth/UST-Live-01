namespace HealthApp.API.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(CancellationToken ct = default);
        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<T> AddAsync(T entity, CancellationToken ct = default);
        Task<T?> UpdateAsync(int id, T entity, CancellationToken ct = default);
    }
}
