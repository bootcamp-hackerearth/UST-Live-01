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
        : Repository<Insurance>,
          IInsuranceRepository
    {
        public InsuranceRepository(
            ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<
            IEnumerable<Insurance>>
            GetInsurancesByPatientAsync(
                int patientId)
        {
            return await _dbSet
                .Include(i => i.Patient)
                .Where(i =>
                    i.PatientId
                    == patientId)
                .OrderByDescending(
                    i => i.ExpiryDate)
                .ToListAsync();
        }

        public async Task<
            IEnumerable<Insurance>>
            GetInsurancesByStatusAsync(
                InsuranceStatus status)
        {
            return await _dbSet
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
            return await _dbSet
                .Include(i => i.Patient)
                .FirstOrDefaultAsync(i =>
                    i.PolicyNumber
                    == policyNumber);
        }

        public async Task<
            IEnumerable<Insurance>>
            GetExpiredInsurancesAsync()
        {
            return await _dbSet
                .Include(i => i.Patient)
                .Where(i =>
                    i.ExpiryDate
                    < DateTime.Today)
                .OrderBy(
                    i => i.ExpiryDate)
                .ToListAsync();
        }

        public async Task<
            IEnumerable<Insurance>>
            GetActiveInsurancesAsync()
        {
            return await _dbSet
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