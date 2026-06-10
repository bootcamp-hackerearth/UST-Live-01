using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Collections.Generic;
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

        // GET: Patient/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Patient/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                ViewBag.Error = "Please enter Patient ID.";
                return View();
            }

            int patientId;

            if (!int.TryParse(userId, out patientId))
            {
                ViewBag.Error = "Invalid Patient ID. Please enter numbers only.";
                return View();
            }

            if (patientId <= 0)
            {
                ViewBag.Error = "Invalid Patient ID. Please enter a valid Patient ID.";
                return View();
            }

            try
            {
                var patient = await _patientService.GetPatientByIdAsync(patientId);

                if (patient == null)
                {
                    ViewBag.Error = "Patient not found. Please enter a valid Patient ID.";
                    return View();
                }

                Session["ReferenceId"] = patientId;

                return RedirectToAction("Dashboard");
            }
            catch (Exception)
            {
                ViewBag.Error = "Patient not found. Please enter a valid Patient ID.";
                return View();
            }
        }

        // ==================================
        // PATIENT
        // ==================================

        // GET: Patient/Dashboard
        public async Task<ActionResult> Dashboard()
        {
            try
            {
                int patientId;

                if (!TryGetPatientIdFromSession(out patientId))
                {
                    TempData["Error"] = "Session expired or invalid. Please login again.";
                    return RedirectToAction("Login");
                }

                var patient = await _patientService.GetPatientByIdAsync(patientId);

                if (patient == null)
                {
                    Session.Remove("ReferenceId");
                    TempData["Error"] = "Patient not found. Please login with a valid Patient ID.";
                    return RedirectToAction("Login");
                }

                return View(patient);
            }
            catch (Exception)
            {
                Session.Remove("ReferenceId");
                TempData["Error"] = "Unable to load patient dashboard. Please login with a valid Patient ID.";
                return RedirectToAction("Login");
            }
        }

        // GET: Patient/MyProfile
        public async Task<ActionResult> MyProfile()
        {
            try
            {
                int patientId;

                if (!TryGetPatientIdFromSession(out patientId))
                {
                    TempData["Error"] = "Session expired or invalid. Please login again.";
                    return RedirectToAction("Login");
                }

                var patient = await _patientService.GetPatientByIdAsync(patientId);

                if (patient == null)
                {
                    Session.Remove("ReferenceId");
                    TempData["Error"] = "Patient not found. Please login with a valid Patient ID.";
                    return RedirectToAction("Login");
                }

                return View(patient);
            }
            catch (Exception)
            {
                Session.Remove("ReferenceId");
                TempData["Error"] = "Unable to load patient profile. Please login again.";
                return RedirectToAction("Login");
            }
        }

        // GET: Patient/EditMyProfile
        public async Task<ActionResult> EditMyProfile()
        {
            try
            {
                int patientId;

                if (!TryGetPatientIdFromSession(out patientId))
                {
                    TempData["Error"] = "Session expired or invalid. Please login again.";
                    return RedirectToAction("Login");
                }

                var patient = await _patientService.GetPatientByIdAsync(patientId);

                if (patient == null)
                {
                    Session.Remove("ReferenceId");
                    TempData["Error"] = "Patient not found. Please login with a valid Patient ID.";
                    return RedirectToAction("Login");
                }

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
            catch (Exception)
            {
                Session.Remove("ReferenceId");
                TempData["Error"] = "Unable to load profile for editing. Please login again.";
                return RedirectToAction("Login");
            }
        }

        // POST: Patient/EditMyProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMyProfile(UpdatePatientDto patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                int patientId;

                if (!TryGetPatientIdFromSession(out patientId))
                {
                    TempData["Error"] = "Session expired or invalid. Please login again.";
                    return RedirectToAction("Login");
                }

                await _patientService.UpdatePatientAsync(patientId, patient);

                return RedirectToAction("MyProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(patient);
            }
        }

        // ==================================
        // ADMIN & DOCTOR
        // ==================================

        // GET: Patient
        public async Task<ActionResult> Index(
            InsuranceStatus? status,
            string sortOrder)
        {
            IEnumerable<PatientDto> patients;

            try
            {
                if (status.HasValue)
                {
                    patients =
                        await _patientService
                            .GetPatientsByInsuranceStatusAsync(status.Value);
                }
                else
                {
                    patients =
                        await _patientService
                            .GetAllPatientsAsync();
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        patients = patients
                            .OrderByDescending(p => p.FullName.ToLower());
                        break;

                    default:
                        patients = patients
                            .OrderBy(p => p.FullName.ToLower());
                        break;
                }

                ViewBag.CurrentSort = sortOrder;

                ViewBag.NameSortParm =
                    string.IsNullOrEmpty(sortOrder)
                        ? "name_desc"
                        : "";

                ViewBag.SelectedStatus = status;

                return View(patients);
            }
            catch (Exception)
            {
                TempData["Error"] = "Unable to load patients.";

                return View(new List<PatientDto>());
            }
        }

        // GET: Patient/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Invalid Patient ID.";
                    return RedirectToAction("Index");
                }

                var patient =
                    await _patientService
                        .GetPatientByIdAsync(id);

                if (patient == null)
                {
                    TempData["Error"] = "Patient not found.";
                    return RedirectToAction("Index");
                }

                return View(patient);
            }
            catch (Exception)
            {
                TempData["Error"] = "Patient not found.";

                return RedirectToAction("Index");
            }
        }

        // ==================================
        // ADMIN ONLY
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

                return RedirectToAction(
                    "Details",
                    new
                    {
                        id = patientId
                    });
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
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Invalid Patient ID.";
                    return RedirectToAction("Index");
                }

                var patient =
                    await _patientService
                        .GetPatientByIdAsync(id);

                if (patient == null)
                {
                    TempData["Error"] = "Patient not found.";
                    return RedirectToAction("Index");
                }

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
            catch (Exception)
            {
                TempData["Error"] = "Patient not found.";

                return RedirectToAction("Index");
            }
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            int id,
            UpdatePatientDto patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Invalid Patient ID.";
                    return RedirectToAction("Index");
                }

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

        // GET: Patient/Deactivate/5
        public async Task<ActionResult> Deactivate(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Invalid Patient ID.";
                    return RedirectToAction("Index");
                }

                var patient =
                    await _patientService
                        .GetPatientByIdAsync(id);

                if (patient == null)
                {
                    TempData["Error"] = "Patient not found.";
                    return RedirectToAction("Index");
                }

                return View(patient);
            }
            catch (Exception)
            {
                TempData["Error"] = "Patient not found.";

                return RedirectToAction("Index");
            }
        }

        // POST: Patient/Deactivate/5
        [HttpPost]
        [ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeactivateConfirmed(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "Invalid Patient ID.";
                    return RedirectToAction("Index");
                }

                await _patientService
                    .DeletePatientAsync(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                try
                {
                    var patient =
                        await _patientService
                            .GetPatientByIdAsync(id);

                    return View(patient);
                }
                catch
                {
                    TempData["Error"] = "Unable to deactivate patient.";

                    return RedirectToAction("Index");
                }
            }
        }

        // ==================================
        // PRIVATE HELPER
        // ==================================

        private bool TryGetPatientIdFromSession(out int patientId)
        {
            patientId = 0;

            if (Session["ReferenceId"] == null)
            {
                return false;
            }

            return int.TryParse(
                Session["ReferenceId"].ToString(),
                out patientId);
        }
    }
}