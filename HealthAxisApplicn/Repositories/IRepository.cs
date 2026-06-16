namespace HealthAxisApplicn.Repositories
{
    public interface IRepository<T> where T:class
    {
        Task<List<T?>> GetAllAsync(CancellationToken ct = default);
        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<T> CreateAsync(T entity, CancellationToken ct = default);
        Task<T?> UpdatebyAsync(int id, T entity, CancellationToken ct = default);
    }
}
