using HealthAxis.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly HealthAxisDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(HealthAxisDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
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

        public async Task<T> CreateAsync(T entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);

            return entity;
        }

        public async Task<T?> UpdateAsync(
            int id,
            T entity,
            CancellationToken ct = default)
        {
            T? existingEntity = await _dbSet.FindAsync(
                new object[] { id },
                ct);

            if (existingEntity == null)
            {
                return null;
            }

            _context.Entry(existingEntity)
                .CurrentValues
                .SetValues(entity);

            await _context.SaveChangesAsync(ct);

            return existingEntity;
        }

        public async Task<T?> DeleteAsync(int id, CancellationToken ct = default)
        {
            T? existingEntity = await _dbSet.FindAsync(
                new object[] { id },
                ct);

            if (existingEntity == null)
            {
                return null;
            }

            _dbSet.Remove(existingEntity);
            await _context.SaveChangesAsync(ct);

            return existingEntity;
        }
    }
}