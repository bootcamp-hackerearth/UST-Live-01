using HealthAxis.Shared.Dtos;
using HealthAxis.Web.Services;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;

namespace HealthAxis.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientApiService _patientService;
        private readonly IDoctorApiService _doctorService;
        private readonly IAppointmentApiService _appointmentService;
        private readonly IHealthRecordApiService _healthService;

        public PatientController(
            IPatientApiService patientService,
            IDoctorApiService doctorService,
            IAppointmentApiService appointmentService,
            IHealthRecordApiService healthService)
        {
            _patientService = patientService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            _healthService = healthService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GoToAction(string actionName, int patientId)
        {
            return RedirectToAction(actionName, new { id = patientId });
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(PatientDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _patientService.Register(dto);

            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View(dto);
            }

            TempData["Success"] = "Patient registered successfully!";
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Profile(int id)
        {
            var patient = await _patientService.GetById(id);

            if (patient == null)
            {
                ViewBag.Error = "Invalid Patient ID";
                return View("EnterPatientId");
            }

            return View(patient);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var patient = await _patientService.GetById(id);

            if (patient == null)
            {
                ViewBag.Error = "Invalid Patient ID";
                return View("EnterPatientId");
            }

            return View(patient);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(PatientDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _patientService.Update(dto.PatientId, dto);

            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View(dto);
            }

            var updatedPatient = await _patientService.GetById(dto.PatientId);

            TempData["Success"] = "Patient updated successfully!";

            return View("Profile", updatedPatient);
        }

        [HttpGet]
        public ActionResult SearchBySpecialisation()
        {
            return View();
        }

        public async Task<ActionResult> SearchBySpecialisation(Specialisation spec)
        {
            var doctors = await _doctorService.GetBySpecialisation(spec);
            return View(doctors);
        }

        public ActionResult BookAppointment()
        {
            return View("EnterPatientId");
        }

        public async Task<ActionResult> Book(int id)
        {
            var patient = await _patientService.GetById(id);

            if (patient == null || !patient.IsActive)
            {
                ViewBag.Error = "Invalid or inactive patient";
                return View("EnterPatientId");
            }

            var dto = new BookAppointmentDto
            {
                PatientId = id
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Book(BookAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _appointmentService.Book(dto);

            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View(dto);
            }

            TempData["Success"] = "Appointment booked successfully!";
            return RedirectToAction("Profile", new { id = dto.PatientId });
        }

        public async Task<ActionResult> MyAppointments(int id)
        {
            var appointments = await _appointmentService.GetByPatient(id);

            if (appointments == null)
            {
                ViewBag.Error = "Invalid Patient ID";
                return View("EnterPatientId");
            }

            return View(appointments);
        }

        public ActionResult CancelAppointment(int id)
        {
            var dto = new CancelAppointmentDto
            {
                AppointmentId = id
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<ActionResult> CancelAppointment(CancelAppointmentDto dto)
        {
            await _appointmentService.Cancel(dto.AppointmentId, dto);

            return RedirectToAction("MyAppointments", new { id = dto.PatientId });
        }

        public async Task<ActionResult> HealthRecords(int id)
        {
            var records = await _healthService.GetByPatient(id);

            if (records == null || records.Count == 0)
            {
                ViewBag.Error = "No health records found";
                return View(new List<HealthRecordDto>());
            }

            return View(records);
        }
    }
}