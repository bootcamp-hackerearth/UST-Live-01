using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Shared.DTOs.DoctorLeaves
{

    public class DoctorLeaveDto
    {
        public int LeaveId { get; set; }

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Reason { get; set; } = string.Empty;


        public DateTime CreatedDate { get; set; }
    }
}


