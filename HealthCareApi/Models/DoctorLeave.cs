using System;

namespace HealthCareApi.Models
{
    public class DoctorLeave
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public DateTime LeaveDate { get; set; }
        public string Reason { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}