using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Repositories
{
    public class InsuranceRepository
        : IInsuranceRepository
    {
        private readonly ApplicationDbContext _context;

        public InsuranceRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Insurance>
            GetByIdAsync(
                int id)
        {
            return await _context.Insurances
                .FindAsync(id);
        }

        public async Task<IEnumerable<Insurance>>
            GetAllAsync()
        {
            return await _context.Insurances
                .ToListAsync();
        }

        public async Task AddAsync(
            Insurance insurance)
        {
            _context.Insurances
                .Add(insurance);

            await Task.CompletedTask;
        }

        public async Task UpdateAsync(
            Insurance insurance)
        {
            _context.Entry(insurance)
                .State = EntityState.Modified;

            await Task.CompletedTask;
        }

        public async Task DeleteAsync(
            int id)
        {
            Insurance insurance =
                await _context.Insurances
                    .FindAsync(id);

            if (insurance != null)
            {
                _context.Insurances
                    .Remove(insurance);
            }
        }

        public async Task<
            IEnumerable<Insurance>>
            GetInsurancesByPatientAsync(
                int patientId)
        {
            return await _context.Insurances
                .Include(i => i.Patient)
                .Where(i =>
                    i.PatientId ==
                    patientId)
                .OrderByDescending(
                    i => i.ExpiryDate)
                .ToListAsync();
        }

        public async Task<
            IEnumerable<Insurance>>
            GetInsurancesByStatusAsync(
                InsuranceStatus status)
        {
            return await _context.Insurances
                .Include(i => i.Patient)
                .Where(i =>
                    i.Status == status)
                .OrderBy(
                    i => i.ProviderName)
                .ToListAsync();
        }

        public async Task<Insurance>
            GetInsuranceByPolicyNumberAsync(
                string policyNumber)
        {
            return await _context.Insurances
                .Include(i => i.Patient)
                .FirstOrDefaultAsync(i =>
                    i.PolicyNumber ==
                    policyNumber);
        }

        public async Task<
            IEnumerable<Insurance>>
            GetExpiredInsurancesAsync()
        {
            return await _context.Insurances
                .Include(i => i.Patient)
                .Where(i =>
                    i.ExpiryDate <
                    DateTime.Today)
                .OrderBy(
                    i => i.ExpiryDate)
                .ToListAsync();
        }

        public async Task<
            IEnumerable<Insurance>>
            GetActiveInsurancesAsync()
        {
            return await _context.Insurances
                .Include(i => i.Patient)
                .Where(i =>
                    i.Status ==
                    InsuranceStatus.Active
                    &&
                    i.ExpiryDate >=
                    DateTime.Today)
                .OrderBy(
                    i => i.ProviderName)
                .ToListAsync();
        }
    }
}