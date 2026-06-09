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

    private readonly IPatientApiService
    _patientService;

    public AppointmentController(
        IAppointmentApiService appointmentService,
        IDoctorApiService doctorService,
        IPatientApiService patientService)
    {
        _appointmentService =
            appointmentService;

        _doctorService =
            doctorService;

        _patientService =
            patientService;
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
        var doctors =
            (await _doctorService
                .GetAllDoctorsAsync())
            .Where(d => d.IsActive);

        var patients =
            await _patientService
                .GetAllPatientsAsync();

        ViewBag.Doctors =
            new SelectList(
                doctors,
                "DoctorId",
                "FullName");

        ViewBag.Patients =
            new SelectList(
                patients,
                "PatientId",
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
        if (!ModelState.IsValid)
        {
            var doctors =
                (await _doctorService
                    .GetAllDoctorsAsync())
                .Where(d => d.IsActive);

            var patients =
                await _patientService
                    .GetAllPatientsAsync();

            ViewBag.Doctors =
                new SelectList(
                    doctors,
                    "DoctorId",
                    "FullName");

            ViewBag.Patients =
                new SelectList(
                    patients,
                    "PatientId",
                    "FullName");

            return View(dto);
        }

        try
        {
            int appointmentId =
                await _appointmentService
                    .CreateAppointmentAsync(
                        dto);

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
                (await _doctorService
                    .GetAllDoctorsAsync())
                .Where(d => d.IsActive);

            var patients =
                await _patientService
                    .GetAllPatientsAsync();

            ViewBag.Doctors =
                new SelectList(
                    doctors,
                    "DoctorId",
                    "FullName");

            ViewBag.Patients =
                new SelectList(
                    patients,
                    "PatientId",
                    "FullName");

            ModelState.AddModelError(
                "",
                ex.Message);

            return View(dto);
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
                "Index");
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

            TempData["Success"] =
                "Appointment completed successfully.";

            return RedirectToAction(
                "Index");
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
