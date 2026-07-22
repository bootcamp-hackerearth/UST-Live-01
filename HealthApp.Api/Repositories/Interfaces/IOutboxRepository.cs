using HealthApp.Api.Models;

namespace HealthApp.Api.Repositories.Interfaces;

public interface IOutboxRepository
{
    Task AddAsync(
        OutboxMessage message,
        CancellationToken ct = default);

    Task<List<OutboxMessage>> GetEligibleMessagesAsync(
        DateTime nowUtc,
        DateTime staleProcessingCutoffUtc,
        int maximumRetryCount,
        int batchSize,
        CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
