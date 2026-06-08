using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

public class AppointmentController
: Controller
{
    private readonly IAppointmentApiService
    _appointmentService;

    private readonly IDoctorApiService
    _doctorService;

    public AppointmentController(
        IAppointmentApiService appointmentService,
        IDoctorApiService doctorService)
    {
        _appointmentService =
            appointmentService;

        _doctorService =
            doctorService;
    }

    // ==================================
    // ADMIN
    // ==================================

    public async Task<ActionResult>
        Index()
    {
        try
        {
            var appointments =
                await _appointmentService
                    .GetAllAppointmentsAsync();

            return View(
                appointments);
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;

            return View(
                Enumerable.Empty<AppointmentDto>());
        }
    }

    // ==================================
    // COMMON
    // ==================================

    public async Task<ActionResult>
        Details(
            int id)
    {
        try
        {
            var appointment =
                await _appointmentService
                    .GetAppointmentByIdAsync(
                        id);

            return View(
                appointment);
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;

            return RedirectToAction(
                "Index");
        }
    }

    // ==================================
    // PATIENT
    // ==================================

    public async Task<ActionResult>
        Create()
    {
        if (Session["ReferenceId"] == null)
        {
            return RedirectToAction(
                "Login",
                "User");
        }

        var doctors =
            (await _doctorService
                .GetAllDoctorsAsync())
            .Where(d => d.IsActive);

        ViewBag.Doctors =
            new SelectList(
                doctors,
                "DoctorId",
                "FullName");

        return View(
            new CreateAppointmentDto
            {
                ScheduledDate =
                    DateTime.Today
            });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult>
        Create(
            CreateAppointmentDto dto)
    {
        if (Session["ReferenceId"] == null)
        {
            return RedirectToAction(
                "Login",
                "User");
        }

        if (!ModelState.IsValid)
        {
            var doctors =
                (await _doctorService
                    .GetAllDoctorsAsync())
                .Where(d => d.IsActive);

            ViewBag.Doctors =
                new SelectList(
                    doctors,
                    "DoctorId",
                    "FullName");

            return View(
                dto);
        }

        try
        {
            dto.PatientId =
                Convert.ToInt32(
                    Session["ReferenceId"]);

            int appointmentId =
                await _appointmentService
                    .CreateAppointmentAsync(
                        dto);

            TempData["Success"] =
                "Appointment created successfully.";

            return RedirectToAction(
                "Details",
                new
                {
                    id =
                        appointmentId
                });
        }
        catch (Exception ex)
        {
            var doctors =
                (await _doctorService
                    .GetAllDoctorsAsync())
                .Where(d => d.IsActive);

            ViewBag.Doctors =
                new SelectList(
                    doctors,
                    "DoctorId",
                    "FullName");

            ModelState.AddModelError(
                "",
                ex.Message);

            return View(
                dto);
        }
    }

    public async Task<ActionResult>
        MyAppointments()
    {
        try
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            int patientId =
                Convert.ToInt32(
                    Session["ReferenceId"]);

            var appointments =
                await _appointmentService
                    .GetAppointmentsByPatientAsync(
                        patientId);

            return View(
                appointments);
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;

            return RedirectToAction(
                "Dashboard",
                "Patient");
        }
    }

    // ==================================
    // DOCTOR
    // ==================================

    public async Task<ActionResult>
        TodaySchedule()
    {
        try
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            int doctorId =
                Convert.ToInt32(
                    Session["ReferenceId"]);

            var appointments =
                await _appointmentService
                    .GetTodayScheduleAsync(
                        doctorId);

            return View(
                appointments);
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;

            return RedirectToAction(
                "Dashboard",
                "Doctor");
        }
    }

    public async Task<ActionResult>
        WeeklySchedule()
    {
        try
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction(
                    "Login",
                    "User");
            }

            int doctorId =
                Convert.ToInt32(
                    Session["ReferenceId"]);

            var appointments =
                await _appointmentService
                    .GetWeeklyScheduleAsync(
                        doctorId);

            return View(
                appointments);
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;

            return RedirectToAction(
                "Dashboard",
                "Doctor");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult>
        Confirm(
            int id)
    {
        try
        {
            await _appointmentService
                .ConfirmAppointmentAsync(
                    id);

            TempData["Success"] =
                "Appointment confirmed successfully.";

            return RedirectToAction(
                "Details",
                new { id });
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;

            return RedirectToAction(
                "Details",
                new { id });
        }
    }

    public async Task<ActionResult>
        Cancel(
            int id)
    {
        try
        {
            var appointment =
                await _appointmentService
                    .GetAppointmentByIdAsync(
                        id);

            return View(
                appointment);
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;

            return RedirectToAction(
                "Details",
                new { id });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult>
        Cancel(
            int id,
            string reason)
    {
        try
        {
            await _appointmentService
                .CancelAppointmentAsync(
                    id,
                    reason);

            TempData["Success"] =
                "Appointment cancelled successfully.";

            return RedirectToAction(
                "Details",
                new { id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(
                "",
                ex.Message);

            var appointment =
                await _appointmentService
                    .GetAppointmentByIdAsync(
                        id);

            return View(
                appointment);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult>
        Complete(
            int id)
    {
        try
        {
            await _appointmentService
                .CompleteAppointmentAsync(
                    id);

            var appointment =
                await _appointmentService
                    .GetAppointmentByIdAsync(
                        id);

            TempData["Success"] =
                "Appointment completed successfully.";

            return RedirectToAction(
                "Create",
                "HealthRecord",
                new
                {
                    appointmentId =
                        appointment.AppointmentId,

                    patientId =
                        appointment.PatientId
                });
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;

            return RedirectToAction(
                "Details",
                new { id });
        }
    }
}
