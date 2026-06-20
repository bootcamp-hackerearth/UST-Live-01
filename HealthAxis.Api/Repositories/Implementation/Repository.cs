using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Repositories.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;

            _dbSet = context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(
            CancellationToken ct = default)
        {
            return await _dbSet
                .ToListAsync(ct);
        }

        public async Task<T?> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(
                new object[] { id },
                ct);
        }

        public async Task<T> CreateAsync(
            T entity,
            CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);

            await _context.SaveChangesAsync(ct);

            return entity;
        }

        public virtual async Task<T?> UpdateAsync(
            int id,
            T entity,
            CancellationToken ct = default)
        {
            var existing = await GetByIdAsync(id, ct);

            if (existing == null)
            {
                return null;
            }

            _context.Entry(existing).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync(ct);

            return existing;
        }

        public async Task<T?> DeleteAsync(
            int id,
            CancellationToken ct = default)
        {
            var existing = await GetByIdAsync(id, ct);

            if (existing == null)
            {
                return null;
            }

            _dbSet.Remove(existing);

            await _context.SaveChangesAsync(ct);

            return existing;
        }

        public async Task<int> SaveChangesAsync(
            CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<int> CountAsync(
            CancellationToken ct = default)
        {
            return await _dbSet.CountAsync(ct);
        }

        public async Task<List<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            return await _dbSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }
    }
}