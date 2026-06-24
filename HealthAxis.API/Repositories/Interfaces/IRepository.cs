using HealthAxis.Shared.DTO.CommonDtos;
using System.Linq.Expressions;

namespace HealthAxis.API.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(CancellationToken ct = default);

        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<T> AddAsync(T entity, CancellationToken ct = default);

        Task<T?> UpdateAsync(int id, T entity, CancellationToken ct = default);

        Task<T?> DeleteAsync(int id, CancellationToken ct = default);

        Task<int> CountAsync(CancellationToken ct = default);

        Task<List<T>> GetPagedAsync<TKey>(
            PaginationQueryDto paginationQuery,
            Expression<Func<T, TKey>> orderBy,
            bool descending = false,
            CancellationToken ct = default);
    }
}