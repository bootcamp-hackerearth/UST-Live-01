using HealthCare.Api.Data;
using Healthcare.Shared.DTOs;
using HealthCare.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HealthCare.Api.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly HealthCareDbContext _context;
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

        public  async Task<T>GetProfileAsync(int id)=>

             await _dbSet.FindAsync(id);

        public async Task<PagedResult<T>> GetAllAsync(
             int pageNumber,
             int pageSize,
             Expression<Func<T, bool>>? predicate = null,
             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }


    }
}
