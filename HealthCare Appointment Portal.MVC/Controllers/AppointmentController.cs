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
    private readonly IPatientApiService _patientService;

    public AppointmentController(
        IAppointmentApiService appointmentService,
        IDoctorApiService doctorService,
        IPatientApiService patientService)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
        _patientService = patientService;
    }

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
            TempData["Error"] =
                ex.Message;

            return View(
                Enumerable.Empty<AppointmentDto>());
        }
    }

    public async Task<ActionResult> Details(
        int? id,
        string returnUrl = null,
        string backText = null)
    {
        if (id == null)
        {
            TempData["Error"] =
                "Appointment Id is missing.";

            return RedirectToAction("MyAppointments");
        }

        try
        {
            var appointment =
                await _appointmentService
                    .GetAppointmentByIdAsync(id.Value);

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl =
                    Url.Action("MyAppointments", "Appointment");
            }

            if (string.IsNullOrWhiteSpace(backText))
            {
                backText =
                    "Back to My Appointments";
            }

            ViewBag.ReturnUrl =
                returnUrl;

            ViewBag.BackText =
                backText;

            return View(appointment);
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;

            return RedirectToAction("MyAppointments");
        }
    }

    public async Task<ActionResult> Create(int? doctorId)
    {
        if (Session["ReferenceId"] == null)
        {
            return RedirectToAction("Create", "Patient");
        }

        int patientId =
            Convert.ToInt32(Session["ReferenceId"]);

        await LoadAppointmentDropdowns(patientId);

        return View(
            new CreateAppointmentDto
            {
                PatientId = patientId,
                DoctorId = doctorId ?? 0,
                ScheduledDate = DateTime.Today
            });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateAppointmentDto dto)
    {
        if (Session["ReferenceId"] == null)
        {
            return RedirectToAction("Create", "Patient");
        }

        int patientId =
            Convert.ToInt32(Session["ReferenceId"]);

        dto.PatientId =
            patientId;

        if (IsPastAppointmentSlot(dto.ScheduledDate, dto.TimeSlot))
        {
            ModelState.AddModelError(
                "TimeSlot",
                "Past time slots are not allowed. Please select a future time slot.");

            await LoadAppointmentDropdowns(patientId);

            return View(dto);
        }

        string duplicateMessage =
            await ValidateDuplicateBooking(
                patientId,
                dto.DoctorId,
                dto.ScheduledDate,
                dto.TimeSlot,
                0);

        if (!string.IsNullOrWhiteSpace(duplicateMessage))
        {
            ModelState.AddModelError(
                "",
                duplicateMessage);

            await LoadAppointmentDropdowns(patientId);

            return View(dto);
        }

        if (!ModelState.IsValid)
        {
            await LoadAppointmentDropdowns(patientId);

            return View(dto);
        }

        try
        {
            int appointmentId =
                await _appointmentService
                    .CreateAppointmentAsync(dto);

            return RedirectToAction(
                "BookingSuccess",
                new
                {
                    id = appointmentId
                });
        }
        catch (Exception ex)
        {
            await LoadAppointmentDropdowns(patientId);

            ModelState.AddModelError(
                "",
                ex.Message);

            return View(dto);
        }
    }

    public async Task<ActionResult> BookingSuccess(int id)
    {
        try
        {
            var appointment =
                await _appointmentService
                    .GetAppointmentByIdAsync(id);

            var patient =
                await _patientService
                    .GetPatientByIdAsync(appointment.PatientId);

            var doctor =
                await _doctorService
                    .GetDoctorByIdAsync(appointment.DoctorId);

            ViewBag.Patient =
                patient;

            ViewBag.Doctor =
                doctor;

            return View(appointment);
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

    public async Task<ActionResult> MyAppointments()
    {
        try
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction(
                    "Create",
                    "Patient");
            }

            int patientId =
                Convert.ToInt32(Session["ReferenceId"]);

            var appointments =
                await _appointmentService
                    .GetAppointmentsByPatientAsync(patientId);

            return View(appointments);
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

    public async Task<ActionResult> Edit(int? id)
    {
        if (id == null)
        {
            TempData["Error"] =
                "Appointment Id is missing.";

            return RedirectToAction("MyAppointments");
        }

        try
        {
            var appointment =
                await _appointmentService
                    .GetAppointmentByIdAsync(id.Value);

            if (appointment.Status.ToString() != "Pending")
            {
                TempData["Error"] =
                    "Only pending appointments can be updated.";

                return RedirectToAction("MyAppointments");
            }

            await LoadAppointmentDropdowns(
                appointment.PatientId);

            var dto =
                new UpdateAppointmentDto
                {
                    DoctorId = appointment.DoctorId,
                    ScheduledDate = appointment.ScheduledDate,
                    TimeSlot = appointment.TimeSlot
                };

            ViewBag.AppointmentId =
                appointment.AppointmentId;

            ViewBag.PatientId =
                appointment.PatientId;

            return View(dto);
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;

            return RedirectToAction("MyAppointments");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(
        int id,
        UpdateAppointmentDto dto)
    {
        try
        {
            var existingAppointment =
                await _appointmentService
                    .GetAppointmentByIdAsync(id);

            if (existingAppointment.Status.ToString() != "Pending")
            {
                TempData["Error"] =
                    "Only pending appointments can be updated.";

                return RedirectToAction("MyAppointments");
            }

            await LoadAppointmentDropdowns(
                existingAppointment.PatientId);

            ViewBag.AppointmentId =
                id;

            ViewBag.PatientId =
                existingAppointment.PatientId;

            if (IsPastAppointmentSlot(dto.ScheduledDate, dto.TimeSlot))
            {
                ModelState.AddModelError(
                    "TimeSlot",
                    "Past time slots are not allowed. Please select a future time slot.");

                return View(dto);
            }

            string duplicateMessage =
                await ValidateDuplicateBooking(
                    existingAppointment.PatientId,
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot,
                    id);

            if (!string.IsNullOrWhiteSpace(duplicateMessage))
            {
                ModelState.AddModelError(
                    "",
                    duplicateMessage);

                return View(dto);
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await _appointmentService
                .UpdateAppointmentAsync(id, dto);

            TempData["AppointmentSuccess"] =
                "Appointment updated successfully.";

            return RedirectToAction("MyAppointments");
        }
        catch (Exception ex)
        {
            var existingAppointment =
                await _appointmentService
                    .GetAppointmentByIdAsync(id);

            await LoadAppointmentDropdowns(
                existingAppointment.PatientId);

            ViewBag.AppointmentId =
                id;

            ViewBag.PatientId =
                existingAppointment.PatientId;

            ModelState.AddModelError(
                "",
                ex.Message);

            return View(dto);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> PatientCancel(int id)
    {
        try
        {
            if (Session["ReferenceId"] == null)
            {
                return RedirectToAction("Create", "Patient");
            }

            int loggedInPatientId =
                Convert.ToInt32(Session["ReferenceId"]);

            var appointment =
                await _appointmentService
                    .GetAppointmentByIdAsync(id);

            if (appointment.PatientId != loggedInPatientId)
            {
                TempData["Error"] =
                    "You can cancel only your own appointment.";

                return RedirectToAction("MyAppointments");
            }

            if (appointment.Status.ToString() != "Pending")
            {
                TempData["Error"] =
                    "Only pending appointments can be cancelled.";

                return RedirectToAction("MyAppointments");
            }

            await _appointmentService
                .CancelAppointmentAsync(
                    id,
                    "Cancelled by patient");

            TempData["AppointmentSuccess"] =
                "Appointment cancelled successfully.";

            return RedirectToAction("MyAppointments");
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;

            return RedirectToAction("MyAppointments");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Confirm(int id)
    {
        try
        {
            await _appointmentService
                .ConfirmAppointmentAsync(id);

            TempData["UpcomingSuccess"] =
                "Appointment confirmed successfully.";

            return RedirectToAction(
                "UpcomingAppointments",
                "Doctor");
        }
        catch (Exception ex)
        {
            TempData["UpcomingError"] =
                ex.Message;

            return RedirectToAction(
                "UpcomingAppointments",
                "Doctor");
        }
    }

    public async Task<ActionResult> Cancel(int? id)
    {
        if (id == null)
        {
            TempData["UpcomingError"] =
                "Appointment Id is missing.";

            return RedirectToAction(
                "UpcomingAppointments",
                "Doctor");
        }

        try
        {
            var appointment =
                await _appointmentService
                    .GetAppointmentByIdAsync(id.Value);

            return View(appointment);
        }
        catch (Exception ex)
        {
            TempData["UpcomingError"] =
                ex.Message;

            return RedirectToAction(
                "UpcomingAppointments",
                "Doctor");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Cancel(
        int? id,
        string reason)
    {
        if (id == null)
        {
            TempData["UpcomingError"] =
                "Appointment Id is missing.";

            return RedirectToAction(
                "UpcomingAppointments",
                "Doctor");
        }

        try
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                reason =
                    "No reason provided";
            }

            await _appointmentService
                .CancelAppointmentAsync(
                    id.Value,
                    reason);

            TempData["UpcomingSuccess"] =
                "Appointment cancelled successfully.";

            return RedirectToAction(
                "UpcomingAppointments",
                "Doctor");
        }
        catch (Exception ex)
        {
            TempData["UpcomingError"] =
                ex.Message;

            return RedirectToAction(
                "UpcomingAppointments",
                "Doctor");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Complete(int id)
    {
        try
        {
            await _appointmentService
                .CompleteAppointmentAsync(id);

            TempData["HealthRecordInfo"] =
                "Appointment completed successfully. Please add health record.";

            return RedirectToAction(
                "Create",
                "HealthRecord",
                new
                {
                    appointmentId = id
                });
        }
        catch (Exception ex)
        {
            TempData["UpcomingError"] =
                ex.Message;

            return RedirectToAction(
                "UpcomingAppointments",
                "Doctor");
        }
    }

    private async Task<string> ValidateDuplicateBooking(
        int patientId,
        int doctorId,
        DateTime scheduledDate,
        string timeSlot,
        int currentAppointmentId)
    {
        var patientAppointments =
            await _appointmentService
                .GetAppointmentsByPatientAsync(patientId);

        bool sameDoctorSameDay =
            patientAppointments.Any(a =>
                a.AppointmentId != currentAppointmentId &&
                a.DoctorId == doctorId &&
                a.ScheduledDate.Date == scheduledDate.Date &&
                a.Status.ToString() != "Cancelled");

        if (sameDoctorSameDay)
        {
            return "You already booked this doctor on the selected date. Please select another doctor or another date.";
        }

        bool sameTimeSlotSameDay =
            patientAppointments.Any(a =>
                a.AppointmentId != currentAppointmentId &&
                a.ScheduledDate.Date == scheduledDate.Date &&
                a.TimeSlot == timeSlot &&
                a.Status.ToString() != "Cancelled");

        if (sameTimeSlotSameDay)
        {
            return "You already have an appointment on the selected date and time slot. Please select another time slot.";
        }

        return null;
    }

    private bool IsPastAppointmentSlot(
        DateTime scheduledDate,
        string timeSlot)
    {
        if (scheduledDate.Date < DateTime.Today)
        {
            return true;
        }

        if (scheduledDate.Date > DateTime.Today)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(timeSlot))
        {
            return false;
        }

        string startTimeText =
            timeSlot.Split('-')[0].Trim();

        TimeSpan startTime;

        if (!TimeSpan.TryParse(startTimeText, out startTime))
        {
            return false;
        }

        DateTime appointmentStartDateTime =
            scheduledDate.Date.Add(startTime);

        return appointmentStartDateTime <= DateTime.Now;
    }

    private async Task LoadAppointmentDropdowns(
        int selectedPatientId = 0)
    {
        var doctors =
            (await _doctorService.GetAllDoctorsAsync())
            .Where(d => d.IsActive)
            .ToList();

        var patients =
            await _patientService.GetAllPatientsAsync();

        var selectedPatient =
            patients.FirstOrDefault(
                p => p.PatientId == selectedPatientId);

        ViewBag.PatientName =
            selectedPatient != null
                ? selectedPatient.FullName
                : "";

        ViewBag.DoctorList =
            doctors;

        ViewBag.PatientList =
            new SelectList(
                patients,
                "PatientId",
                "FullName",
                selectedPatientId);
    }
}