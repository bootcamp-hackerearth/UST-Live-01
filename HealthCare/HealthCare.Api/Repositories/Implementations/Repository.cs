using HealthCare.Api.Data;
using HealthCare.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HealthCare.Api.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(HealthCareDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity,CancellationToken ct=default)
        {
            await _dbSet.AddAsync(entity,ct);
            return entity;
        }

        public Task UpdateAsync( T entity,CancellationToken ct=default)
        {
            _dbSet.Update(entity);
       
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is not null)
                _dbSet.Remove(entity);
        }

        public  async Task<T?>GetByIdAsync(int id)=>
             await _dbSet.FindAsync(id);
       


        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct=default)
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }


        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        //Pagination
    //    public virtual async Task<PagedResult<T>> GetPagedAsync(
    //int pageNumber,
    //int pageSize,
    //Expression<Func<T, bool>>? filter = null,
    //CancellationToken ct = default)
    //    {
    //        IQueryable<T> query = _dbSet.AsNoTracking();

    //        if (filter != null)
    //            query = query.Where(filter);

    //        var totalCount = await query.CountAsync(ct);

    //        var items = await query
    //            .Skip((pageNumber - 1) * pageSize)
    //            .Take(pageSize)
    //            .ToListAsync(ct);

    //        return new PagedResult<T>
    //        {
    //            Items = items,
    //            TotalCount = totalCount,
    //            PageNumber = pageNumber,
    //            PageSize = pageSize
    //        };
    //    }


    }
}
