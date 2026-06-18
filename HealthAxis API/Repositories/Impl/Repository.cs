using HealthAxis.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HealthAxis.API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly HealthAxisDbContext Context;
        protected readonly DbSet<T> DbSet;

        public Repository(HealthAxisDbContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await DbSet
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await DbSet.FindAsync(new object[] { id }, ct);
        }

        public async Task<T> CreateAsync(T entity, CancellationToken ct = default)
        {
            await DbSet.AddAsync(entity, ct);
            await Context.SaveChangesAsync(ct);

            return entity;
        }

        public async Task<T?> UpdateAsync(
            int id,
            T entity,
            CancellationToken ct = default)
        {
            T? existingEntity = await DbSet.FindAsync(new object[] { id }, ct);

            if (existingEntity == null)
            {
                return null;
            }

            Context.Entry(existingEntity)
                .CurrentValues
                .SetValues(entity);

            await Context.SaveChangesAsync(ct);

            return existingEntity;
        }

        public async Task<T?> DeleteAsync(int id, CancellationToken ct = default)
        {
            T? existingEntity = await DbSet.FindAsync(new object[] { id }, ct);

            if (existingEntity == null)
            {
                return null;
            }

            DbSet.Remove(existingEntity);
            await Context.SaveChangesAsync(ct);

            return existingEntity;
        }

        public async Task<bool> ExistsAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default)
        {
            return await DbSet.AnyAsync(predicate, ct);
        }

        public async Task<int> CountAsync(
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken ct = default)
        {
            if (predicate == null)
            {
                return await DbSet.CountAsync(ct);
            }

            return await DbSet.CountAsync(predicate, ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await Context.SaveChangesAsync(ct);
        }
    }
}

