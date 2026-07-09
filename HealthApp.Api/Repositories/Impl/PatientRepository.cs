using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HealthApp.Api.Data;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;

namespace HealthApp.Api.Repositories.Impl
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly HealthAppDbContext _context;

        public PatientRepository(HealthAppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Patient> Items, int TotalCount)> GetPatientsAsync(
            string? name,
            string? email,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Set<Patient>()
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p =>
                    p.FullName != null &&
                    p.FullName.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(p =>
                    p.Email != null &&
                    p.Email.Contains(email));
            }

            var totalCount = await query.CountAsync(ct);

            var patients = await query
                .OrderBy(p => p.PatientId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (patients, totalCount);
        }

        public async Task<bool> IsDuplicatePatient(
            string name,
            DateTime dob,
            string email,
            CancellationToken ct = default)
        {
            var dobOnly = DateOnly.FromDateTime(dob);

            return await _context.Set<Patient>()
                .AnyAsync(p =>
                    p.FullName == name &&
                    p.DateOfBirth == dobOnly &&
                    p.Email == email,
                    ct);
        }

        public async Task<string?> GetPatientUserIdAsync(
            int patientId,
            CancellationToken ct = default)
        {
            return await _context.Users
                .Where(user => user.PatientId == patientId)
                .Select(user => user.Id)
                .FirstOrDefaultAsync(ct);
        }
    }
}