using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;
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

        public PatientController(
            IPatientApiService patientService)
        {
            _patientService =
                patientService;
        }

        #region Private Helpers

        private int GetPatientId()
        {
            if (Session[
                Constants.ReferenceIdKey] == null)
            {
                throw new InvalidOperationException(
                    "Patient session expired.");
            }

            return Convert.ToInt32(
                Session[
                    Constants.ReferenceIdKey]);
        }

        private async Task<PatientDto>
            GetCurrentPatientAsync()
        {
            return await _patientService
                .GetPatientByIdAsync(
                    GetPatientId());
        }

        private async Task<PatientDto>
            GetPatientAsync(
                int patientId)
        {
            return await _patientService
                .GetPatientByIdAsync(
                    patientId);
        }

        private static UpdatePatientDto
            MapToUpdatePatientDto(
                PatientDto patient)
        {
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

        private ActionResult ReturnErrorView(
            Exception ex,
            object model)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(model);
        }

        #endregion

        #region Patient

        public async Task<ActionResult>
            Dashboard()
        {
            var patient =
                await GetCurrentPatientAsync();

            ViewBag.PageTitle =
                Constants.DashboardAction;

            return View(patient);
        }

        public async Task<ActionResult>
            MyProfile()
        {
            var patient =
                await GetCurrentPatientAsync();

            ViewBag.PageTitle =
                Constants.MyProfileAction;

            return View(patient);
        }

        public async Task<ActionResult>
            EditMyProfile()
        {
            var patient =
                await GetCurrentPatientAsync();

            return View(
                MapToUpdatePatientDto(
                    patient));
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
                await _patientService
                    .UpdatePatientAsync(
                        GetPatientId(),
                        patient);

                TempData[
                    Constants.SuccessKey] =
                    "Profile updated successfully.";

                return RedirectToAction(
                    Constants.MyProfileAction);
            }
            catch (Exception ex)
            {
                return ReturnErrorView(
                    ex,
                    patient);
            }
        }

        #endregion

        #region Admin And Doctor

        public async Task<ActionResult>
            Index(
                string patientId = "",
                string patientName = "",
                string email = "",
                InsuranceStatus? status = null,
                int page = 1)
        {
            IEnumerable<PatientDto>
                patients;

            if (status.HasValue)
            {
                patients =
                    await _patientService
                        .GetPatientsByInsuranceStatusAsync(
                            status.Value);
            }
            else
            {
                patients =
                    await _patientService
                        .GetAllPatientsAsync();
            }

            var filteredPatients =
                patients.ToList();

            if (!string.IsNullOrWhiteSpace(
                patientId))
            {
                filteredPatients =
                    filteredPatients
                    .Where(p =>
                        p.PatientId
                         .ToString()
                         .Contains(
                             patientId))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(
                patientName))
            {
                filteredPatients =
                    filteredPatients
                    .Where(p =>
                        !string.IsNullOrWhiteSpace(
                            p.FullName)
                        &&
                        p.FullName
                         .ToLower()
                         .Contains(
                             patientName
                             .ToLower()))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(
                email))
            {
                filteredPatients =
                    filteredPatients
                    .Where(p =>
                        !string.IsNullOrWhiteSpace(
                            p.Email)
                        &&
                        p.Email
                         .ToLower()
                         .Contains(
                             email
                             .ToLower()))
                    .ToList();
            }

            const int pageSize = 5;

            int totalRecords =
                filteredPatients.Count;

            int totalPages =
                Math.Max(
                    1,
                    (int)Math.Ceiling(
                        (double)totalRecords /
                        pageSize));

            page =
                Math.Max(
                    1,
                    Math.Min(
                        page,
                        totalPages));

            var pagedPatients =
                filteredPatients
                .OrderBy(
                    p => p.PatientId)
                .Skip(
                    (page - 1)
                    * pageSize)
                .Take(
                    pageSize)
                .ToList();

            ViewBag.PatientId =
                patientId;

            ViewBag.PatientName =
                patientName;

            ViewBag.Email =
                email;

            ViewBag.Status =
                status;

            ViewBag.TotalRecords =
                totalRecords;

            ViewBag.CurrentPage =
                page;

            ViewBag.TotalPages =
                totalPages;

            return View(
                pagedPatients);
        }

        public async Task<ActionResult>
            Details(
                int id)
        {
            var patient =
                await GetPatientAsync(id);

            ViewBag.IsDetails =
                true;

            return View(patient);
        }

        public async Task<ActionResult>
            PatientDetailsModal(
                int id)
        {
            var patient =
                await GetPatientAsync(id);

            return PartialView(
                "_PatientDetailsModal",
                patient);
        }

        [HttpPost]
        public async Task<JsonResult>
            DeletePatient(
                int id)
        {
            try
            {
                await _patientService
                    .DeletePatientAsync(
                        id);

                return Json(
                    new
                    {
                        success = true,
                        message =
                            "Patient deleted successfully."
                    });
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false,
                        message =
                            ex.Message
                    });
            }
        }

        #endregion

        #region Admin Only

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Create(
                CreatePatientDto patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                await _patientService
                    .CreatePatientAsync(
                        patient);

                TempData[
                    Constants.SuccessKey] =
                    "Patient created successfully.";

                return RedirectToAction(
                    Constants.IndexAction);
            }
            catch (Exception ex)
            {
                return ReturnErrorView(
                    ex,
                    patient);
            }
        }

        public async Task<ActionResult>
            Edit(
                int id)
        {
            var patient =
                await GetPatientAsync(id);

            return View(
                MapToUpdatePatientDto(
                    patient));
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

                TempData[
                    Constants.SuccessKey] =
                    "Patient updated successfully.";

                return RedirectToAction(
                    Constants.IndexAction);
            }
            catch (Exception ex)
            {
                return ReturnErrorView(
                    ex,
                    patient);
            }
        }

        #endregion
    }
}