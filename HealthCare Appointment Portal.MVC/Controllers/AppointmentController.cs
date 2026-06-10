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

    public AppointmentController(
        IAppointmentApiService appointmentService,
        IDoctorApiService doctorService)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
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
    public async Task<ActionResult> Confirm(int id)
    {
        try
        {
            var appointment =
                await _appointmentService.GetAppointmentByIdAsync(id);

            if (appointment == null)
            {
                TempData["Error"] = "Appointment not found.";
                return RedirectToAction("TodaySchedule");
            }

            var startTime =
                appointment.TimeSlot.Split('-')[0].Trim();

            DateTime appointmentDateTime =
                DateTime.Parse($"{appointment.ScheduledDate:yyyy-MM-dd} {startTime}");

            if (appointmentDateTime <= DateTime.Now)
            {
                TempData["Error"] =
                    "Cannot confirm past appointment.";

                return RedirectToAction("TodaySchedule");
            }

            await _appointmentService.ConfirmAppointmentAsync(id);

            TempData["Success"] =
                "Appointment confirmed successfully.";

            return RedirectToAction("TodaySchedule");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return RedirectToAction("TodaySchedule");
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

            return RedirectToAction("TodaySchedule");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Cancel(int id, string reason)
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

            return RedirectToAction("TodaySchedule");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return RedirectToAction("TodaySchedule");
        }
    }

    // ==================================
    // COMPLETE APPOINTMENT
    // ==================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Complete(int id)
    {
        try
        {
            await _appointmentService.CompleteAppointmentAsync(id);

            TempData["Success"] =
                "Appointment completed successfully.";

            var appointment =
                await _appointmentService.GetAppointmentByIdAsync(id);

            return RedirectToAction(
                "Create",
                "HealthRecord",
                new
                {
                    appointmentId = appointment.AppointmentId,
                    patientId = appointment.PatientId
                });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return RedirectToAction("TodaySchedule");
        }
    }
}
