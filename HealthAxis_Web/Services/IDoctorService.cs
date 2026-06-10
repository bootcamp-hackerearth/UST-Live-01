using HealthAxis.Shared.Dtos;
using System.Collections.Generic;

namespace HealthAxis.Api.Services
{
    public interface IDoctorService
    {
        DoctorDto AddDoctor(DoctorDto dto);

        List<DoctorDto> GetAllDoctors();

        DoctorDto GetById(int doctorId);

        DoctorDto UpdateDoctor(int id, DoctorDto dto);
    }
}