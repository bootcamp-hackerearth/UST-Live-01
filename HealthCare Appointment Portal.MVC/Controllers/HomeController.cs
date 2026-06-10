using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using HealthCare_Appointment_Portal_MVC.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPatientApiService
            _patientService;

        private readonly IDoctorApiService
            _doctorService;

        private readonly IAppointmentApiService
            _appointmentService;

        private readonly IHealthRecordApiService
            _healthRecordService;

        public HomeController(
            IPatientApiService patientService,
            IDoctorApiService doctorService,
            IAppointmentApiService appointmentService,
            IHealthRecordApiService healthRecordService)
        {
            _patientService =
                patientService;

            _doctorService =
                doctorService;

            _appointmentService =
                appointmentService;

            _healthRecordService =
                healthRecordService;
        }

        public async Task<ActionResult> Index()
        {
            var patients =
                await _patientService
                    .GetAllPatientsAsync();

            var doctors =
                await _doctorService
                    .GetAllDoctorsAsync();

            var appointments =
                await _appointmentService
                    .GetAllAppointmentsAsync();

            var records =
                await _healthRecordService
                    .GetAllHealthRecordsAsync();

            var vm =
                new AdminDashboardViewModel
                {
                    TotalPatients =
                        patients.Count(),

                    TotalDoctors =
                        doctors.Count(),

                    TotalAppointments =
                        appointments.Count(),

                    TotalHealthRecords =
                        records.Count()
                };

            return View(vm);
        }
    }
}