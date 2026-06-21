using HealthApp.API.Data;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.API.Repository.Impl;

public class RefreshTokenRepository(HealthAppDbContext context)
    : IRefreshTokenRepository
{
    public async Task AddAsync(
        RefreshToken token,
        CancellationToken ct = default)
    {
        await context.RefreshTokens.AddAsync(token, ct);
        await context.SaveChangesAsync(ct);
    }

    public Task<RefreshToken?> GetByTokenAsync(
        string token,
        CancellationToken ct = default)
        => context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token, ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);
}