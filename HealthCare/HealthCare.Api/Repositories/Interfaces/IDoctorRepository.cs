using Healthcare.Shared.DTOs.Doctor;
using HealthCare.Api.Models;


namespace HealthCare.Api.Repositories.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<Doctor?> GetByUserIdAsync(string userId);
        Task<List<string>> GetSlots(int doctorId);
        Task CreateSlots(int doctorId, List<string> timeslots);
        Task<List<DoctorLeaves>> GetLeavesByDoctorId(int doctorId);
        Task CreateLeaves(int doctorId, List<CreateLeaveDto> leaves);
        Task<List<DoctorListDto>> AvailableDoctors(string specialisation, DateOnly date);

    }
}
