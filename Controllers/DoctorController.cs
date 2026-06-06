using System;
using System.Web.Mvc;
using HealthcareMvcApp.Exceptions;
using HealthcareMvcApp.Filters;
using HealthcareMvcApp.Models;
using HealthcareMvcApp.Services;

namespace HealthcareMvcApp.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        private readonly IHealthRecordService _healthRecordService;

        public DoctorController(
            IDoctorService doctorService,
            IAppointmentService appointmentService,
            IHealthRecordService healthRecordService)
        {
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            _healthRecordService = healthRecordService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(int? doctorId)
        {
            if (!doctorId.HasValue || doctorId.Value <= 0)
            {
                ModelState.AddModelError("", "Valid Doctor ID is required.");
                return View();
            }

            try
            {
                _doctorService.GetDoctorById(doctorId.Value);

                return RedirectToAction(
                    "Dashboard",
                    new { doctorId = doctorId.Value });
            }
            catch (HealthcareAppException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [RequirePositiveIntParameters(
            "doctorId",
            ErrorMessage = "Please login before accessing the doctor dashboard.")]
        public ActionResult Dashboard(int? doctorId)
        {
            try
            {
                Doctor doctor = _doctorService.GetDoctorById(doctorId.Value);

                return View(doctor);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
        }

        [RequirePositiveIntParameters(
            "doctorId",
            ErrorMessage = "Please login before viewing doctor appointments.")]
        public ActionResult UpcomingAppointments(int? doctorId)
        {
            try
            {
                ViewBag.DoctorId = doctorId.Value;
                ViewBag.Doctor = _doctorService.GetDoctorById(doctorId.Value);

                var appointments =
                    _appointmentService.GetUpcomingAppointmentsByDoctor(doctorId.Value);

                return View(appointments);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult ConfirmAppointment(int? appointmentId, int? doctorId)
        {
            if (!appointmentId.HasValue ||
                appointmentId.Value <= 0 ||
                !doctorId.HasValue ||
                doctorId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Appointment ID and Doctor ID are required.";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                _appointmentService.ConfirmAppointment(
                    appointmentId.Value,
                    doctorId.Value);

                TempData["SuccessMessage"] = "Appointment confirmed successfully.";
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(
                "UpcomingAppointments",
                new { doctorId = doctorId.Value });
        }

        [HttpPost]
        public ActionResult CancelAppointment(
            int? appointmentId,
            int? doctorId,
            string reason)
        {
            if (!appointmentId.HasValue ||
                appointmentId.Value <= 0 ||
                !doctorId.HasValue ||
                doctorId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Appointment ID and Doctor ID are required.";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                _appointmentService.CancelAppointmentByDoctor(
                    appointmentId.Value,
                    doctorId.Value,
                    reason);

                TempData["SuccessMessage"] = "Appointment cancelled successfully.";
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(
                "UpcomingAppointments",
                new { doctorId = doctorId.Value });
        }

        [HttpPost]
        public ActionResult CompleteAppointment(int? appointmentId, int? doctorId)
        {
            if (!appointmentId.HasValue ||
                appointmentId.Value <= 0 ||
                !doctorId.HasValue ||
                doctorId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Appointment ID and Doctor ID are required.";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                Appointment appointment =
                    _appointmentService.CompleteAppointment(
                        appointmentId.Value,
                        doctorId.Value);

                TempData["SuccessMessage"] =
                    "Appointment completed. Please add the health record.";

                return RedirectToAction(
                    "AddHealthRecord",
                    new
                    {
                        appointmentId = appointment.AppointmentId,
                        doctorId = doctorId.Value
                    });
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction(
                    "UpcomingAppointments",
                    new { doctorId = doctorId.Value });
            }
        }

        [RequirePositiveIntParameters(
            "appointmentId",
            "doctorId",
            ErrorMessage = "Please select a valid appointment before adding a health record.")]
        public ActionResult AddHealthRecord(int? appointmentId, int? doctorId)
        {
            try
            {
                _doctorService.GetDoctorById(doctorId.Value);

                Appointment appointment =
                    _appointmentService.GetAppointmentById(appointmentId.Value);

                if (appointment.DoctorId != doctorId.Value)
                {
                    TempData["ErrorMessage"] =
                        "This appointment does not belong to the selected doctor.";

                    return RedirectToAction(
                        "UpcomingAppointments",
                        new { doctorId = doctorId.Value });
                }

                HealthRecord record = new HealthRecord
                {
                    AppointmentId = appointment.AppointmentId,
                    DoctorId = doctorId.Value,
                    PatientId = appointment.PatientId,
                    VisitDate = appointment.ScheduledDate
                };

                return View(record);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction(
                    "UpcomingAppointments",
                    new { doctorId = doctorId.Value });
            }
        }

        [HttpPost]
        public ActionResult AddHealthRecord(HealthRecord record)
        {
            if (!ModelState.IsValid)
            {
                return View(record);
            }

            try
            {
                _healthRecordService.AddRecord(
                    record.AppointmentId,
                    record.Diagnosis,
                    record.Prescription,
                    record.Notes);

                TempData["SuccessMessage"] = "Health record added successfully.";

                return RedirectToAction(
                    "HealthRecords",
                    new { doctorId = record.DoctorId });
            }
            catch (HealthcareAppException ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(record);
            }
        }

        [RequirePositiveIntParameters(
            "doctorId",
            ErrorMessage = "Please login before viewing doctor health records.")]
        public ActionResult HealthRecords(int? doctorId)
        {
            try
            {
                ViewBag.DoctorId = doctorId.Value;
                ViewBag.Doctor = _doctorService.GetDoctorById(doctorId.Value);

                var records =
                    _healthRecordService.GetRecordsByDoctor(doctorId.Value);

                return View(records);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
        }

        [RequirePositiveIntParameters(
            "id",
            "doctorId",
            ErrorMessage = "Please select a valid health record to edit.")]
        public ActionResult EditHealthRecord(int? id, int? doctorId)
        {
            try
            {
                HealthRecord record =
                    _healthRecordService.GetRecordForDoctor(
                        id.Value,
                        doctorId.Value);

                ViewBag.DoctorId = doctorId.Value;

                return View(record);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction(
                    "HealthRecords",
                    new { doctorId = doctorId.Value });
            }
        }

        [HttpPost]
        public ActionResult EditHealthRecord(HealthRecord record, int? doctorId)
        {
            if (!doctorId.HasValue || doctorId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Doctor ID is required.";

                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.DoctorId = doctorId.Value;
                return View(record);
            }

            try
            {
                _healthRecordService.UpdateRecordByDoctor(
                    doctorId.Value,
                    record);

                TempData["SuccessMessage"] = "Health record updated successfully.";

                return RedirectToAction(
                    "HealthRecords",
                    new { doctorId = doctorId.Value });
            }
            catch (HealthcareAppException ex)
            {
                ViewBag.DoctorId = doctorId.Value;

                ModelState.AddModelError("", ex.Message);

                return View(record);
            }
        }

        [RequirePositiveIntParameters(
            "doctorId",
            ErrorMessage = "Please login before managing off days.")]
        public ActionResult OffDays(int? doctorId)
        {
            try
            {
                ViewBag.DoctorId = doctorId.Value;
                ViewBag.Doctor = _doctorService.GetDoctorById(doctorId.Value);

                var offDays = _doctorService.GetOffDays(doctorId.Value);

                return View(offDays);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult AddOffDay(int? doctorId, DateTime? offDay)
        {
            if (!doctorId.HasValue || doctorId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Doctor ID is required.";

                return RedirectToAction("Index", "Home");
            }

            if (!offDay.HasValue)
            {
                TempData["ErrorMessage"] = "Off day is required.";

                return RedirectToAction(
                    "OffDays",
                    new { doctorId = doctorId.Value });
            }

            try
            {
                _doctorService.AddOffDay(doctorId.Value, offDay.Value);

                TempData["SuccessMessage"] = "Off day added successfully.";
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(
                "OffDays",
                new { doctorId = doctorId.Value });
        }

        [HttpPost]
        public ActionResult RemoveOffDay(int? doctorId, DateTime? offDay)
        {
            if (!doctorId.HasValue || doctorId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Doctor ID is required.";

                return RedirectToAction("Index", "Home");
            }

            if (!offDay.HasValue)
            {
                TempData["ErrorMessage"] = "Off day is required.";

                return RedirectToAction(
                    "OffDays",
                    new { doctorId = doctorId.Value });
            }

            try
            {
                _doctorService.RemoveOffDay(doctorId.Value, offDay.Value);

                TempData["SuccessMessage"] = "Off day removed successfully.";
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(
                "OffDays",
                new { doctorId = doctorId.Value });
        }

        [HttpPost]
        public ActionResult DeactivateSelf(int? doctorId)
        {
            if (!doctorId.HasValue || doctorId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Doctor ID is required.";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                _doctorService.DeactivateDoctor(doctorId.Value);

                TempData["SuccessMessage"] =
                    "Doctor account deactivated successfully.";

                return RedirectToAction("Index", "Home");
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction(
                    "Dashboard",
                    new { doctorId = doctorId.Value });
            }
        }

        [HttpPost]
        public ActionResult ReactivateSelf(int? doctorId)
        {
            if (!doctorId.HasValue || doctorId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Doctor ID is required.";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                _doctorService.ReactivateDoctor(doctorId.Value);

                TempData["SuccessMessage"] =
                    "Doctor account reactivated successfully.";

                return RedirectToAction(
                    "Dashboard",
                    new { doctorId = doctorId.Value });
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction(
                    "Dashboard",
                    new { doctorId = doctorId.Value });
            }
        }
    }
}