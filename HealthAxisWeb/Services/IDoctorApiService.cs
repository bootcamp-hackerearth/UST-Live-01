using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAxis.Web.Services
{
    public interface IDoctorApiService
    {
        Task<List<DoctorDto>> GetAllDoctors();
        Task<DoctorDto> GetDoctorById(int id);
        Task<bool> AddDoctor(DoctorDto dto);
        Task<bool> UpdateDoctor(int id, DoctorDto dto);

        Task<List<DoctorDto>> GetBySpecialisation(Specialisation spec);

        Task<List<PatientDto>> GetAllPatients();
    }
}