using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Models;
using Microsoft.EntityFrameworkCore;
using HealthCare.Api.Data;
using HealthCare.Api.Models;

namespace HealthCare.Api.Repositories.Implementations
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(HealthCareDbContext context) : base(context) { }


        //public async Task<List<Doctor>> GetBySpecializationAsync(string specialisation)
        //{
        //    return await _context.Doctors
        //        .AsNoTracking()
        //        .Where(d => d.Specialisation == specialisation && d.IsActive)
        //        .OrderBy(d => d.FullName)
        //        .ToListAsync();
        //}

        //public async Task AddRangeAsync(List<AvailableSlots> slots)
        //{
        //    await _context.AvailableSlots.AddRangeAsync(slots);
        //    await _context.SaveChangesAsync();
        //}

    }


}
