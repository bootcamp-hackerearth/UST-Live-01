//using HealthCareApi.Data.Context;
using HealthCareApi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCareApi.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly HealthAppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(HealthAppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();

            }
            catch (DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => $"Property: {e.PropertyName}, Error: {e.ErrorMessage}");

                throw new Exception(string.Join(" | ", errors));
            }
            //await Task.CompletedTask; // EF6 doesn't have async Add
        }

        public async Task UpdateAsync(T entity)
        {
            await _context.SaveChangesAsync();
            //await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }
    }
}