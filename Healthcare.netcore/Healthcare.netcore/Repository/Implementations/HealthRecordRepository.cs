using HealthAxis.API.Data;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Repositories.Impl;

public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
{
    public HealthRecordRepository(HealthAxisDbContext context) : base(context)
    {
    }

    public async Task<List<HealthRecord>> GetByPatientIdAsync(int patientId)
    {
        return await _context.HealthRecords
            .Where(x => x.PatientId == patientId)
            .ToListAsync();
    }
}