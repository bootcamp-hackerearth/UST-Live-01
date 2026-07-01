using HealthApp.Shared.Dto;

namespace HealthApp.Api.Service.Interface
{
    public interface IDoctorService
    {
        Task<DoctorDto> AddDoctorAsync(DoctorDto dto);


        Task<DoctorDto> GetDoctorByIdAsync(int id);

        Task<DoctorDto> UpdateDoctorByIdAsync(int id, DoctorDto doctorDto);

        Task<DoctorDto> GetMyProfileAsync(string identityUserId);




        Task<(List<DoctorDto> Items, int TotalCount)> GetPagedDoctorsAsync(int pageNumber,int pageSize);

        Task<(List<DoctorDto> Items, int TotalCount)> GetPagedActiveDoctorsAsync(int pageNumber, int pageSize);

        Task<(List<DoctorDto> Items, int TotalCount)> SearchBySpecialisationPagedAsync
            (string specialisation,int pageNumber, int pageSize);


    }
}
