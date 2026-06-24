using Healthcare.Shared.DTOs.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Shared.DTOs.Appointment
{

    public class PagedAppointmentResponse
    {
        public List<AppointmentReportDto> Items { get; set; }
    }

}
