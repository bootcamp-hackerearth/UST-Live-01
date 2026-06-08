using HealthcareApi.Dtos;
using HealthcareApi.Enums;
using System.Collections.Generic;

namespace HealthcareApi.Services
{
    public interface IDoctorService
    {
        List<DoctorDto> GetAllDoctors();

        List<DoctorDto> GetAllActiveDoctors();

        DoctorDto GetDoctorById(int doctorId);

        List<DoctorDto> SearchDoctorsBySpecialisation(Specialisation specialisation);

        DoctorDto AddDoctor(CreateDoctorDto dto);

        DoctorDto UpdateDoctor(int doctorId, UpdateDoctorDto dto);

        DoctorDto DeactivateDoctor(int doctorId);

        DoctorDto ReactivateDoctor(int doctorId);
    }
}