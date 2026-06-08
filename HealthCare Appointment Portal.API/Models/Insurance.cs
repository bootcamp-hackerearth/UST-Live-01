using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare_Appointment_Portal.Models
{

    public class Insurance
    {

        [Key]
        public int InsuranceId { get; set; }

        [Required(ErrorMessage = Constants.PatientRequired)]
        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }

        [Required(ErrorMessage = Constants.ProviderNameRequired)]
        [StringLength(ValidationLimits.ProviderNameLength)]
        public string ProviderName { get; set; } = string.Empty;

        [Required(ErrorMessage = Constants.PolicyNumberRequired)]
        [StringLength(ValidationLimits.PolicyNumberLength)]
        public string PolicyNumber { get; set; } = string.Empty;

        [Range(
            typeof(decimal),
            ValidationLimits.MinCoverageAmount,
            ValidationLimits.MaxCoverageAmount)]
        public decimal CoverageAmount { get; set; }

        [Required(ErrorMessage = Constants.ExpiryDateRequired)]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        [Required(ErrorMessage = Constants.InsuranceStatusRequired)]
        public InsuranceStatus Status { get; set; }

        public bool IsExpired()
        {

            return ExpiryDate.Date < DateTime.Today;
        }

        public bool IsActive()
        {

            return Status == InsuranceStatus.Active &&
                   !IsExpired();
        }

        public int DaysUntilExpiry()
        {

            return Math.Max( 0,
                (ExpiryDate.Date - DateTime.Today).Days);
        }
    }
}