using HealthApp.Api.Data;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repositories.Impl
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        private readonly HealthAppDbContext _context;
        public DoctorRepository(HealthAppDbContext context) : base(context)
        {
            _context=context;
        }

        public async Task<bool> ChangeStatusAsync(int id, bool isActive, CancellationToken ct = default)
        {
            var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.DoctorId == id,ct);

            if (doctor == null)
                return false;

            doctor.IsActive = isActive;

            return await _context.SaveChangesAsync(ct)>0;
        }

        public async Task<(IEnumerable<Doctor> Items, int TotalCount)> SearchDoctorsAsync(
    string? search,
    SpecialisationType? specialisation,
    bool? isActive,
    int pageNumber,
    int pageSize,
    CancellationToken ct = default)
        {
            var query = _context.Doctors
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d =>
                    d.FullName!.Contains(search));
            }

            if (specialisation.HasValue)
            {
                query = query.Where(d =>
                    d.Specialisation == specialisation.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(d =>
                    d.IsActive == isActive.Value);
            }

            var totalCount = await query.CountAsync(ct);

            query = query.OrderBy(d => d.DoctorId);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Doctors
                .AnyAsync(doctor => doctor.DoctorEmail == email);
        }

    }
}
