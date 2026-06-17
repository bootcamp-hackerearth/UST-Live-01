using HealthApp.Api.Data;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repository.Impl
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HealthAppDbContext _context;

        public GenericRepository(HealthAppDbContext context)
        {
            _context = context;
        }
        public async Task<T> addasync(T entity, CancellationToken cd = default)
        {
            await _context.Set<T>().AddAsync(entity, cd);
            await _context.SaveChangesAsync(cd);
            return entity;
        }


        public async Task<List<T>?> getallasync(CancellationToken cd = default)
        {
            var exiting = await _context.Set<T>().ToListAsync(cd);
            if (exiting == null) return null;
            return exiting;
        }

        public async Task<T?> getbyidasync(int id, CancellationToken cd = default)
        {
            var exiting= await _context.Set<T>().FindAsync([id], cd);
            if(exiting == null) return null;
            return exiting;
        }

        public async Task<T?> getbynameasync(string name, CancellationToken cd = default)
        {
            var exiting = await _context.Set<T>().FindAsync([name], cd);
            if (exiting == null) return null;
            return exiting;
        }       
        public async Task<T?> updateasync(int id, T entity, CancellationToken cd = default)
        {
            var exiting = await _context.Set<T>().FindAsync([id], cd);
            if (exiting == null) return null;

            _context.Entry(exiting).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync(cd);
            return exiting;
        }
    }
}
