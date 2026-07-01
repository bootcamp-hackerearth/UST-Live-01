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
        public async Task<T> addAsync(T entity, CancellationToken cd = default)
        {
            await _context.Set<T>().AddAsync(entity, cd);
            await _context.SaveChangesAsync(cd);
            return entity;
        }


        public async Task<List<T>?> getallAsync(CancellationToken cd = default)
        {
            var exiting = await _context.Set<T>().ToListAsync(cd);
            if (exiting == null) return null;
            return exiting;
        }

        public async Task<T?> getbyidAsync(int id, CancellationToken cd = default)
        {
            var exiting= await _context.Set<T>().FindAsync([id], cd);
            if(exiting == null) return null;
            return exiting;
        }

        public async Task<T?> getbynameAsync(string name, CancellationToken cd = default)
        {
            var exiting = await _context.Set<T>().FindAsync([name], cd);
            if (exiting == null) return null;
            return exiting;
        }       
        public async Task<T?> updateAsync(int id, T entity, CancellationToken cd = default)
        {
            var exiting = await _context.Set<T>().FindAsync([id], cd);
            if (exiting == null) return null;

            _context.Entry(exiting).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync(cd);
            return exiting;
        }




        public async Task<(List<T> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cd = default)
        {
            var query = _context.Set<T>().AsQueryable();

            var totalCount = await query.CountAsync(cd);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cd);

            return (items, totalCount);
        }




    }
}
