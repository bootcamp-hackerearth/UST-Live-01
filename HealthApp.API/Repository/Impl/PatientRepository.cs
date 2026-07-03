using HealthApp.API.Data;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.API.Repository.Impl;

public class PatientRepository(HealthAppDbContext context)
    : Repository<Patient>(context), IPatientRepository
{
    private const string LikeEscapeCharacter = "\\";

    public Task<Patient?> GetByUserIdAsync(
        string userId,
        CancellationToken ct = default)
        => context.Patients
            .FirstOrDefaultAsync(patient => patient.UserId == userId, ct);

    public async Task<bool> IsDuplicatePatientAsync(
        string patientName,
        string email,
        string phoneNumber,
        DateTime dateOfBirth,
        int? excludePatientId = null,
        CancellationToken ct = default)
    {
        var normalizedPatientName = patientName.Trim();
        var normalizedEmail = email.Trim();
        var normalizedPhoneNumber = phoneNumber.Trim();

        return await context.Patients.AnyAsync(patient =>
            (!excludePatientId.HasValue || patient.PatientId != excludePatientId.Value) &&
            patient.PatientName == normalizedPatientName &&
            patient.Email != null &&
            patient.Email == normalizedEmail &&
            patient.PhoneNumber == normalizedPhoneNumber &&
            patient.DateOfBirth.Date == dateOfBirth.Date,
            ct);
    }

    public async Task<List<Patient>> GetFilteredAsync(
        string? search = null,
        GenderType? gender = null,
        bool? hasInsurance = null,
        CancellationToken ct = default)
    {
        var query = BuildFilteredQuery(search, gender, hasInsurance);

        return await query
            .OrderBy(patient => patient.PatientName)
            .ToListAsync(ct);
    }

    public async Task<(List<Patient> Items, int TotalCount)> GetFilteredPagedAsync(
        string? search = null,
        GenderType? gender = null,
        bool? hasInsurance = null,
        int pageNumber = 1,
        int pageSize = 5,
        CancellationToken ct = default)
    {
        if (pageNumber < 1)
        {
            pageNumber = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 5;
        }

        var query = BuildFilteredQuery(search, gender, hasInsurance);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderBy(patient => patient.PatientId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    private IQueryable<Patient> BuildFilteredQuery(
        string? search,
        GenderType? gender,
        bool? hasInsurance)
    {
        var query = context.Patients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = ApplySearchFilter(query, search);
        }

        if (gender.HasValue)
        {
            query = query.Where(patient => patient.Gender == gender.Value.ToString());
        }

        if (hasInsurance.HasValue)
        {
            query = hasInsurance.Value
                ? query.Where(patient => !string.IsNullOrEmpty(patient.InsuranceId))
                : query.Where(patient => string.IsNullOrEmpty(patient.InsuranceId));
        }

        return query;
    }

    private static IQueryable<Patient> ApplySearchFilter(
        IQueryable<Patient> query,
        string search)
    {
        var keyword = search.Trim();
        var searchPattern = $"%{EscapeLikePattern(keyword)}%";
        var isPatientIdSearch = int.TryParse(keyword, out var patientId);

        return query.Where(patient =>
            (isPatientIdSearch && patient.PatientId == patientId) ||
            EF.Functions.Like(patient.PatientName, searchPattern, LikeEscapeCharacter) ||
            (patient.Email != null &&
                EF.Functions.Like(patient.Email, searchPattern, LikeEscapeCharacter)) ||
            EF.Functions.Like(patient.PhoneNumber, searchPattern, LikeEscapeCharacter) ||
            (patient.InsuranceId != null &&
                EF.Functions.Like(patient.InsuranceId, searchPattern, LikeEscapeCharacter)));
    }

    private static string EscapeLikePattern(string value)
    {
        return value
            .Replace(LikeEscapeCharacter, "\\\\")
            .Replace("%", "\\%")
            .Replace("_", "\\_")
            .Replace("[", "\\[");
    }
}