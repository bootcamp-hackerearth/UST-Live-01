using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCareApi.Models.Views
{
    public partial class VwPatientHealthHistory
    {
        [Key]
        public int RecordId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string Specialisation { get; set; }
        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
        public string Notes { get; set; }
    }
}