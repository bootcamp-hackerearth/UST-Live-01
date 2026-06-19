using HealthApp.Api.Dto;

namespace HealthApp.Api.Service.Interface
{
    public interface IDoctorService
    {
        Task<DoctorDto> AddDoctorAsync(DoctorDto dto);

        Task<List<DoctorDto>> GetAllActiveDoctorAsync();

        Task<List<DoctorDto>> GetAllDoctorsAsync();

        Task<DoctorDto> GetDoctorByIdAsync(int id);

        Task<List<DoctorDto>> SearchBySpecialisationAsync(string specialisation);

        Task<DoctorDto> UpdateDoctorByIdAsync(int id, DoctorDto doctorDto);

    }
}
