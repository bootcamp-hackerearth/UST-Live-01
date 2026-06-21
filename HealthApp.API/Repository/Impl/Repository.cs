using HealthApp.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.API.Repository.Impl;

public class Repository<T>(DbContext context) : IRepository<T> where T : class
{
    public async Task<List<T>> GetAllAsync(CancellationToken ct = default)
        => await context.Set<T>().ToListAsync(ct);

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        => await context.Set<T>().FindAsync([id], ct);

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await context.Set<T>().AddAsync(entity, ct);
        await context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<T?> UpdateAsync(
        int id,
        T entity,
        CancellationToken ct = default)
    {
        var existing = await context.Set<T>().FindAsync([id], ct);

        if (existing is null)
            return null;

        context.Entry(existing).CurrentValues.SetValues(entity);
        await context.SaveChangesAsync(ct);

        return existing;
    }

    public async Task<T?> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = await context.Set<T>().FindAsync([id], ct);

        if (existing is null)
            return null;

        context.Set<T>().Remove(existing);
        await context.SaveChangesAsync(ct);

        return existing;
    }
}