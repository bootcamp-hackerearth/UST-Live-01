using HealthAxis.API.Data;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Repositories.Impl;

public class PatientRepository : Repository<Patient>, IPatientRepository
{
    public PatientRepository(HealthAxisDbContext context) : base(context)
    {
    }

    public async Task<Patient?> GetByUserIdAsync(int userId)
    {
        return await _context.Patients.FirstOrDefaultAsync(x => x.UserId == userId);
    }
}