using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using HealthCare_Appointment_Portal.Utilities;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class HealthRecordController : Controller
    {
        private readonly IHealthRecordApiService _healthRecordService;

        public HealthRecordController(
            IHealthRecordApiService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }

        #region Private Helpers

        private bool IsLoggedIn()
        {
            return Session[Constants.ReferenceIdKey] != null;
        }

        private int GetReferenceId()
        {
            return Convert.ToInt32(
                Session[Constants.ReferenceIdKey]);
        }

        private RedirectToRouteResult RedirectToLogin()
        {
            return RedirectToAction(
                Constants.LoginAction,
                Constants.UserController);
        }

        #endregion

        // ==================================
        // ADMIN
        // ==================================

        public async Task<ActionResult> Index()
        {
            try
            {
                var records =
                    await _healthRecordService
                        .GetAllHealthRecordsAsync();

                return View(records);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return View();
            }
        }

        // ==================================
        // COMMON
        // ==================================

        public async Task<ActionResult> Details(
            int id)
        {
            try
            {
                var record =
                    await _healthRecordService
                        .GetHealthRecordByIdAsync(id);

                return View(record);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.IndexAction);
            }
        }

        // ==================================
        // DOCTOR
        // ==================================

        public ActionResult Create(
            int appointmentId,
            int patientId)
        {
            if (!IsLoggedIn())
            {
                return RedirectToLogin();
            }

            return View(
                new CreateHealthRecordDto
                {
                    AppointmentId =
                        appointmentId,

                    PatientId =
                        patientId,

                    DoctorId =
                        GetReferenceId(),

                    VisitDate =
                        DateTime.Today
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            CreateHealthRecordDto dto)
        {
            if (!IsLoggedIn())
            {
                return RedirectToLogin();
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                dto.DoctorId =
                    GetReferenceId();

                int recordId =
                    await _healthRecordService
                        .CreateHealthRecordAsync(dto);

                TempData[Constants.SuccessKey] =
                    "Health record created successfully.";

                return RedirectToAction(
                    Constants.DetailsAction,
                    new
                    {
                        id = recordId
                    });
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return View(dto);
            }
        }

        public async Task<ActionResult> Edit(
            int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToLogin();
            }

            try
            {
                var record =
                    await _healthRecordService
                        .GetHealthRecordByIdAsync(id);

                ViewBag.RecordId =
                    record.RecordId;

                return View(
                    new UpdateHealthRecordDto
                    {
                        Diagnosis =
                            record.Diagnosis,

                        Prescription =
                            record.Prescription,

                        Notes =
                            record.Notes
                    });
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    nameof(DoctorRecords));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            int id,
            UpdateHealthRecordDto dto)
        {
            if (!IsLoggedIn())
            {
                return RedirectToLogin();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.RecordId = id;
                return View(dto);
            }

            try
            {
                await _healthRecordService
                    .UpdateHealthRecordAsync(
                        id,
                        dto);

                TempData[Constants.SuccessKey] =
                    "Health record updated successfully.";

                return RedirectToAction(
                    Constants.DetailsAction,
                    new { id });
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                ViewBag.RecordId = id;

                return View(dto);
            }
        }

        public async Task<ActionResult> DoctorRecords()
        {
            if (!IsLoggedIn())
            {
                return RedirectToLogin();
            }

            try
            {
                var records =
                    await _healthRecordService
                        .GetRecordsByDoctorAsync(
                            GetReferenceId());

                return View(records);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return View();
            }
        }

        // ==================================
        // PATIENT
        // ==================================

        public async Task<ActionResult> PatientHistory()
        {
            if (!IsLoggedIn())
            {
                return RedirectToLogin();
            }

            try
            {
                var records =
                    await _healthRecordService
                        .GetRecordsByPatientAsync(
                            GetReferenceId());

                return View(records);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return View();
            }
        }

        // ==================================
        // ADMIN ONLY
        // ==================================

        public async Task<ActionResult> Delete(
            int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToLogin();
            }

            try
            {
                var record =
                    await _healthRecordService
                        .GetHealthRecordByIdAsync(id);

                return View(record);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    Constants.IndexAction);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(
            int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToLogin();
            }

            try
            {
                await _healthRecordService
                    .DeleteHealthRecordAsync(id);

                TempData[Constants.SuccessKey] =
                    "Health record deleted successfully.";

                return RedirectToAction(
                    Constants.IndexAction);
            }
            catch (Exception ex)
            {
                TempData[Constants.ErrorKey] =
                    ex.Message;

                return RedirectToAction(
                    nameof(Delete),
                    new { id });
            }
        }
    }
}