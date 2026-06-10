using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>>
            GetAllDoctorsAsync();

        Task<DoctorDto>
            GetDoctorByIdAsync(
                int doctorId);

        Task<int>
            AddDoctorAsync(
                CreateDoctorDto doctorDto);

        Task UpdateDoctorAsync(
            int doctorId,
            UpdateDoctorDto doctorDto);

        Task DeleteDoctorAsync(
            int doctorId);

        Task<IEnumerable<DoctorDto>>
            GetDoctorsBySpecialisationAsync(
                Specialisation specialisation);

        Task<IEnumerable<DoctorDto>>
            GetDoctorsByNameAsync(
                string searchTerm);
    }
}