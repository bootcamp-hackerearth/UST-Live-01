using System.Linq.Expressions;

namespace HealthAxis.API.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(CancellationToken ct = default);

        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<T> CreateAsync(T entity, CancellationToken ct = default);

        Task<T?> UpdateAsync(int id, T entity, CancellationToken ct = default);

        Task<T?> DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> ExistsAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default);

        Task<int> CountAsync(
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken ct = default);

        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
