namespace HealthAxisAdminLayout.DTOs.Dashboard
{
        public class AdminDashboardDto
        {
            public int TotalDoctors { get; set; }

            public int ActiveDoctors { get; set; }

            public int TotalPatients { get; set; }

            public int TotalAppointments { get; set; }

            public int PendingAppointments { get; set; }

            public int ConfirmedAppointments { get; set; }

            public int CancelledAppointments { get; set; }

            public int TotalHealthRecords { get; set; }
        }
    }
