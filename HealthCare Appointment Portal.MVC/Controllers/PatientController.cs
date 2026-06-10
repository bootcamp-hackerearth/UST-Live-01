using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Linq;
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

        // ==================================
        // PATIENT DASHBOARD
        // ==================================

        // GET: Patient/Dashboard
        public async Task<ActionResult> Dashboard()
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction("Create");
            }

            int patientId =
                Convert.ToInt32(Session["ReferenceId"]);

            var patient =
                await _patientService
                    .GetPatientByIdAsync(patientId);

            return View(patient);
        }

        // ==================================
        // EXISTING PATIENT LOGIN
        // ==================================

        // POST: Patient/ExistingPatientLogin
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExistingPatientLogin(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["LoginError"] =
                    "Please enter your registered email address.";

                return RedirectToAction("Create");
            }

            var patients =
                await _patientService
                    .GetAllPatientsAsync();

            var existingPatient =
                patients.FirstOrDefault(
                    p => p.Email.ToLower() == email.ToLower());

            if (existingPatient == null)
            {
                TempData["LoginError"] =
                    "No patient found with this email. Please register as a new patient.";

                return RedirectToAction("Create");
            }

            Session["ReferenceId"] =
                existingPatient.PatientId;

            return RedirectToAction("Dashboard");
        }

        // ==================================
        // PATIENT PROFILE
        // ==================================

        // GET: Patient/MyProfile
        public async Task<ActionResult> MyProfile()
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction("Create");
            }

            int patientId =
                Convert.ToInt32(Session["ReferenceId"]);

            var patient =
                await _patientService
                    .GetPatientByIdAsync(patientId);

            return View(patient);
        }

        // GET: Patient/EditMyProfile
        public async Task<ActionResult> EditMyProfile()
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction("Create");
            }

            int patientId =
                Convert.ToInt32(Session["ReferenceId"]);

            var patient =
                await _patientService
                    .GetPatientByIdAsync(patientId);

            return View(
                new UpdatePatientDto
                {
                    FullName = patient.FullName,
                    DateOfBirth = patient.DateOfBirth,
                    Gender = patient.Gender,
                    PhoneNumber = patient.PhoneNumber,
                    Email = patient.Email
                });
        }

        // POST: Patient/EditMyProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<ActionResult> EditMyProfile(UpdatePatientDto dto)
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction("Create", "Patient");
            }

            int patientId =
                Convert.ToInt32(Session["ReferenceId"]);

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _patientService.UpdatePatientAsync(patientId, dto);

                ViewBag.Success =
                    "Profile updated successfully.";

                return View(dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(dto);
            }
        }
        // ==================================
        // ADMIN / DOCTOR PATIENT LIST
        // ==================================

        // GET: Patient
        public async Task<ActionResult> Index(InsuranceStatus? status)
        {
            if (status.HasValue)
            {
                var patients =
                    await _patientService
                        .GetPatientsByInsuranceStatusAsync(status.Value);

                return View(patients);
            }

            var patientsList =
                await _patientService
                    .GetAllPatientsAsync();

            return View(patientsList);
        }

        // GET: Patient/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var patient =
                await _patientService
                    .GetPatientByIdAsync(id);

            return View(patient);
        }

        // ==================================
        // NEW PATIENT REGISTRATION
        // ==================================

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
                int patientId =
                    await _patientService
                        .CreatePatientAsync(patient);

                Session["ReferenceId"] =
                    patientId;

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(patient);
            }
        }

        // ==================================
        // ADMIN EDIT PATIENT
        // ==================================

        // GET: Patient/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var patient =
                await _patientService
                    .GetPatientByIdAsync(id);

            return View(
                new UpdatePatientDto
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
                await _patientService
                    .UpdatePatientAsync(id, patient);

                return RedirectToAction(
                    "Details",
                    new
                    {
                        id
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(patient);
            }
        }

        // ==================================
        // ADMIN DEACTIVATE PATIENT
        // ==================================

        // GET: Patient/Deactivate/5
        public async Task<ActionResult> Deactivate(int id)
        {
            var patient =
                await _patientService
                    .GetPatientByIdAsync(id);

            return View(patient);
        }

        // POST: Patient/Deactivate/5
        [HttpPost]
        [ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeactivateConfirmed(int id)
        {
            try
            {
                await _patientService
                    .DeletePatientAsync(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var patient =
                    await _patientService
                        .GetPatientByIdAsync(id);

                return View(patient);
            }
        }
    }
}