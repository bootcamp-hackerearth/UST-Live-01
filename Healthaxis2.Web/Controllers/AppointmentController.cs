using Healthaxis2.Shared.DTOs;
using Healthaxis2.Web.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Healthaxis2.Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _service;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;

        public AppointmentController(
            IAppointmentService service,
            IDoctorService doctorService,
            IPatientService patientService)
        {
            _service = service;
            _doctorService = doctorService;
            _patientService = patientService;
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Patients = await _patientService.GetAll();

            var doctors = await _doctorService.GetAll();
            ViewBag.Doctors = doctors;

            ViewBag.Specialisations = doctors
                .Select(d => d.Specialisation)
                .Distinct()
                .ToList();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(AppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _patientService.GetAll();

                var doctors = await _doctorService.GetAll();
                ViewBag.Doctors = doctors;

                ViewBag.Specialisations = doctors
                    .Select(d => d.Specialisation)
                    .Distinct()
                    .ToList();

                return View(dto);
            }

            await _service.Create(dto);

            return RedirectToAction("Index", "Patient");
        }

        public ActionResult PatientAppointments()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PatientAppointments(int patientId)
        {
            var allAppointments = await _service.GetAll();

            var result = allAppointments
                .Where(a => a.PatientId == patientId)
                .ToList();

            return View("PatientAppointmentsResult", result);
        }

        [HttpPost]
        public async Task<ActionResult> Cancel(int id, string reason)
        {
            await _service.UpdateStatus(id, "Cancelled", reason);
            return RedirectToAction("PatientAppointments");
        }

        public async Task<ActionResult> DoctorAppointments(int doctorId)
        {
            var appts = await _service.GetAll();
            var result = appts.FindAll(a => a.DoctorId == doctorId);

            return View(result);
        }

        public async Task<ActionResult> UpdateStatus(int id, string status)
        {
            await _service.UpdateStatus(id, status, null);

            if (status == "Completed")
                return RedirectToAction("Create", "HealthRecord", new { appointmentId = id });

            return RedirectToAction("DoctorAppointments");
        }
    }
}