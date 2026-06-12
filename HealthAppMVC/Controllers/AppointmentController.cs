using HealthAppMVC.Constants;
using HealthAppMVC.Services.Interface;
using HealthAppWebAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAppMVC.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IPatientService patientService,
            IDoctorService doctorService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        public async Task<ActionResult> Index(
            string patientName,
            int? doctorId,
            int? patientId,
            string status,
            DateTime? date)
        {
            try
            {
                ViewBag.PatientName = patientName;
                ViewBag.DoctorId = doctorId;
                ViewBag.PatientId = patientId;
                ViewBag.Status = status;
                ViewBag.Date = date.HasValue ? date.Value.ToString("yyyy-MM-dd") : "";

                await LoadDropdowns();

                var appointments = await _appointmentService.GetAllAppointmentsAsync();

                if (!string.IsNullOrWhiteSpace(patientName))
                {
                    appointments = appointments
                        .Where(a =>
                            a.PatientName != null &&
                            a.PatientName.ToLower().Contains(patientName.ToLower()))
                        .ToList();
                }

                if (doctorId.HasValue)
                {
                    appointments = appointments
                        .Where(a => a.DoctorId == doctorId.Value)
                        .ToList();
                }

                if (patientId.HasValue)
                {
                    appointments = appointments
                        .Where(a => a.PatientId == patientId.Value)
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(status))
                {
                    appointments = appointments
                        .Where(a =>
                            a.Status != null &&
                            a.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (date.HasValue)
                {
                    appointments = appointments
                        .Where(a =>
                        {
                            DateTime scheduledDate;

                            return DateTime.TryParse(a.ScheduledDate, out scheduledDate)
                                   && scheduledDate.Date == date.Value.Date;
                        })
                        .ToList();
                }

                return View(appointments);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                await LoadDropdowns();
                return View(new List<AppointmentDto>());
            }
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            await LoadDropdowns();

            return PartialView(
                "_CreateAppointmentModal",
                new CreateAppointmentDto()
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();

                return PartialView(
                    "_CreateAppointmentModal",
                    dto
                );
            }

            try
            {
                await _appointmentService.BookAppointmentAsync(dto);

                return Json(new
                {
                    success = true,
                    message = "Appointment booked successfully."
                });
            }
            catch (Exception ex)
            {
                await LoadDropdowns();

                ModelState.AddModelError("", ex.Message);

                return PartialView(
                    "_CreateAppointmentModal",
                    dto
                );
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(int id)
        {
            try
            {
                await _appointmentService.ConfirmAppointmentAsync(id);
                TempData["Success"] = "Appointment confirmed successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        private async Task LoadDropdowns()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            var doctors = await _doctorService.GetAllDoctorsAsync();

            ViewBag.PatientsList = patients;
            ViewBag.DoctorsList = doctors
                .Where(d => d.IsActive)
                .ToList();

            ViewBag.Patients = new SelectList(
                patients,
                "PatientId",
                "FullName");

            ViewBag.Doctors = new SelectList(
                doctors.Where(d => d.IsActive),
                "DoctorId",
                "FullName");
        }
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
                var healthRecordExists = await _appointmentService.HealthRecordExistsAsync(id);

                ViewBag.HealthRecordExists = healthRecordExists;

                return View(appointment);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAvailableSlots(int doctorId, DateTime scheduledDate)
        {
            try
            {
                var apiAvailableSlots =
                    await _appointmentService.GetAvailableSlotsAsync(
                        doctorId,
                        scheduledDate);

                var availableSlots =
                    HealthAppMVC.Constants.TimeSlots.Slots
                        .Where(slot => apiAvailableSlots.Contains(slot))
                        .ToList();

                return Json(
                    new
                    {
                        success = true,
                        slots = availableSlots
                    },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false,
                        message = ex.Message
                    },
                    JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult Cancel(int id)
        {
            ViewBag.AppointmentId = id;

            return PartialView(
                "_CancelAppointmentModal",
                new CancelAppointmentDto()
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(int id, CancelAppointmentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CancellationReason))
            {
                ViewBag.AppointmentId = id;

                ModelState.AddModelError(
                    "",
                    "Cancellation reason is required.");

                return PartialView(
                    "_CancelAppointmentModal",
                    dto
                );
            }

            try
            {
                await _appointmentService.CancelAppointmentAsync(
                    id,
                    dto.CancellationReason);

                return Json(new
                {
                    success = true,
                    message = "Appointment cancelled successfully."
                });
            }
            catch (Exception ex)
            {
                ViewBag.AppointmentId = id;

                ModelState.AddModelError("", ex.Message);

                return PartialView(
                    "_CancelAppointmentModal",
                    dto
                );
            }
        }

    }
}