namespace HealthApp.Api.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
        Task<T> GetByIdAsync(int id, CancellationToken ct = default);
        Task<T> Add(T entity, CancellationToken ct = default);
        Task<T?> Update(int id,T entity, CancellationToken ct = default);
    }
}
