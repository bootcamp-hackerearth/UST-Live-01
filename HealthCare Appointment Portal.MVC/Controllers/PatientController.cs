using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class PatientController : Controller
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
        // DASHBOARD
        // ==================================

        public async Task<ActionResult>
            Dashboard()
        {
            int patientId = 1;

            var patient =
                await _patientService
                    .GetPatientByIdAsync(
                        patientId);

            return View(patient);
        }

        // ==================================
        // PROFILE
        // ==================================

        public async Task<ActionResult>
            MyProfile()
        {
            int patientId = 1;

            var patient =
                await _patientService
                    .GetPatientByIdAsync(
                        patientId);

            return View(patient);
        }

        public async Task<ActionResult>
            EditMyProfile()
        {
            int patientId = 1;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            EditMyProfile(
                UpdatePatientDto patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                int patientId = 1;

                await _patientService
                    .UpdatePatientAsync(
                        patientId,
                        patient);

                TempData["Success"] =
                    "Profile updated successfully.";

                return RedirectToAction(
                    "MyProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(patient);
            }
        }

        // ==================================
        // LIST + SEARCH
        // ==================================

        public async Task<ActionResult>
            Index(string search)
        {
            var patients =
                await _patientService
                    .GetAllPatientsAsync();

            // SEARCH

            if (!string.IsNullOrWhiteSpace(search))
            {
                search =
                    search.ToLower();

                patients =
                    patients.Where(p =>
                        p.FullName.ToLower().Contains(search)
                        ||
                        p.PatientId.ToString() == search);
            }

            return View(patients);
        }

        // ==================================
        // DETAILS
        // ==================================

        public async Task<ActionResult>
            Details(int id)
        {
            var patient =
                await _patientService
                    .GetPatientByIdAsync(id);

            return View(patient);
        }

        // ==================================
        // CREATE
        // ==================================

        public ActionResult
            Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Create(CreatePatientDto patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                int patientId =
                    await _patientService
                        .CreatePatientAsync(
                            patient);

                TempData["Success"] =
                    "Patient created successfully.";

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

                return View(patient);
            }
        }

        // ==================================
        // EDIT
        // ==================================

        public async Task<ActionResult>
            Edit(int id)
        {
            var patient =
                await _patientService
                    .GetPatientByIdAsync(id);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Edit(
                int id,
                UpdatePatientDto patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                await _patientService
                    .UpdatePatientAsync(
                        id,
                        patient);

                TempData["Success"] =
                    "Patient updated successfully.";

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

                return View(patient);
            }
        }

        // ==================================
        // DELETE
        // ==================================

        public async Task<ActionResult>
            Deactivate(int id)
        {
            var patient =
                await _patientService
                    .GetPatientByIdAsync(id);

            return View(patient);
        }

        [HttpPost]
        [ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            DeactivateConfirmed(int id)
        {
            try
            {
                await _patientService
                    .DeletePatientAsync(id);

                TempData["Success"] =
                    "Patient deactivated successfully.";

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
                        .GetPatientByIdAsync(id);

                return View(patient);
            }
        }
    }
}