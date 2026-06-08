using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class PatientController
        : Controller
    {
        private readonly IPatientApiService
            _patientService;

        public PatientController(
            IPatientApiService patientService)
        {
            _patientService =
                patientService;
        }

        // ==================================
        // PATIENT
        // ==================================

        // GET: Patient/Dashboard
        public async Task<ActionResult>
            Dashboard()
        {
            int patientId =
                Convert.ToInt32(
                    Session["ReferenceId"]);

            var patient =
                await _patientService
                    .GetPatientByIdAsync(
                        patientId);

            return View(
                patient);
        }

        // GET: Patient/MyProfile
        public async Task<ActionResult>
            MyProfile()
        {
            int patientId =
                Convert.ToInt32(
                    Session["ReferenceId"]);

            var patient =
                await _patientService
                    .GetPatientByIdAsync(
                        patientId);

            return View(
                patient);
        }

        // GET: Patient/EditMyProfile
        public async Task<ActionResult>
            EditMyProfile()
        {
            int patientId =
                Convert.ToInt32(
                    Session["ReferenceId"]);

            var patient =
                await _patientService
                    .GetPatientByIdAsync(
                        patientId);

            return View(
                new UpdatePatientDto
                {
                    FullName =
                        patient.FullName,

                    DateOfBirth =
                        patient.DateOfBirth,

                    Gender =
                        patient.Gender,

                    PhoneNumber =
                        patient.PhoneNumber,

                    Email =
                        patient.Email
                });
        }

        // POST: Patient/EditMyProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            EditMyProfile(
                UpdatePatientDto patient)
        {
            if (!ModelState.IsValid)
            {
                return View(
                    patient);
            }

            try
            {
                int patientId =
                    Convert.ToInt32(
                        Session["ReferenceId"]);

                await _patientService
                    .UpdatePatientAsync(
                        patientId,
                        patient);

                return RedirectToAction(
                    "MyProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(
                    patient);
            }
        }

        // ==================================
        // ADMIN & DOCTOR
        // ==================================

        // GET: Patient
        public async Task<ActionResult>
            Index(
                InsuranceStatus? status)
        {
            if (status.HasValue)
            {
                var patients =
                    await _patientService
                        .GetPatientsByInsuranceStatusAsync(
                            status.Value);

                return View(
                    patients);
            }

            var patientsList =
                await _patientService
                    .GetAllPatientsAsync();

            return View(
                patientsList);
        }

        // GET: Patient/Details/5
        public async Task<ActionResult>
            Details(
                int id)
        {
            var patient =
                await _patientService
                    .GetPatientByIdAsync(
                        id);

            return View(
                patient);
        }

        // ==================================
        // ADMIN ONLY
        // ==================================

        // GET: Patient/Create
        public ActionResult
            Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Create(
                CreatePatientDto patient)
        {
            if (!ModelState.IsValid)
            {
                return View(
                    patient);
            }

            try
            {
                int patientId =
                    await _patientService
                        .CreatePatientAsync(
                            patient);

                return RedirectToAction(
                    "Details",
                    new
                    {
                        id = patientId
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(
                    patient);
            }
        }

        // GET: Patient/Edit/5
        public async Task<ActionResult>
            Edit(
                int id)
        {
            var patient =
                await _patientService
                    .GetPatientByIdAsync(
                        id);

            return View(
                new UpdatePatientDto
                {
                    FullName =
                        patient.FullName,

                    DateOfBirth =
                        patient.DateOfBirth,

                    Gender =
                        patient.Gender,

                    PhoneNumber =
                        patient.PhoneNumber,

                    Email =
                        patient.Email
                });
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Edit(
                int id,
                UpdatePatientDto patient)
        {
            if (!ModelState.IsValid)
            {
                return View(
                    patient);
            }

            try
            {
                await _patientService
                    .UpdatePatientAsync(
                        id,
                        patient);

                return RedirectToAction(
                    "Details",
                    new
                    {
                        id
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(
                    patient);
            }
        }

        // GET: Patient/Deactivate/5
        public async Task<ActionResult>
            Deactivate(
                int id)
        {
            var patient =
                await _patientService
                    .GetPatientByIdAsync(
                        id);

            return View(
                patient);
        }

        // POST: Patient/Deactivate/5
        [HttpPost]
        [ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            DeactivateConfirmed(
                int id)
        {
            try
            {
                await _patientService
                    .DeletePatientAsync(
                        id);

                return RedirectToAction(
                    "Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                var patient =
                    await _patientService
                        .GetPatientByIdAsync(
                            id);

                return View(
                    patient);
            }
        }
    }
}