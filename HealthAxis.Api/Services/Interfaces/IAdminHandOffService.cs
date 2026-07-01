using HealthAxisCore_Api.Models.Dtos;
using System.Security.Claims;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IAdminHandoffService
    {
        Task<CreateAdminHandoffResponseDto> CreateAsync(
            ClaimsPrincipal user,
            CancellationToken ct = default);

        Task<AuthResponseDto> ExchangeAsync(
            ExchangeAdminHandoffRequestDto request,
            CancellationToken ct = default);
    }
}