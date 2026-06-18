using HealthAxis.API.Data;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Repositories.Implementations
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        private readonly HealthAxisDbContext _context;

        public DoctorRepository(HealthAxisDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> SearchByNameAsync(string name)
        {
            return await _context.Doctors
                .Where(d => d.FullName.Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetBySpecialisationAsync(string specialization)
        {
            return await _context.Doctors
                .Where(d => d.Specialisation.ToString() == specialization)
                .ToListAsync();
        }

        public async Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync()
        {
            return await _context.Doctors
                .Where(d => d.IsActive)
                .ToListAsync();
        }
    }
}