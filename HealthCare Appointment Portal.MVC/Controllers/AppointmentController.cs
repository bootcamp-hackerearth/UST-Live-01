using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class AppointmentController : Controller
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
        // ALL APPOINTMENTS + SEARCH
        // ==================================

        public async Task<ActionResult>
            Index(string search)
        {
            try
            {
                var appointments =
                    await _appointmentService
                        .GetAllAppointmentsAsync();

                // SEARCH

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search =
                        search.ToLower();

                    appointments =
                        appointments.Where(a =>
                            a.PatientName.ToLower().Contains(search)
                            ||
                            a.DoctorName.ToLower().Contains(search)
                            ||
                            a.AppointmentId.ToString() == search);
                }

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

        // ==================================
        // DETAILS
        // ==================================

        public async Task<ActionResult>
            Details(int id)
        {
            try
            {
                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(id);

                return View(appointment);
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
        // CREATE APPOINTMENT
        // ==================================

        public async Task<ActionResult>
            Create()
        {
            var doctors =
                (await _doctorService
                    .GetAllDoctorsAsync())
                    .Where(d => d.IsActive)
                    .ToList();

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
            Create(CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                var doctors =
                    (await _doctorService
                        .GetAllDoctorsAsync())
                        .Where(d => d.IsActive)
                        .ToList();

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
                        .CreateAppointmentAsync(dto);

                TempData["Success"] =
                    "Appointment created successfully.";

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
                        .Where(d => d.IsActive)
                        .ToList();

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

        // ==================================
        // PATIENT APPOINTMENTS
        // ==================================

        public async Task<ActionResult>
            MyAppointments()
        {
            try
            {
                int patientId = 1;

                var appointments =
                    await _appointmentService
                        .GetAppointmentsByPatientAsync(
                            patientId);

                return View(appointments);
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
        // TODAY SCHEDULE
        // ==================================

        public async Task<ActionResult>
            TodaySchedule()
        {
            try
            {
                int doctorId = 1;

                var appointments =
                    await _appointmentService
                        .GetTodayScheduleAsync(
                            doctorId);

                return View(appointments);
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
        // WEEKLY SCHEDULE
        // ==================================

        public async Task<ActionResult>
            WeeklySchedule()
        {
            try
            {
                int doctorId = 1;

                var appointments =
                    await _appointmentService
                        .GetWeeklyScheduleAsync(
                            doctorId);

                return View(appointments);
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
        // CONFIRM
        // ==================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Confirm(int id)
        {
            try
            {
                await _appointmentService
                    .ConfirmAppointmentAsync(id);

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

        // ==================================
        // CANCEL GET
        // ==================================

        public async Task<ActionResult>
            Cancel(int id)
        {
            try
            {
                var appointment =
                    await _appointmentService
                        .GetAppointmentByIdAsync(id);

                return View(appointment);
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
        // CANCEL POST
        // ==================================

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
                        .GetAppointmentByIdAsync(id);

                return View(appointment);
            }
        }

        // ==================================
        // COMPLETE
        // ==================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Complete(int id)
        {
            try
            {
                await _appointmentService
                    .CompleteAppointmentAsync(id);

                TempData["Success"] =
                    "Appointment completed successfully.";

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
    }
}