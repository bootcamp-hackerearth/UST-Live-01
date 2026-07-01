using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.Models;

namespace HealthAxis.API.Services
{
    public interface IDoctorService
        : IService<Doctor, DoctorReadDto, DoctorCreateDto, DoctorUpdateDto>
    {
        Task<DoctorAvailabilityDto?> GetAvailabilityAsync(
            int doctorId,
            DateTime date,
            string timeSlot,
            CancellationToken ct = default);

        Task<List<string>> GetAvailableSlotsAsync(
            int doctorId,
            DateTime date,
            CancellationToken ct = default);

        Task<DoctorReadDto?> UpdateActiveStatusAsync(
            int doctorId,
            bool isActive,
            CancellationToken ct = default);

    }
}