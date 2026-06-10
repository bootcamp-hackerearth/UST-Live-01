using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using HealthCare_Appointment_Portal.Utilities;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Globalization;

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

        public async Task<ActionResult> Index(
            string patient = "",
            string doctor = "",
            string diagnosis = "",
            string visitDate = "",
            int page = 1)
        {
            try
            {
                var records =
                    (await _healthRecordService
                        .GetAllHealthRecordsAsync())
                    .ToList();

                if (!string.IsNullOrWhiteSpace(
                    patient))
                {
                    records =
                        records
                        .Where(r =>
                            !string.IsNullOrWhiteSpace(
                                r.PatientName)
                            &&
                            r.PatientName
                            .IndexOf(
                                patient,
                                StringComparison
                                .OrdinalIgnoreCase) >= 0)
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(
                    doctor))
                {
                    records =
                        records
                        .Where(r =>
                            !string.IsNullOrWhiteSpace(
                                r.DoctorName)
                            &&
                            r.DoctorName
                            .IndexOf(
                                doctor,
                                StringComparison
                                .OrdinalIgnoreCase) >= 0)
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(
                    diagnosis))
                {
                    records =
                        records
                        .Where(r =>
                            !string.IsNullOrWhiteSpace(
                                r.Diagnosis)
                            &&
                            r.Diagnosis
                            .IndexOf(
                                diagnosis,
                                StringComparison
                                .OrdinalIgnoreCase) >= 0)
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(
                    visitDate))
                {
                    DateTime selectedDate;

                    if (DateTime.TryParse(
                        visitDate,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out selectedDate))
                    {
                        records =
                            records
                            .Where(r =>
                                r.VisitDate.Date ==
                                selectedDate.Date)
                            .ToList();
                    }
                }

                records =
                    records
                    .OrderByDescending(
                        r => r.VisitDate)
                    .ThenByDescending(
                        r => r.RecordId)
                    .ToList();

                const int pageSize = 5;

                int totalRecords =
                    records.Count;

                int totalPages =
                    Math.Max(
                        1,
                        (int)Math.Ceiling(
                            (double)totalRecords /
                            pageSize));

                if (page < 1)
                {
                    page = 1;
                }

                if (page > totalPages)
                {
                    page = totalPages;
                }

                var pagedRecords =
                    records
                    .Skip(
                        (page - 1) *
                        pageSize)
                    .Take(
                        pageSize)
                    .ToList();

                ViewBag.TotalRecords =
                    totalRecords;

                ViewBag.CurrentPage =
                    page;

                ViewBag.TotalPages =
                    totalPages;

                ViewBag.Patient =
                    patient;

                ViewBag.Doctor =
                    doctor;

                ViewBag.Diagnosis =
                    diagnosis;

                ViewBag.VisitDate =
                    visitDate;

                return View(
                    pagedRecords);
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;

                return View(
                    Enumerable.Empty<
                        HealthRecordDto>());
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

        [HttpGet]
        public ActionResult Create(
            int appointmentId,
            int patientId,
            int? doctorId)
        {
            var model =
                new CreateHealthRecordDto
                {
                    AppointmentId = appointmentId,
                    PatientId = patientId,
                    DoctorId = doctorId ?? 0,
                    VisitDate = DateTime.Today
                };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            CreateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
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

        public async Task<ActionResult> DoctorRecords(
            string patient = "",
            string diagnosis = "",
            string visitDate = "",
            int page = 1)
        {
            if (!IsLoggedIn())
            {
                return RedirectToLogin();
            }

            try
            {
                var records =
                    (await _healthRecordService
                        .GetRecordsByDoctorAsync(
                            GetReferenceId()))
                    .ToList();

                ViewBag.Patients =
                    records
                    .Select(r => r.PatientName)
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                ViewBag.VisitDates =
                    records
                    .Select(r => r.VisitDate.Date)
                    .Distinct()
                    .OrderByDescending(x => x)
                    .ToList();

                if (!string.IsNullOrWhiteSpace(
                    patient))
                {
                    records =
                        records
                        .Where(r =>
                            !string.IsNullOrWhiteSpace(
                                r.PatientName)
                            &&
                            r.PatientName
                            .ToLower()
                            .Contains(
                                patient
                                .ToLower()))
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(
                    diagnosis))
                {
                    records =
                        records
                        .Where(r =>
                            !string.IsNullOrWhiteSpace(
                                r.Diagnosis)
                            &&
                            r.Diagnosis
                            .ToLower()
                            .Contains(
                                diagnosis
                                .ToLower()))
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(
                    visitDate))
                {
                    DateTime selectedDate;

                    if (DateTime.TryParse(
                        visitDate,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out selectedDate))
                    {
                        records =
                            records
                            .Where(r =>
                                r.VisitDate.Date ==
                                selectedDate.Date)
                            .ToList();
                    }
                }

                const int pageSize = 5;

                int totalRecords =
                    records.Count;

                ViewBag.TotalRecords =
                    totalRecords;

                int totalPages =
                    Math.Max(
                        1,
                        (int)Math.Ceiling(
                            (double)totalRecords /
                            pageSize));

                if (page < 1)
                {
                    page = 1;
                }

                if (page > totalPages)
                {
                    page = totalPages;
                }

                var pagedRecords =
                    records
                    .OrderByDescending(
                        r => r.VisitDate)
                    .Skip(
                        (page - 1) *
                        pageSize)
                    .Take(
                        pageSize)
                    .ToList();

                ViewBag.CurrentPage =
                    page;

                ViewBag.TotalPages =
                    totalPages;

                ViewBag.Patient =
                    patient;

                ViewBag.Diagnosis =
                    diagnosis;

                ViewBag.VisitDate =
                    visitDate;

                return View(
                    pagedRecords);
            }
            catch (Exception ex)
            {
                TempData[
                    Constants.ErrorKey] =
                    ex.Message;

                return View();
            }
        }

        // ==================================
        // PATIENT
        // ==================================

        public async Task<ActionResult> PatientHistory(
            string doctor = "",
            string specialisation = "",
            string visitDate = "",
            string diagnosis = "",
            int page = 1)
        {
            if (!IsLoggedIn())
            {
                return RedirectToLogin();
            }

            try
            {
                var records =
                    (await _healthRecordService
                        .GetRecordsByPatientAsync(
                            GetReferenceId()))
                    .ToList();

                ViewBag.Doctors =
                    records
                    .Select(r => r.DoctorName)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                ViewBag.Specialisations =
                    records
                    .Select(r => r.Specialisation)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                ViewBag.VisitDates =
                    records
                    .Select(r => r.VisitDate.Date)
                    .Distinct()
                    .OrderByDescending(x => x)
                    .ToList();

                if (!string.IsNullOrWhiteSpace(doctor))
                {
                    records =
                        records
                        .Where(r =>
                            r.DoctorName ==
                            doctor)
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(
                    specialisation))
                {
                    records =
                        records
                        .Where(r =>
                            r.Specialisation ==
                            specialisation)
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(
                    visitDate))
                {
                    var selectedDate =
                           DateTime.Parse(
                           visitDate,
                           CultureInfo.InvariantCulture);

                    records =
                        records
                        .Where(r =>
                            r.VisitDate.Date ==
                            selectedDate.Date)
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(
                    diagnosis))
                {
                    records =
                        records
                        .Where(r =>
                            r.Diagnosis != null
                            &&
                            r.Diagnosis
                                .ToLower()
                                .Contains(
                                    diagnosis
                                    .ToLower()))
                        .ToList();
                }

                const int pageSize = 5;

                int totalRecords =
                    records.Count;

                ViewBag.TotalRecords =
                    totalRecords;

                int totalPages =
                    Math.Max(
                        1,
                        (int)Math.Ceiling(
                            (double)totalRecords /
                            pageSize));

                if (page < 1)
                {
                    page = 1;
                }

                if (page > totalPages)
                {
                    page = totalPages;
                }

                var pagedRecords =
                    records
                    .Skip(
                        (page - 1) *
                        pageSize)
                    .Take(
                        pageSize)
                    .ToList();

                ViewBag.CurrentPage =
                    page;

                ViewBag.TotalPages =
                    totalPages;

                ViewBag.Doctor =
                    doctor;

                ViewBag.Specialisation =
                    specialisation;

                ViewBag.VisitDate =
                    visitDate;

                ViewBag.Diagnosis =
                    diagnosis;

                return View(
                    pagedRecords);
            }
            catch (Exception ex)
            {
                TempData[
                    Constants.ErrorKey] =
                    ex.Message;

                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult>
            GetRecordDetailsModal(
        int id)
        {
            try
            {
                var record =
                    await _healthRecordService
                        .GetHealthRecordByIdAsync(
                            id);

                return PartialView(
                    "_HealthRecordDetailsModal",
                    record);
            }
            catch (Exception ex)
            {
                return Content(
                    $"<div class='alert alert-danger'>{ex.Message}</div>");
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