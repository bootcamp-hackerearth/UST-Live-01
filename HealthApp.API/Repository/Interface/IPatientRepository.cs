using HealthApp.API.Models;
using HealthApp.Shared.Enums;

namespace HealthApp.API.Repository.Interface;

public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> GetByUserIdAsync(
        string userId,
        CancellationToken ct = default);

    Task<bool> IsDuplicatePatientAsync(
        string patientName,
        string email,
        string phoneNumber,
        DateTime dateOfBirth,
        int? excludePatientId = null,
        CancellationToken ct = default);

    Task<List<Patient>> GetFilteredAsync(
        string? search = null,
        GenderType? gender = null,
        bool? hasInsurance = null,
        CancellationToken ct = default);

    Task<(List<Patient> Items, int TotalCount)> GetFilteredPagedAsync(
    string? search = null,
    GenderType? gender = null,
    bool? hasInsurance = null,
    int pageNumber = 1,
    int pageSize = 5,
    CancellationToken ct = default);
}