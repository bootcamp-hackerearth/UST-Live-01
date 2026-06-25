using HealthApp.API.Data;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.Shared.Enums;
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

    public async Task<List<Patient>> GetFilteredAsync(
        string? search = null,
        GenderType? gender = null,
        bool? hasInsurance = null,
        CancellationToken ct = default)
    {
        var query = BuildFilteredQuery(search, gender, hasInsurance);

        return await query
            .OrderBy(p => p.PatientName)
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
            .OrderBy(p => p.PatientId)
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
            var keyword = search.Trim().ToLower();

            query = query.Where(p =>
                p.PatientName.ToLower().Contains(keyword) ||
                (p.Email != null && p.Email.ToLower().Contains(keyword)) ||
                p.PhoneNumber.Contains(keyword) ||
                (p.InsuranceId != null && p.InsuranceId.ToLower().Contains(keyword)));

            if (int.TryParse(keyword, out var patientId))
            {
                query = query.Where(p =>
                    p.PatientId == patientId ||
                    p.PatientName.ToLower().Contains(keyword) ||
                    (p.Email != null && p.Email.ToLower().Contains(keyword)) ||
                    p.PhoneNumber.Contains(keyword) ||
                    (p.InsuranceId != null && p.InsuranceId.ToLower().Contains(keyword)));
            }
        }

        if (gender.HasValue)
        {
            query = query.Where(p => p.Gender == gender.Value.ToString());
        }

        if (hasInsurance.HasValue)
        {
            query = hasInsurance.Value
                ? query.Where(p => p.InsuranceId != null && p.InsuranceId != "")
                : query.Where(p => p.InsuranceId == null || p.InsuranceId == "");
        }

        return query;
    }
}