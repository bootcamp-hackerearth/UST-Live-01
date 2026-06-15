using HealthAxis.API.Data;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Repositories.Impl;

public class DoctorRepository : Repository<Doctor>, IDoctorRepository
{
    public DoctorRepository(HealthAxisDbContext context) : base(context)
    {
    }

    public async Task<List<Doctor>> GetAvailableDoctorsAsync()
    {
        return await _context.Doctors
            .Where(d => d.IsAvailable)
            .ToListAsync();
    }
}