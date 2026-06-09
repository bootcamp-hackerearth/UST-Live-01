using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientApiService _patientService;

        public PatientController(
            IPatientApiService patientService)
        {
            _patientService = patientService;
        }

        #region Private Helpers

        private int GetPatientId()
        {
            return Convert.ToInt32(
                Session[Constants.ReferenceIdKey]);
        }

        private async Task<dynamic> GetCurrentPatientAsync()
        {
            return await _patientService
                .GetPatientByIdAsync(
                    GetPatientId());
        }

        private async Task<dynamic> GetPatientAsync(
            int patientId)
        {
            return await _patientService
                .GetPatientByIdAsync(
                    patientId);
        }

        private async Task<UpdatePatientDto> BuildUpdatePatientDtoAsync(
            int patientId)
        {
            var patient =
                await _patientService
                    .GetPatientByIdAsync(
                        patientId);

            return new UpdatePatientDto
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
            };
        }

        #endregion

        // ==================================
        // PATIENT
        // ==================================

        public async Task<ActionResult> Dashboard()
        {
            var patient =
                await GetCurrentPatientAsync();

            ViewBag.PageTitle =
                Constants.DashboardAction;

            return View(patient);
        }

        public async Task<ActionResult> MyProfile()
        {
            var patient =
                await GetCurrentPatientAsync();

            ViewBag.PageTitle =
                Constants.MyProfileAction;

            return View(patient);
        }

        public async Task<ActionResult> EditMyProfile()
        {
            return View(
                await BuildUpdatePatientDtoAsync(
                    GetPatientId()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMyProfile(
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
                        GetPatientId(),
                        patient);

                return RedirectToAction(
                    Constants.MyProfileAction);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(patient);
            }
        }

        // ==================================
        // ADMIN & DOCTOR
        // ==================================

        public async Task<ActionResult> Index(
            InsuranceStatus? status)
        {
            if (status.HasValue)
            {
                var patients =
                    await _patientService
                        .GetPatientsByInsuranceStatusAsync(
                            status.Value);

                return View(patients);
            }

            var patientsList =
                await _patientService
                    .GetAllPatientsAsync();

            return View(patientsList);
        }

        public async Task<ActionResult> Details(
            int id)
        {
            var patient =
                await GetPatientAsync(id);

            ViewBag.IsDetails =
                true;

            return View(patient);
        }

        // ==================================
        // ADMIN ONLY
        // ==================================

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            CreatePatientDto patient)
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

                return RedirectToAction(
                    Constants.DetailsAction,
                    new
                    {
                        id = patientId
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(patient);
            }
        }

        public async Task<ActionResult> Edit(
            int id)
        {
            return View(
                await BuildUpdatePatientDtoAsync(
                    id));
        }

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
                await _patientService
                    .UpdatePatientAsync(
                        id,
                        patient);

                return RedirectToAction(
                    Constants.DetailsAction,
                    new
                    {
                        id
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(patient);
            }
        }

        public async Task<ActionResult> Deactivate(
            int id)
        {
            var patient =
                await GetPatientAsync(id);

            ViewBag.IsDeactivate =
                true;

            return View(patient);
        }

        [HttpPost]
        [ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeactivateConfirmed(
            int id)
        {
            try
            {
                await _patientService
                    .DeletePatientAsync(
                        id);

                return RedirectToAction(
                    Constants.IndexAction);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                var patient =
                    await GetPatientAsync(id);

                return View(patient);
            }
        }
    }
}