using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Repositories.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(AppDbContext context) { _context = context; _dbSet = context.Set<T>(); }
        public async Task<List<T>> GetAllAsync(CancellationToken ct = default) => await _dbSet.ToListAsync(ct);
        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default) => await _dbSet.FindAsync(new object[] { id }, ct);
        public async Task<T> CreateAsync(T entity, CancellationToken ct = default) { await _dbSet.AddAsync(entity, ct); await _context.SaveChangesAsync(ct); return entity; }
        public virtual async Task<T?> UpdateAsync(int id, T entity, CancellationToken ct = default)
        {
            var existing = await GetByIdAsync(id, ct);
            if (existing == null) return null;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync(ct);
            return existing;
        }
        public async Task<T?> DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await GetByIdAsync(id, ct);
            if (existing == null) return null;
            _dbSet.Remove(existing);
            await _context.SaveChangesAsync(ct);
            return existing;
        }
        public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _context.SaveChangesAsync(ct);
    }
}
