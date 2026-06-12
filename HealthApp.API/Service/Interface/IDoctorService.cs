using HealthApp.Shared.DTOs;
using HealthApp.Shared.Constant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.API.Service.Interface
{
    public interface IDoctorService
    {
        Task AddDoctor(DoctorDto dto);

        Task<List<DoctorDto>> GetAllDoctors();

        Task<DoctorDto> GetDoctorById(int id);

        Task<List<DoctorDto>> SearchBySpecialisation(SpecialisationType specialisation);
        Task UpdateDoctorById(int id, DoctorDto doctorDto);
        Task ChangeDoctorStatus(int id);
    }
}