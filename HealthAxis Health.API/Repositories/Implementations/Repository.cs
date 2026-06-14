using HealthAxisHealth.API.Data;
using HealthAxisHealth.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisHealth.API.Repositories.Implementations
{
    public class Repository<T> :
        IRepository<T>
        where T : class
    {
        #region Fields

        protected readonly ApplicationDbContext _context;

        protected readonly DbSet<T> _dbSet;

        #endregion

        #region Constructor

        public Repository(
            ApplicationDbContext context)
        {
            _context = context;

            _dbSet = context.Set<T>();
        }

        #endregion

        #region Methods

        public async Task<T?> GetByIdAsync(
            int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>>
            GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(
            T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(
            T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(
            T entity)
        {
            _dbSet.Remove(entity);
        }

        #endregion
    }
}