using System;
using System.Web.Mvc;
using HealthcareMvcApp.Enums;
using HealthcareMvcApp.Exceptions;
using HealthcareMvcApp.Filters;
using HealthcareMvcApp.Models;
using HealthcareMvcApp.Services;

namespace HealthcareMvcApp.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IHealthRecordService _healthRecordService;
        private readonly IDoctorService _doctorService;

        public PatientController(
            IPatientService patientService,
            IAppointmentService appointmentService,
            IHealthRecordService healthRecordService,
            IDoctorService doctorService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _healthRecordService = healthRecordService;
            _doctorService = doctorService;
        }

        public ActionResult Register()
        {
            Patient patient = new Patient
            {
                Gender = Gender.Other
            };

            return View(patient);
        }

        [HttpPost]
        public ActionResult Register(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                Patient createdPatient = _patientService.RegisterPatient(patient);

                TempData["SuccessMessage"] = "Patient registered successfully.";

                return RedirectToAction(
                    "Dashboard",
                    new { patientId = createdPatient.PatientId });
            }
            catch (HealthcareAppException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(patient);
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(int? patientId)
        {
            if (!patientId.HasValue || patientId.Value <= 0)
            {
                ModelState.AddModelError("", "Valid Patient ID is required.");
                return View();
            }

            try
            {
                _patientService.GetPatientById(patientId.Value);

                return RedirectToAction(
                    "Dashboard",
                    new { patientId = patientId.Value });
            }
            catch (HealthcareAppException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [RequirePositiveIntParameters(
            "patientId",
            ErrorMessage = "Please login before accessing the patient dashboard.")]
        public ActionResult Dashboard(int? patientId)
        {
            try
            {
                Patient patient = _patientService.GetPatientById(patientId.Value);

                return View(patient);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
        }

        [RequirePositiveIntParameters(
            "patientId",
            ErrorMessage = "Please login before editing your profile.")]
        public ActionResult EditProfile(int? patientId)
        {
            try
            {
                Patient patient = _patientService.GetPatientById(patientId.Value);

                return View(patient);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EditProfile(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                Patient updatedPatient = _patientService.UpdatePatient(patient);

                TempData["SuccessMessage"] = "Profile updated successfully.";

                return RedirectToAction(
                    "Dashboard",
                    new { patientId = updatedPatient.PatientId });
            }
            catch (HealthcareAppException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(patient);
            }
        }

        [RequirePositiveIntParameters(
            "patientId",
            ErrorMessage = "Please login before viewing appointments.")]
        public ActionResult UpcomingAppointments(int? patientId)
        {
            try
            {
                ViewBag.PatientId = patientId.Value;
                ViewBag.Patient = _patientService.GetPatientById(patientId.Value);

                var appointments =
                    _appointmentService.GetUpcomingAppointmentsByPatient(patientId.Value);

                return View(appointments);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
        }

        [RequirePositiveIntParameters(
            "appointmentId",
            "patientId",
            ErrorMessage = "Please select a valid appointment to cancel.")]
        public ActionResult CancelAppointment(int? appointmentId, int? patientId)
        {
            try
            {
                _patientService.GetPatientById(patientId.Value);

                Appointment appointment =
                    _appointmentService.GetAppointmentById(appointmentId.Value);

                if (appointment.PatientId != patientId.Value)
                {
                    TempData["ErrorMessage"] =
                        "This appointment does not belong to the selected patient.";

                    return RedirectToAction(
                        "UpcomingAppointments",
                        new { patientId = patientId.Value });
                }

                ViewBag.PatientId = patientId.Value;

                return View(appointment);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction(
                    "UpcomingAppointments",
                    new { patientId = patientId.Value });
            }
        }

        [HttpPost]
        [ActionName("CancelAppointment")]
        public ActionResult ConfirmCancelAppointment(
            int? appointmentId,
            int? patientId,
            string reason)
        {
            if (!appointmentId.HasValue ||
                appointmentId.Value <= 0 ||
                !patientId.HasValue ||
                patientId.Value <= 0)
            {
                TempData["ErrorMessage"] =
                    "Please select a valid appointment to cancel.";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                _appointmentService.CancelAppointmentByPatient(
                    appointmentId.Value,
                    patientId.Value,
                    reason);

                TempData["SuccessMessage"] = "Appointment cancelled successfully.";
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(
                "UpcomingAppointments",
                new { patientId = patientId.Value });
        }

        [RequirePositiveIntParameters(
            "patientId",
            ErrorMessage = "Please login before booking an appointment.")]
        public ActionResult BookAppointment(
            int? patientId,
            Specialisation? specialisation,
            DateTime? scheduledDate,
            int? slotNumber)
        {
            try
            {
                _patientService.GetPatientById(patientId.Value);

                Appointment appointment = new Appointment
                {
                    PatientId = patientId.Value,
                    ScheduledDate = scheduledDate.HasValue
                        ? scheduledDate.Value.Date
                        : DateTime.Today.AddDays(1),
                    SlotNumber = slotNumber.HasValue
                        ? slotNumber.Value
                        : 1
                };

                ViewBag.PatientId = patientId.Value;
                ViewBag.SelectedSpecialisation = specialisation;
                ViewBag.Doctors = null;

                if (specialisation.HasValue)
                {
                    ViewBag.Doctors =
                        _doctorService.GetAvailableDoctorsBySpecialisation(
                            specialisation.Value,
                            appointment.ScheduledDate,
                            appointment.SlotNumber);
                }

                return View(appointment);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction(
                    "Dashboard",
                    new { patientId = patientId.Value });
            }
        }

        [HttpPost]
        public ActionResult BookAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PatientId = appointment.PatientId;
                ViewBag.Doctors = null;

                return View(appointment);
            }

            try
            {
                _appointmentService.BookAppointment(
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.ScheduledDate,
                    appointment.SlotNumber);

                TempData["SuccessMessage"] = "Appointment booked successfully.";

                return RedirectToAction(
                    "UpcomingAppointments",
                    new { patientId = appointment.PatientId });
            }
            catch (HealthcareAppException ex)
            {
                ViewBag.PatientId = appointment.PatientId;
                ViewBag.Doctors = null;

                ModelState.AddModelError("", ex.Message);

                return View(appointment);
            }
        }

        [RequirePositiveIntParameters(
            "patientId",
            ErrorMessage = "Please login before viewing health records.")]
        public ActionResult HealthRecords(int? patientId)
        {
            try
            {
                ViewBag.PatientId = patientId.Value;
                ViewBag.Patient = _patientService.GetPatientById(patientId.Value);

                ViewBag.CancelledAppointments =
                    _appointmentService.GetCancelledAppointmentsByPatient(patientId.Value);

                var records =
                    _healthRecordService.GetRecordsByPatient(patientId.Value);

                return View(records);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
        }

        [RequirePositiveIntParameters(
            "patientId",
            ErrorMessage = "Please login before searching doctors.")]
        public ActionResult SearchDoctors(int? patientId)
        {
            ViewBag.PatientId = patientId.Value;

            return View();
        }

        [HttpPost]
        public ActionResult SearchDoctors(int? patientId, Specialisation specialisation)
        {
            if (!patientId.HasValue || patientId.Value <= 0)
            {
                TempData["ErrorMessage"] =
                    "Please login before searching doctors.";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                ViewBag.PatientId = patientId.Value;

                var doctors =
                    _doctorService.SearchDoctorsBySpecialisation(specialisation);

                return View(doctors);
            }
            catch (HealthcareAppException ex)
            {
                ViewBag.PatientId = patientId.Value;

                ModelState.AddModelError("", ex.Message);

                return View();
            }
        }
    }
}