using HealthAxisCore_Api.Models.Dtos;
using System.Security.Claims;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAppointmentsAsync(int? patientId, int? doctorId, DateTime? date, ClaimsPrincipal user, CancellationToken ct = default);

        Task<AppointmentDto> CreateAsync(CreateAppointmentDto request, ClaimsPrincipal user, CancellationToken ct = default);

        Task<AppointmentDto> UpdateStatusAsync(int id, UpdateAppointmentStatusDto request, ClaimsPrincipal user, CancellationToken ct = default); Task DeleteAsync(int id, CancellationToken ct = default);
    }
}