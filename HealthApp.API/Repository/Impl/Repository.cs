using HealthApp.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.API.Repository.Impl
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;

        public Repository(DbContext context) 
        {
            _context = context;
        }
        public async Task<T> AddAsync(T entity, CancellationToken ct = default)
        {
            await _context.Set<T>().AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<List<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Set<T>().ToListAsync(ct);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Set<T>().FindAsync([id], ct);
        }

        public async Task<T?> UpdateAsync(int id, T entity, CancellationToken ct = default)
        {
            var existing = await _context.Set<T>().FindAsync([id], ct);
            if (existing is null) return null;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync(ct);
            return existing;
        }
    }
}
