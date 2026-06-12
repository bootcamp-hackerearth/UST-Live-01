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
        private readonly IHealthRecordApiService _healthRecordService;
        private readonly IAppointmentApiService _appointmentService;

        public HealthRecordController(
            IHealthRecordApiService healthRecordService,
            IAppointmentApiService appointmentService)
        {
            _healthRecordService = healthRecordService;
            _appointmentService = appointmentService;
        }

        // GET: HealthRecord/Create?appointmentId=5
        public async Task<ActionResult> Create(int? appointmentId)
        {
            if (appointmentId == null)
            {
                TempData["Error"] = "Appointment Id is missing.";
                return RedirectToAction("UpcomingAppointments", "Doctor");
            }

            try
            {
                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(appointmentId.Value);

                var dto =
                    new CreateHealthRecordDto
                    {
                        AppointmentId = appointment.AppointmentId,
                        PatientId = appointment.PatientId,
                        DoctorId = appointment.DoctorId,
                        VisitDate = appointment.ScheduledDate
                    };

                ViewBag.VisitDateText =
                    appointment.ScheduledDate.ToString("dd-MMM-yyyy");

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("UpcomingAppointments", "Doctor");
            }
        }

        // POST: HealthRecord/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateHealthRecordDto dto)
        {
            try
            {
                if (dto.AppointmentId <= 0)
                {
                    ModelState.AddModelError("AppointmentId", "Appointment Id is required.");
                }

                if (string.IsNullOrWhiteSpace(dto.Diagnosis))
                {
                    ModelState.AddModelError("Diagnosis", "Diagnosis is required.");
                }

                if (string.IsNullOrWhiteSpace(dto.Prescription))
                {
                    ModelState.AddModelError("Prescription", "Prescription is required.");
                }

                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(dto.AppointmentId);

                dto.PatientId = appointment.PatientId;
                dto.DoctorId = appointment.DoctorId;
                dto.VisitDate = appointment.ScheduledDate;

                ViewBag.VisitDateText =
                    appointment.ScheduledDate.ToString("dd-MMM-yyyy");

                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                await _healthRecordService
                    .CreateHealthRecordAsync(dto);

                TempData["Success"] =
                    "Health record saved successfully.";

                return RedirectToAction("Dashboard", "Doctor");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                ViewBag.VisitDateText =
                    dto.VisitDate.ToString("dd-MMM-yyyy");

                return View(dto);
            }
        }

        // GET: HealthRecord/PatientHistory?patientId=4
        public async Task<ActionResult> PatientHistory(int? patientId)
        {
            try
            {
                if (patientId == null)
                {
                    if (Session["ReferenceId"] == null)
                    {
                        return RedirectToAction("Create", "Patient");
                    }

                    patientId =
                        Convert.ToInt32(Session["ReferenceId"]);
                }

                var records =
                    await _healthRecordService
                        .GetRecordsByPatientAsync(patientId.Value);

                return View(records);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Dashboard", "Patient");
            }
        }

        // GET: HealthRecord/DoctorRecords
        public async Task<ActionResult> DoctorRecords()
        {
            try
            {
                if (Session["ReferenceId"] == null)
                {
                    return RedirectToAction("Create", "Doctor");
                }

                int doctorId =
                    Convert.ToInt32(Session["ReferenceId"]);

                var records =
                    await _healthRecordService
                        .GetRecordsByDoctorAsync(doctorId);

                var orderedRecords =
                    records
                        .OrderByDescending(r => r.VisitDate)
                        .ToList();

                return View(orderedRecords);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Dashboard", "Doctor");
            }
        }

        // GET: HealthRecord/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Health Record Id is missing.";
                return RedirectToAction("DoctorRecords");
            }

            try
            {
                var record =
                    await _healthRecordService
                        .GetHealthRecordByIdAsync(id.Value);

                return View(record);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("DoctorRecords");
            }
        }

        // GET: HealthRecord/Edit/5
        public async Task<ActionResult> Edit(int? id, string returnUrl = null)
        {
            if (id == null)
            {
                TempData["Error"] = "Health Record Id is missing.";
                return RedirectToAction("DoctorRecords", "HealthRecord");
            }

            try
            {
                var record = await _healthRecordService.GetHealthRecordByIdAsync(id.Value);

                var dto = new UpdateHealthRecordDto
                {
                    Diagnosis = record.Diagnosis,
                    Prescription = record.Prescription,
                    Notes = record.Notes
                };

                ViewBag.RecordId = id.Value;
                ViewBag.ReturnUrl = returnUrl ?? Url.Action("DoctorRecords", "HealthRecord");

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("DoctorRecords", "HealthRecord");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UpdateHealthRecordDto dto, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RecordId = id;
                ViewBag.ReturnUrl = returnUrl ?? Url.Action("DoctorRecords", "HealthRecord");
                return View(dto);
            }

            try
            {
                await _healthRecordService.UpdateHealthRecordAsync(id, dto);

                TempData["Success"] = "Health record updated successfully.";

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("DoctorRecords", "HealthRecord");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                ViewBag.RecordId = id;
                ViewBag.ReturnUrl = returnUrl ?? Url.Action("DoctorRecords", "HealthRecord");

                return View(dto);
            }
        }
    }
}


