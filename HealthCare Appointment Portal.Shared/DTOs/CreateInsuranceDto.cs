using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCare_Appointment_Portal.DTOs.InsuranceDtos
{
    public class CreateInsuranceDto
    {
        [Required]
        public int PatientId
        {
            get;
            set;
        }

        [Required]
        [StringLength(
            ValidationLimits.ProviderNameLength)]
        public string ProviderName
        {
            get;
            set;
        }

        [Required]
        [StringLength(
            ValidationLimits.PolicyNumberLength)]
        public string PolicyNumber
        {
            get;
            set;
        }

        [Range(
            typeof(decimal),
            ValidationLimits.MinCoverageAmount,
            ValidationLimits.MaxCoverageAmount)]
        public decimal CoverageAmount
        {
            get;
            set;
        }

        [Required]
        public DateTime ExpiryDate
        {
            get;
            set;
        }

        [Required]
        public InsuranceStatus Status
        {
            get;
            set;
        }
    }
}