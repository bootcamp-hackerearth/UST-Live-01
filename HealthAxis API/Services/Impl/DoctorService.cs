using AutoMapper;
using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;

namespace HealthAxis.API.Services
{
    public class DoctorService
        : Service<Doctor, DoctorReadDto, DoctorCreateDto, DoctorUpdateDto>, IDoctorService
    {
        public DoctorService(
            IDoctorRepository doctorRepository,
            IMapper mapper)
            : base(doctorRepository, mapper)
        {
        }
    }
}
