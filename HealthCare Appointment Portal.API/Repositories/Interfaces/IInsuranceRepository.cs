using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IInsuranceRepository
        : IRepository<Insurance>
    {
        Task<IEnumerable<Insurance>>
            GetInsurancesByPatientAsync(
                int patientId);

        Task<IEnumerable<Insurance>>
            GetInsurancesByStatusAsync(
                InsuranceStatus status);

        Task<Insurance>
            GetInsuranceByPolicyNumberAsync(
                string policyNumber);

        Task<IEnumerable<Insurance>>
            GetExpiredInsurancesAsync();

        Task<IEnumerable<Insurance>>
            GetActiveInsurancesAsync();
    }
}