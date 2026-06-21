using HealthApp.API.Models;
namespace HealthApp.API.Repository.Interface;
public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token, CancellationToken ct = default);
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
