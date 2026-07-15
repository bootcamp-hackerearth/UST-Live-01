using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Shared.DTOs.Doctor
{

    public class DoctorAvailabilityResponseDto
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }

        public bool IsOnLeave { get; set; }

        public DateTime? LeaveStartDate { get; set; }


        public DateTime? LeaveEndDate { get; set; }
    }
}



