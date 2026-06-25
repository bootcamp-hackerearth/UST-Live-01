using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Shared.DTOs.Appointments
{

    public class AppointmentSummaryDto
    {


        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }

        public int TotalAppointments { get; set; }
        public int PendingCount { get; set; }
        public int ConfirmedCount { get; set; }
        public int CompletedCount { get; set; }
        public int CancelledCount { get; set; }

        public decimal TotalRevenue { get; set; }
    }

}
