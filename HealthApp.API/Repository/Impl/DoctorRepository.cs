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
            .OrderBy(d => d.DoctorName)
            .ToListAsync(ct);

    public Task<List<Doctor>> GetBySpecialisationAsync(
        SpecialisationType specialisation,
        CancellationToken ct = default)
        => context.Doctors
            .Where(d => d.Specialisation == specialisation.ToString())
            .OrderBy(d => d.DoctorName)
            .ToListAsync(ct);

    public Task<List<Doctor>> GetActiveBySpecialisationAsync(
        SpecialisationType specialisation,
        CancellationToken ct = default)
        => context.Doctors
            .Where(d =>
                d.IsActive &&
                d.Specialisation == specialisation.ToString())
            .OrderBy(d => d.DoctorName)
            .ToListAsync(ct);

    public Task<Doctor?> GetByUserIdAsync(
        string userId,
        CancellationToken ct = default)
        => context.Doctors
            .FirstOrDefaultAsync(d => d.UserId == userId, ct);

    public async Task<(List<Doctor> Items, int TotalCount)> GetPagedAsync(
        int pageNumber = 1,
        int pageSize = 5,
        CancellationToken ct = default)
    {
        if (pageNumber < 1)
        {
            pageNumber = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 5;
        }

        var query = context.Doctors.AsQueryable();

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderBy(d => d.DoctorId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}