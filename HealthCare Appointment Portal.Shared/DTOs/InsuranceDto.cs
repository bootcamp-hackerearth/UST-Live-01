using HealthCare_Appointment_Portal.Enums;
using System;

namespace HealthCare_Appointment_Portal.DTOs.InsuranceDtos
{
    public class InsuranceDto
    {
        public int InsuranceId
        {
            get;
            set;
        }

        public int PatientId
        {
            get;
            set;
        }

        public string PatientName
        {
            get;
            set;
        }

        public string ProviderName
        {
            get;
            set;
        }

        public string PolicyNumber
        {
            get;
            set;
        }

        public decimal CoverageAmount
        {
            get;
            set;
        }

        public DateTime ExpiryDate
        {
            get;
            set;
        }

        public InsuranceStatus Status
        {
            get;
            set;
        }

        public int DaysUntilExpiry
        {
            get;
            set;
        }

        public bool IsExpired
        {
            get;
            set;
        }
    }
}