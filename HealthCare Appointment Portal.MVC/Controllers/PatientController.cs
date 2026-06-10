using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class PatientController : Controller
    {

        private readonly IPatientApiService _patientService;

        public PatientController(IPatientApiService patientService)
        {
            _patientService = patientService;
        }


        // GET: Patient
        public async Task<ActionResult> Index(string searchQuery = null)
        {
            ViewBag.CurrentSearch = searchQuery;

            var patientsList = await _patientService.GetAllPatientsAsync(searchQuery);

            return View(patientsList);
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreatePatientDto patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                await _patientService.CreatePatientAsync(patient);

                TempData["SuccessMessage"] =
                    "Patient added successfully.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(patient);
            }
        }

        // GET: Patient/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);

            return View(new UpdatePatientDto
            {
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email
            });
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UpdatePatientDto patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                await _patientService.UpdatePatientAsync(id, patient);

                TempData["SuccessMessage"] =
                    "Patient updated successfully.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(patient);
            }
        }

        // GET: Patient/Deactivate/5
        public async Task<ActionResult> Deactivate(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);

            return View("Deactivate", patient);
        }

        // POST: Patient/Deactivate/5
        [HttpPost]
        [ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeactivateConfirmed(int id)
        {
            try
            {
                await _patientService.DeletePatientAsync(id);

                TempData["SuccessMessage"] =
                    "Patient deleted successfully.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var patient = await _patientService.GetPatientByIdAsync(id);
                return View("Deactivate", patient);
            }
        }
    }
}