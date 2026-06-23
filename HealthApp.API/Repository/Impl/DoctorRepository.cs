using HealthApp.API.Data;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.API.Repository.Impl;

public class DoctorRepository(HealthAppDbContext context)
    : Repository<Doctor>(context), IDoctorRepository
{
    public Task<List<Doctor>> GetAllActiveAsync(CancellationToken ct = default)
        => context.Doctors
            .Where(d => d.IsActive)
            .ToListAsync(ct);

    public Task<List<Doctor>> GetBySpecialisationAsync(
        SpecialisationType specialisation,
        CancellationToken ct = default)
        => context.Doctors
            .Where(d => d.Specialisation == specialisation.ToString())
            .ToListAsync(ct);

    public Task<List<Doctor>> GetActiveBySpecialisationAsync(
        SpecialisationType specialisation,
        CancellationToken ct = default)
        => context.Doctors
            .Where(d => d.IsActive &&
                        d.Specialisation == specialisation.ToString())
            .ToListAsync(ct);

    public Task<Doctor?> GetByUserIdAsync(
        string userId,
        CancellationToken ct = default)
        => context.Doctors
            .FirstOrDefaultAsync(d => d.UserId == userId, ct);
}