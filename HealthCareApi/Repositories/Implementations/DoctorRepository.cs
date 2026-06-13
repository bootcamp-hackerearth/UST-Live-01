//using HealthCareApi.Data.Context;
using HealthCare.Shared;
using HealthCareApi.Repositories.Implementations;
using HealthCareApi.Repositories.Interfaces;
using HealthCareWebApi;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
//using HealthCareApi.Models;

namespace HealthCareApi.Repositories.Implementations
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(HealthAppDbContext _context) : base(_context)
        {
        }

        public async Task<PagedResult<Doctor>> GetDoctorsAsync(
            string specialization = null,
            string searchTerm = null,
            bool orderByDescending = false,
            int pageNumber = 1,
            int pageSize = 10)
        {
            // ✅ Safety
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize > 50 ? 50 : pageSize;

            // ✅ Base Query
            IQueryable<Doctor> query = _context.Doctors
                .Where(d => d.IsActive)
                .AsQueryable();

            // ✅ Filter
            if (!string.IsNullOrWhiteSpace(specialization))
            {
                query = query.Where(d =>
                    d.Specialisation.ToLower().Contains(specialization.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(d =>
                    d.FullName.ToLower().Contains(searchTerm.ToLower()));
            }

            // ✅ TOTAL COUNT (must be before pagination)
            int totalCount = await query.CountAsync();

            // ✅ Sorting
            query = orderByDescending
                ? query.OrderByDescending(d => d.YearsOfExperience)
                : query.OrderBy(d => d.YearsOfExperience);

            // ✅ Pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // ✅ Return full paged result
            return new PagedResult<Doctor>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

        }

        public async Task<List<Doctor>> GetBySpecializationAsync(string specialization)
        {
            return await _context.Doctors
                .Where(d => d.Specialisation == specialization && d.IsActive)
                .OrderBy(d => d.FullName)
                .ToListAsync();
        }

        public override async Task DeleteAsync(Doctor doctor)
        {
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor));

            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<DoctorAvailableSlot> slots)
        {
            _context.DoctorAvailableSlots.AddRange(slots);
            await _context.SaveChangesAsync();

        }
    }
}