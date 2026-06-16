using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.Models;

namespace HealthAxis.API.Services
{
    public interface IDoctorService
        : IService<Doctor, DoctorReadDto, DoctorCreateDto, DoctorUpdateDto>
    {
    }
}
