
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;

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