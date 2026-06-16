using HealthAxisHealth.API.Data;
using HealthAxisHealth.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisHealth.API.Repositories.Implementations
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        #region Fields

        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        #endregion

        #region Constructor

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        #endregion

        #region Methods

        public async Task<T?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(
                new object[] { id },
                cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(
            T entity,
            CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public Task UpdateAsync(
            T entity,
            CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(
            T entity,
            CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        #endregion
    }
}
