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
    }
}
