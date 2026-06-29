using Healthcare.Shared.DTOs;
using Healthcare.Shared.DTOs.Patient;
using System.Linq.Expressions;

namespace HealthCare.Api.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T entity, CancellationToken ct = default);
        Task UpdateAsync(T entity, CancellationToken ct = default);
        Task DeleteAsync(int id);
        Task<T?> GetProfileAsync(int id);
        //Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);

        Task<PagedResult<T>> GetAllAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? predicte = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
            );
    }
}
