using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class HealthRecordController : Controller
    {
        // ==================================
        // CONSTANTS (Fixes Sonar S1192)
        // ==================================
        private const string ErrorKey = "Error";
        private const string SuccessKey = "Success";
        private const string ReferenceIdKey = "ReferenceId";
        private const string LoginAction = "Login";
        private const string UserController = "User";

        private readonly IHealthRecordApiService _healthRecordService;
        private readonly IAppointmentApiService _appointmentService;

        public HealthRecordController(
            IHealthRecordApiService healthRecordService,
            IAppointmentApiService appointmentService)
        {
            _healthRecordService = healthRecordService;
            _appointmentService = appointmentService;
        }

        // ==================================
        // ADMIN
        // ==================================

        // GET: HealthRecord
        public async Task<ActionResult> Index()
        {
            try
            {
                var records = await _healthRecordService.GetAllHealthRecordsAsync();
                return View(records);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
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
                var record = await _healthRecordService.GetHealthRecordByIdAsync(id);
                return View(record);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // ==================================
        // DOCTOR
        // ==================================

        public ActionResult Create(int appointmentId, int patientId, int doctorId)
        {
            return View(new CreateHealthRecordDto
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                DoctorId = doctorId,
                VisitDate = DateTime.Today
            });
        }

        // POST: HealthRecord/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                int recordId = await _healthRecordService.CreateHealthRecordAsync(dto);
                TempData[SuccessKey] = "Health record created successfully.";
                return RedirectToAction("Details", new { id = recordId });
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return View(dto);
            }
        }

        // GET: HealthRecord/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (Session[ReferenceIdKey] == null)
            {
                return RedirectToAction(LoginAction, UserController);
            }

            try
            {
                var record = await _healthRecordService.GetHealthRecordByIdAsync(id);
                ViewBag.RecordId = record.RecordId;

                return View(new UpdateHealthRecordDto
                {
                    Diagnosis = record.Diagnosis,
                    Prescription = record.Prescription,
                    Notes = record.Notes
                });
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return RedirectToAction("DoctorRecords");
            }
        }

        // POST: HealthRecord/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UpdateHealthRecordDto dto)
        {
            if (Session[ReferenceIdKey] == null)
            {
                return RedirectToAction(LoginAction, UserController);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.RecordId = id;
                return View(dto);
            }

            try
            {
                await _healthRecordService.UpdateHealthRecordAsync(id, dto);
                TempData[SuccessKey] = "Health record updated successfully.";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                ViewBag.RecordId = id;
                return View(dto);
            }
        }

        // GET: HealthRecord/DoctorRecords
        public async Task<ActionResult> DoctorRecords()
        {
            if (Session[ReferenceIdKey] == null)
            {
                return RedirectToAction(LoginAction, UserController);
            }

            try
            {
                int doctorId = Convert.ToInt32(Session[ReferenceIdKey]);
                var records = await _healthRecordService.GetRecordsByDoctorAsync(doctorId);
                return View(records);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return View();
            }
        }

        // ==================================
        // PATIENT
        // ==================================

        // GET: HealthRecord/PatientHistory
        public async Task<ActionResult> PatientHistory()
        {
            if (Session[ReferenceIdKey] == null)
            {
                return RedirectToAction(LoginAction, UserController);
            }

            try
            {
                int patientId = Convert.ToInt32(Session[ReferenceIdKey]);
                var records = await _healthRecordService.GetRecordsByPatientAsync(patientId);
                return View(records);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return View();
            }
        }

        // ==================================
        // ADMIN ONLY
        // ==================================

        // GET: HealthRecord/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (Session[ReferenceIdKey] == null)
            {
                return RedirectToAction(LoginAction, UserController);
            }

            try
            {
                var record = await _healthRecordService.GetHealthRecordByIdAsync(id);
                return View(record);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: HealthRecord/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (Session[ReferenceIdKey] == null)
            {
                return RedirectToAction(LoginAction, UserController);
            }

            try
            {
                await _healthRecordService.DeleteHealthRecordAsync(id);
                TempData[SuccessKey] = "Health record deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return RedirectToAction("Delete", new { id });
            }
        }

        public async Task<ActionResult> SelectAppointment()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            var records = await _healthRecordService.GetAllHealthRecordsAsync();
            var usedAppointmentIds = records.Select(r => r.AppointmentId);

            var completedAppointments = appointments
                .Where(a => a.Status.ToString() == "Completed" && !usedAppointmentIds.Contains(a.AppointmentId))
                .ToList();

            return View(completedAppointments);
        }
    }
}