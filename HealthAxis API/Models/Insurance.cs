using HealthAxis.API.Enums;
using HealthAxis.API.Utilities;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxis.API.Models
{

    public class Insurance
    {

        [Key]
        public int InsuranceId { get; set; }

        [Required(ErrorMessage = Constant.PatientRequired)]
        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }

        [Required(ErrorMessage = Constant.ProviderNameRequired)]
        [StringLength(ValidationLimits.ProviderNameLength)]
        public string ProviderName { get; set; } = string.Empty;

        [Required(ErrorMessage = Constant.PolicyNumberRequired)]
        [StringLength(ValidationLimits.PolicyNumberLength)]
        public string PolicyNumber { get; set; } = string.Empty;

        [Range(
            typeof(decimal),
            ValidationLimits.MinCoverageAmount,
            ValidationLimits.MaxCoverageAmount)]
        public decimal CoverageAmount { get; set; }

        [Required(ErrorMessage = Constant.ExpiryDateRequired)]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        [Required(ErrorMessage = Constant.InsuranceStatusRequired)]
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

            return Math.Max(0,
                (ExpiryDate.Date - DateTime.Today).Days);
        }
    }
}