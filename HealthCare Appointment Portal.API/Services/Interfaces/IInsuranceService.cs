using HealthCare_Appointment_Portal.DTOs.InsuranceDtos;
using HealthCare_Appointment_Portal.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IInsuranceService
    {
        Task<IEnumerable<InsuranceDto>>
            GetAllInsurancesAsync();

        Task<InsuranceDto>
            GetInsuranceByIdAsync(
                int insuranceId);

        Task<IEnumerable<InsuranceDto>>
            GetInsurancesByPatientAsync(
                int patientId);

        Task<IEnumerable<InsuranceDto>>
            GetInsurancesByStatusAsync(
                InsuranceStatus status);

        Task<IEnumerable<InsuranceDto>>
            GetExpiredInsurancesAsync();

        Task<IEnumerable<InsuranceDto>>
            GetActiveInsurancesAsync();

        Task<int>
            AddInsuranceAsync(
                CreateInsuranceDto insuranceDto);

        Task UpdateInsuranceAsync(
            int insuranceId,
            UpdateInsuranceDto insuranceDto);

        Task DeleteInsuranceAsync(
            int insuranceId);
    }
}