using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

public class AppointmentController : Controller
{
    private readonly IAppointmentApiService _appointmentService;
    private readonly IDoctorApiService _doctorService;
    private readonly IHealthRecordApiService _healthRecordService;

    public AppointmentController(
        IAppointmentApiService appointmentService,
        IDoctorApiService doctorService,
        IHealthRecordApiService healthRecordService)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
        _healthRecordService = healthRecordService;
    }

    // ==================================
    // ADMIN
    // ==================================

    public async Task<ActionResult> Index()
    {
        try
        {
            var appointments =
                await _appointmentService.GetAllAppointmentsAsync();

            return View(appointments);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return View(Enumerable.Empty<AppointmentDto>());
        }
    }

    // ==================================
    // COMMON
    // ==================================

    public async Task<ActionResult> Details(int id)
    {
        try
        {
            var appointment =
                await _appointmentService.GetAppointmentByIdAsync(id);

            return View(appointment);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return RedirectToAction("Index");
        }
    }

    // ==================================
    // PATIENT
    // ==================================

    public async Task<ActionResult> Create()
    {
        if (Session["ReferenceId"] == null)
        {
            return RedirectToAction("Login", "Patient");
        }

        var doctors =
            (await _doctorService.GetAllDoctorsAsync())
            .Where(d => d.IsActive);

        ViewBag.Doctors =
            new SelectList(doctors, "DoctorId", "FullName");

        return View(new CreateAppointmentDto
        {
            ScheduledDate = DateTime.Today
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateAppointmentDto dto)
    {
        if (Session["ReferenceId"] == null)
        {
            return RedirectToAction("Login", "Patient");
        }

        if (!ModelState.IsValid)
        {
            var doctors =
                (await _doctorService.GetAllDoctorsAsync())
                .Where(d => d.IsActive);

            ViewBag.Doctors =
                new SelectList(doctors, "DoctorId", "FullName");

            return View(dto);
        }

        try
        {
            var startTime =
                dto.TimeSlot.Split('-')[0].Trim();

            DateTime appointmentDateTime =
                DateTime.Parse($"{dto.ScheduledDate:yyyy-MM-dd} {startTime}");

            if (appointmentDateTime <= DateTime.Now)
            {
                ModelState.AddModelError(
                    "",
                    "Cannot book appointment in past time.");

                var doctors =
                    (await _doctorService.GetAllDoctorsAsync())
                    .Where(d => d.IsActive);

                ViewBag.Doctors =
                    new SelectList(doctors, "DoctorId", "FullName");

                return View(dto);
            }

            dto.PatientId =
                Convert.ToInt32(Session["ReferenceId"]);

            int appointmentId =
                await _appointmentService.CreateAppointmentAsync(dto);

            return RedirectToAction(
                "Details",
                new
                {
                    id = appointmentId
                });
        }
        catch (Exception ex)
        {
            var doctors =
                (await _doctorService.GetAllDoctorsAsync())
                .Where(d => d.IsActive);

            ViewBag.Doctors =
                new SelectList(doctors, "DoctorId", "FullName");

            ModelState.AddModelError("", ex.Message);

            return View(dto);
        }
    }

    public async Task<ActionResult> MyAppointments()
    {
        try
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction("Login", "Patient");
            }

            int patientId =
                Convert.ToInt32(Session["ReferenceId"]);

            var appointments =
                await _appointmentService.GetAppointmentsByPatientAsync(patientId);

            return View(appointments);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return RedirectToAction("Dashboard", "Patient");
        }
    }

    // ==================================
    // DOCTOR
    // ==================================

    public async Task<ActionResult> TodaySchedule()
    {
        try
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction("Login", "Doctor");
            }

            int doctorId =
                Convert.ToInt32(Session["ReferenceId"]);

            var appointments =
                await _appointmentService.GetTodayScheduleAsync(doctorId);

            var healthRecords =
                await _healthRecordService.GetRecordsByDoctorAsync(doctorId);

            if (appointments != null)
            {
                foreach (var appointment in appointments)
                {
                    appointment.HasHealthRecord =
                        healthRecords != null &&
                        healthRecords.Any(hr =>
                            hr.AppointmentId == appointment.AppointmentId);
                }
            }

            return View(appointments);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return RedirectToAction("Dashboard", "Doctor");
        }
    }

    public async Task<ActionResult> WeeklySchedule()
    {
        try
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction("Login", "Doctor");
            }

            int doctorId =
                Convert.ToInt32(Session["ReferenceId"]);

            var appointments =
                await _appointmentService.GetWeeklyScheduleAsync(doctorId);

            var healthRecords =
                await _healthRecordService.GetRecordsByDoctorAsync(doctorId);

            if (appointments != null)
            {
                foreach (var appointment in appointments)
                {
                    appointment.HasHealthRecord =
                        healthRecords != null &&
                        healthRecords.Any(hr =>
                            hr.AppointmentId == appointment.AppointmentId);
                }
            }

            return View(appointments);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return RedirectToAction("Dashboard", "Doctor");
        }
    }

    // ==================================
    // CONFIRM APPOINTMENT
    // ==================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Confirm(int id, string returnUrl = null)
    {
        try
        {
            var appointment =
                await _appointmentService.GetAppointmentByIdAsync(id);

            if (appointment == null)
            {
                TempData["Error"] = "Appointment not found.";

                return RedirectBackToSchedule(returnUrl);
            }

            var startTime =
                appointment.TimeSlot.Split('-')[0].Trim();

            DateTime appointmentDateTime =
                DateTime.Parse($"{appointment.ScheduledDate:yyyy-MM-dd} {startTime}");

            if (appointmentDateTime <= DateTime.Now)
            {
                TempData["Error"] =
                    "Cannot confirm past appointment.";

                return RedirectBackToSchedule(returnUrl);
            }

            await _appointmentService.ConfirmAppointmentAsync(id);

            TempData["Success"] =
                "Appointment confirmed successfully.";

            return RedirectBackToSchedule(returnUrl);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return RedirectBackToSchedule(returnUrl);
        }
    }

    // ==================================
    // CANCEL APPOINTMENT
    // ==================================

    public async Task<ActionResult> Cancel(int id)
    {
        try
        {
            var appointment =
                await _appointmentService.GetAppointmentByIdAsync(id);

            return View(appointment);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return RedirectBackToSchedule(null);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Cancel(
        int id,
        string reason,
        string returnUrl = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                reason = "Cancelled by doctor";
            }

            await _appointmentService.CancelAppointmentAsync(id, reason);

            TempData["Success"] =
                "Appointment cancelled successfully.";

            return RedirectBackToSchedule(returnUrl);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return RedirectBackToSchedule(returnUrl);
        }
    }

    // ==================================
    // COMPLETE APPOINTMENT
    // ==================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Complete(int id, string returnUrl = null)
    {
        try
        {
            await _appointmentService.CompleteAppointmentAsync(id);

            TempData["Success"] =
                "Appointment completed successfully.";

            return RedirectBackToSchedule(returnUrl);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return RedirectBackToSchedule(returnUrl);
        }
    }

    // ==================================
    // HELPER
    // ==================================

    private ActionResult RedirectBackToSchedule(string returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) &&
            Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        if (Request.UrlReferrer != null)
        {
            string referrerPath =
                Request.UrlReferrer.AbsolutePath;

            if (referrerPath.Contains("WeeklySchedule"))
            {
                return RedirectToAction("WeeklySchedule");
            }

            if (referrerPath.Contains("TodaySchedule"))
            {
                return RedirectToAction("TodaySchedule");
            }
        }

        return RedirectToAction("TodaySchedule");
    }
}
