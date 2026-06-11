using SharedClasses.Dtos;
using SharedClasses.Enums;
using System.Collections.Generic;

namespace HealthcareApi.Services
{
    public interface IDoctorService
    {
        List<DoctorDto> GetAllDoctors();

        List<DoctorDto> GetAllActiveDoctors();

        DoctorDto GetDoctorById(int doctorId);

        List<DoctorDto> SearchDoctors(string query);

        List<DoctorDto> SearchDoctorsBySpecialisation(Specialisation specialisation);

        List<DoctorDto> SearchActiveDoctors(
            string query,
            Specialisation? specialisation);
        DoctorDto AddDoctor(CreateDoctorDto dto);

        DoctorDto UpdateDoctor(int doctorId, UpdateDoctorDto dto);

        DoctorDto DeactivateDoctor(int doctorId);

        DoctorDto ReactivateDoctor(int doctorId);
    }
}