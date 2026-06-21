using HealthApp.API.Enums;
using HealthApp.API.Models.DTOs;

namespace HealthApp.API.Service.Interface;

public interface IDoctorService
{
    Task<List<DoctorDto>> GetAllDoctorsAsync();

    Task<DoctorDto> GetDoctorByIdAsync(int doctorId);

    Task<List<DoctorDto>> GetDoctorsBySpecialisationAsync(
        SpecialisationType specialisation);

    Task<DoctorAvailabilityDto> GetDoctorAvailabilityAsync(
        int doctorId,
        DateTime date);
}