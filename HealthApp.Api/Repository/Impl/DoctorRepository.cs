using HealthApp.Api.Data;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repository.Impl
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly HealthAppDbContext _context;

        public DoctorRepository(HealthAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Doctor?> GetByIdentityUserIdAsync(string identityUserId)
        {
            return await _context.Set<Doctor>()
                .FirstOrDefaultAsync(d => d.IdentityUserId == identityUserId);
        }





        public async Task<(List<Doctor> Items, int TotalCount)> GetPagedAsync( int pageNumber,int pageSize)
        {
            var query = _context.Set<Doctor>().AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(d => d.DoctorId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(List<Doctor> Items, int TotalCount)> GetActivePagedAsync( int pageNumber,int pageSize)
        {
            var query = _context.Doctors
                .Where(d => d.IsActive == true);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(d => d.DoctorId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(List<Doctor> Items, int TotalCount)> SearchBySpecialisationPagedAsync
            ( string specialisation,int pageNumber, int pageSize)
        {
            var query = _context.Doctors
                .Where(d => d.Specialisation == specialisation);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(d => d.DoctorId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}