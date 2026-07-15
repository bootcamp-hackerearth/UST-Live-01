using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Shared.DTOs.DoctorLeaves
{

    public class CreateMyDoctorLeaveDto
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}
