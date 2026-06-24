using HealthAxis.API.Data;
using HealthAxis.Shared.DTO.CommonDtos;
using HealthAxis.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HealthAxis.API.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> AddAsync(T entity, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _dbSet.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);

            return entity;
        }

        public async Task<T?> DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await _dbSet.FindAsync(new object[] { id }, ct);

            if (existing is null)
            {
                return null;
            }

            _dbSet.Remove(existing);
            await _context.SaveChangesAsync(ct);

            return existing;
        }

        public async Task<List<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, ct);
        }

        public async Task<T?> UpdateAsync(int id, T entity, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            var existing = await _dbSet.FindAsync(new object[] { id }, ct);

            if (existing is null)
            {
                return null;
            }

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync(ct);

            return existing;
        }

        public async Task<int> CountAsync(CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .CountAsync(ct);
        }

        public async Task<List<T>> GetPagedAsync<TKey>(
            PaginationQueryDto paginationQuery,
            Expression<Func<T, TKey>> orderBy,
            bool descending = false,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(paginationQuery);
            ArgumentNullException.ThrowIfNull(orderBy);

            var query = _dbSet.AsNoTracking();

            query = descending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);

            return await query
                .Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize)
                .Take(paginationQuery.PageSize)
                .ToListAsync(ct);
        }
    }
}