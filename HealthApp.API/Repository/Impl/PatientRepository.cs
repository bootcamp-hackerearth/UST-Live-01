using HealthApp.API.Data;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.API.Repository.Impl;

public class PatientRepository(HealthAppDbContext context)
    : Repository<Patient>(context), IPatientRepository
{
    public Task<Patient?> GetByUserIdAsync(
        string userId,
        CancellationToken ct = default)
        => context.Patients
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

    public async Task<bool> IsDuplicatePatientAsync(
        string patientName,
        string email,
        string phoneNumber,
        DateTime dateOfBirth,
        int? excludePatientId = null,
        CancellationToken ct = default)
    {
        return await context.Patients.AnyAsync(p =>
            (!excludePatientId.HasValue || p.PatientId != excludePatientId.Value) &&
            p.PatientName.ToLower() == patientName &&
            (p.Email ?? "").ToLower() == email &&
            p.PhoneNumber == phoneNumber &&
            p.DateOfBirth.Date == dateOfBirth.Date, ct);
    }
}