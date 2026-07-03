using HealthAxisApplicn.Dto.Patients;
using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto?> GetByIdAsync(int id);
        Task<PatientDto> CreateAsync(CreatePatientDto entity);
        Task<PatientDto?> UpdateAsync(int id, UpdatePatientDto entity);
        Task<List<PatientDto>> SearchByPatientNameAsync(string name);
        Task<PatientDto?> SearchByPhoneNumberAsync(string phoneNumber);
        Task<PatientDto?> SearchByEmailAsync(string email);
        Task<PatientDto> DeactivatePatientAsync(int id);
        Task<List<PatientDto>> SearchAsync(string? name, string? phone);
        Task<Patient?> GetByUserIdAsync(string userId);
        Task<PatientDetailsDto?> GetPatientDetailsForDoctorAsync(int doctorId,int patientId);
    }
}
