using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Interfaces;

namespace HealthCare_Appointment_Portal.Repositories
{
    public class Repository<T>
        : IRepository<T>
        where T : class
    {
        protected readonly
            ApplicationDbContext _context;

        protected readonly
            DbSet<T> _dbSet;

        public Repository(
            ApplicationDbContext context)
        {
            _context = context;

            _dbSet = context.Set<T>();
        }

        public virtual async Task<T>
            GetByIdAsync(
                int id)
        {
            return await _dbSet
                .FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>>
            GetAllAsync()
        {
            return await _dbSet
                .ToListAsync();
        }

        public virtual async Task AddAsync(
            T entity)
        {
            _dbSet.Add(entity);

            await Task.CompletedTask;
        }

        public virtual async Task UpdateAsync(
            T entity)
        {
            _context.Entry(entity)
                .State =
                EntityState.Modified;

            await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(
            int id)
        {
            T entity =
                await _dbSet.FindAsync(id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
    }
}