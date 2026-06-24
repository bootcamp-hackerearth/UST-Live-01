using HealthCareApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Repository.Impl

{

    public class Repository<T> : IRepository<T> where T : class

    {

        private readonly DbContext _context;

        public Repository(DbContext context)

        {

            _context = context;

        }

        public async Task<T> CreateAsync(T entity, CancellationToken ct = default)

        {

            await _context.Set<T>().AddAsync(entity, ct);

            await _context.SaveChangesAsync(ct);

            return entity;

        }

        public async Task<T?> DeleteAsync(int id, CancellationToken ct = default)

        {

            var existing = await _context.Set<T>().FindAsync([id], ct);

            if (existing is null)

            {

                return null;

            }

            _context.Set<T>().Remove(existing);

            await _context.SaveChangesAsync(ct);

            return existing;

        }

        public async Task<List<T>> GetAllAsync(CancellationToken ct = default)

        {

            return await _context.Set<T>().ToListAsync(ct);

        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)

        {

            var existing = await _context.Set<T>().FindAsync([id], ct);

            return existing;

        }

        public async Task<T?> UpdateAsync(int id, T entity, CancellationToken ct = default)

        {

            var existing = await _context.Set<T>().FindAsync([id], ct);

            if (existing is null)

            {

                return null;

            }

            _context.Entry(existing).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync(ct);

            return existing;

        }

    }

}