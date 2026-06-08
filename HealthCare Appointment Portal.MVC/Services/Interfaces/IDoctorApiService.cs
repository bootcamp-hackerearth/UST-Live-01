using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal_MVC.Services.Interfaces
{
    public interface IDoctorApiService
    {
        Task<IEnumerable<DoctorDto>>
            GetAllDoctorsAsync();

        Task<DoctorDto>
            GetDoctorByIdAsync(
                int id);

        Task<IEnumerable<DoctorDto>>
            GetDoctorsBySpecialisationAsync(
                Specialisation specialisation);

        Task<int>
            CreateDoctorAsync(
                CreateDoctorDto dto);

        Task
            UpdateDoctorAsync(
                int id,
                UpdateDoctorDto dto);

        Task
            DeleteDoctorAsync(
                int id);
    }
}