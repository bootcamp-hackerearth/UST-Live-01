using HealthAxis.Shared.Dtos;
using HealthAxis.Web.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace HealthAxis.Web.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientApiService _patientService;
        private readonly IAppointmentApiService _appointmentService;

        public PatientController(
            IPatientApiService patientService,
            IAppointmentApiService appointmentService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
        }

        public async Task<ActionResult> Index(string sort, bool? isActive, bool? insured)
        {
            var patients = await _patientService.GetAll();

            if (patients == null)
                patients = new List<PatientDto>();

            if (isActive.HasValue)
            {
                patients = patients
                    .Where(p => p.IsActive == isActive.Value)
                    .ToList();
            }

            if (insured.HasValue)
            {
                patients = patients
                    .Where(p => insured.Value
                        ? !string.IsNullOrEmpty(p.InsuranceId)
                        : string.IsNullOrEmpty(p.InsuranceId))
                    .ToList();
            }

            if (sort == "asc")
                patients = patients.OrderBy(p => p.FullName).ToList();
            else if (sort == "desc")
                patients = patients.OrderByDescending(p => p.FullName).ToList();

            ViewBag.Total = patients.Count;
            ViewBag.Active = patients.Count(p => p.IsActive);
            ViewBag.Insured = patients.Count(p => !string.IsNullOrEmpty(p.InsuranceId));

            return View(patients);
        }

        public async Task<ActionResult> Details(int id)
        {
            var patient = await _patientService.GetById(id);

            if (patient == null)
            {
                ViewBag.Error = "Patient not found";
                return RedirectToAction("Index");
            }

            return View(patient);
        }

        public ActionResult Create()
        {
            return View(new CreatePatientDto());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreatePatientDto dto)
        {
            if (string.IsNullOrWhiteSpace(Request["DateOfBirth"]))
            {
                ModelState.AddModelError("DateOfBirth", "Date of Birth is required");
            }

            if (!ModelState.IsValid)
            {
                return View("Create", dto);
            }

            var result = await _patientService.Create(dto);

    if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View("Create", dto);
            }

            TempData["Success"] = "Patient added successfully";

            return RedirectToAction("Index");
        }



        public async Task<ActionResult> Edit(int id)
        {
            var patient = await _patientService.GetById(id);

            if (patient == null)
            {
                ViewBag.Error = "Patient not found";
                return RedirectToAction("Index");
            }

            return View(patient);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, PatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result = await _patientService.Update(id, dto);

            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View(dto);
            }

            TempData["Success"] = "Patient updated successfully";

            return RedirectToAction("Details", new { id = id });
        }


        [HttpPost]
        public async Task<JsonResult> Deactivate(int id)
        {
            var result = await _patientService.Deactivate(id);
            return Json(result.Success);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateStatus(int id, UpdateAppointmentStatusDto dto)
        {
            var result = await _appointmentService.UpdateStatus(id, dto);
            return Json(result.Success);
        }
    }
}
