using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Models;
using Microsoft.EntityFrameworkCore;
using HealthCare.Api.Data;

namespace HealthCare.Api.Repositories.Implementations
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public readonly HealthCareDbContext _context;
        public DoctorRepository(HealthCareDbContext context) : base(context) { }

        public async Task<List<string>> GetSlots(int doctorId) =>
            await _context.AvailableSlots
                .Where(s => s.DoctorId == doctorId)
                .Select(s => s.TimeSlot)
                .ToListAsync();
    }


}
