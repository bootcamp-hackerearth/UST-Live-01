using HealthApp.Api.Data;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repositories.Impl;

public class OutboxRepository : IOutboxRepository
{
    private readonly HealthAppDbContext _context;

    public OutboxRepository(HealthAppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        OutboxMessage message,
        CancellationToken ct = default)
    {
        await _context.OutboxMessages.AddAsync(message, ct);
    }

    public async Task<List<OutboxMessage>> GetEligibleMessagesAsync(
        DateTime nowUtc,
        DateTime staleProcessingCutoffUtc,
        int maximumRetryCount,
        int batchSize,
        CancellationToken ct = default)
    {
        return await _context.OutboxMessages
            .Where(message =>
                message.RetryCount < maximumRetryCount &&
                ((message.Status == OutboxMessageStatuses.Pending &&
                  (!message.NextAttemptAtUtc.HasValue ||
                   message.NextAttemptAtUtc <= nowUtc)) ||
                 (message.Status == OutboxMessageStatuses.Failed &&
                  (!message.NextAttemptAtUtc.HasValue ||
                   message.NextAttemptAtUtc <= nowUtc)) ||
                 (message.Status == OutboxMessageStatuses.Processing &&
                  message.LastAttemptAtUtc < staleProcessingCutoffUtc)))
            .OrderBy(message => message.CreatedAtUtc)
            .Take(batchSize)
            .ToListAsync(ct);
    }

    public async Task SaveChangesAsync(
        CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}