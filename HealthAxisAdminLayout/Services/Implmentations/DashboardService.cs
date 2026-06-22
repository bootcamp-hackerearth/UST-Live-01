using HealthAxisAdminLayout.DTOs.Dashboard;
using HealthAxisAdminLayout.Services.Interfaces;

namespace HealthAxisAdminLayout.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IDoctorApiService _doctorService;
        private readonly IPatientApiService _patientService;
        private readonly IAppointmentApiService _appointmentService;
        private readonly IHealthRecordApiService _healthRecordService;

        public DashboardService(
            IDoctorApiService doctorService,
            IPatientApiService patientService,
            IAppointmentApiService appointmentService,
            IHealthRecordApiService healthRecordService)
        {
            _doctorService = doctorService;
            _patientService = patientService;
            _appointmentService = appointmentService;
            _healthRecordService = healthRecordService;
        }

        public async Task<AdminDashboardDTO> GetDashboardAsync()
        {
            var doctors = await _doctorService.GetDoctorsAsync();
            var patients = await _patientService.GetPatientsAsync();
            var appointments = await _appointmentService.GetAppointmentsAsync();
            var records = await _healthRecordService.GetHealthRecordsAsync();

            return new AdminDashboardDTO
            {
                TotalDoctors = doctors.Count,
                ActiveDoctors = doctors.Count(d => d.IsActive),
                TotalPatients = patients.Count,
                TotalAppointments = appointments.Count,
                PendingAppointments = appointments.Count(a => a.Status == 0),
                ConfirmedAppointments = appointments.Count(a => a.Status == 1),
                CancelledAppointments = appointments.Count(a => a.Status == 2),
                TotalHealthRecords = records.Count
            };
        }
    }
}