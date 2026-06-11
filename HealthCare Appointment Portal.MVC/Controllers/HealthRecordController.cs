using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
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

        // ==================================
        // ADMIN
        // ==================================

        // GET: HealthRecord
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
                TempData["Error"] = ex.Message;

                return View();
            }
        }

        // ==================================
        // COMMON
        // ==================================

        // GET: HealthRecord/Details/5
        public async Task<ActionResult> Details(int id)
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
                TempData["Error"] = ex.Message;

                return RedirectToAction("Index");
            }
        }

        // ==================================
        // DOCTOR
        // ==================================

        // GET: HealthRecord/Create
        public ActionResult Create(
            int appointmentId,
            int patientId)
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            return View(
                new CreateHealthRecordDto
                {
                    AppointmentId = appointmentId,

                    PatientId = patientId,

                    DoctorId = Convert.ToInt32(
                        Session["ReferenceId"]),

                    VisitDate = DateTime.Today
                });
        }

        // POST: HealthRecord/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            CreateHealthRecordDto dto)
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                dto.DoctorId =
                    Convert.ToInt32(
                        Session["ReferenceId"]);

                await _healthRecordService
                    .CreateHealthRecordAsync(dto);

                TempData["Success"] =
                    "Health record added successfully.";

                return RedirectToAction(
                    "DoctorRecords",
                    "HealthRecord");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return View(dto);
            }
        }

        // GET: HealthRecord/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            try
            {
                var record =
                    await _healthRecordService
                        .GetHealthRecordByIdAsync(id);

                ViewBag.RecordId = record.RecordId;

                return View(
                    new UpdateHealthRecordDto
                    {
                        Diagnosis = record.Diagnosis,

                        Prescription = record.Prescription,

                        Notes = record.Notes
                    });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(
                    "DoctorRecords",
                    "HealthRecord");
            }
        }

        // POST: HealthRecord/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            int id,
            UpdateHealthRecordDto dto)
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
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

                TempData["Success"] =
                    "Health record updated successfully.";

                return RedirectToAction(
                    "DoctorRecords",
                    "HealthRecord");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                ViewBag.RecordId = id;

                return View(dto);
            }
        }

        // GET: HealthRecord/DoctorRecords
        public async Task<ActionResult> DoctorRecords()
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            try
            {
                int doctorId =
                    Convert.ToInt32(
                        Session["ReferenceId"]);

                var records =
                    await _healthRecordService
                        .GetRecordsByDoctorAsync(doctorId);

                return View(records);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return View();
            }
        }

        // ==================================
        // PATIENT
        // ==================================

        // GET: HealthRecord/PatientHistory
        public async Task<ActionResult> PatientHistory()
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            try
            {
                int patientId =
                    Convert.ToInt32(
                        Session["ReferenceId"]);

                var records =
                    await _healthRecordService
                        .GetRecordsByPatientAsync(patientId);

                return View(records);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return View();
            }
        }

        // ==================================
        // ADMIN ONLY
        // ==================================

        // GET: HealthRecord/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
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
                TempData["Error"] = ex.Message;

                return RedirectToAction("Index");
            }
        }

        // POST: HealthRecord/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            try
            {
                await _healthRecordService
                    .DeleteHealthRecordAsync(id);

                TempData["Success"] =
                    "Health record deleted successfully.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(
                    "Delete",
                    new
                    {
                        id
                    });
            }
        }
    }
}