using HealthcareWeb.Filters;
using HealthcareWeb.Services;
using SharedClasses.Dtos;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthcareWeb.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorApiService _doctorApiService;
        private readonly IAppointmentApiService _appointmentApiService;
        private readonly IHealthRecordApiService _healthRecordApiService;

        public DoctorController(
            IDoctorApiService doctorApiService,
            IAppointmentApiService appointmentApiService,
            IHealthRecordApiService healthRecordApiService)
        {
            _doctorApiService = doctorApiService;
            _appointmentApiService = appointmentApiService;
            _healthRecordApiService = healthRecordApiService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(int? doctorId)
        {
            if (!doctorId.HasValue || doctorId.Value <= 0)
            {
                ModelState.AddModelError("", "Valid Doctor ID is required.");
                return View();
            }

            try
            {
                await _doctorApiService.GetByIdAsync(doctorId.Value);

                return RedirectToAction(
                    "Dashboard",
                    new { doctorId = doctorId.Value });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [RequirePositiveIntParameters(
            "doctorId",
            ErrorMessage = "Please login before accessing the doctor dashboard.")]
        public async Task<ActionResult> Dashboard(int? doctorId)
        {
            try
            {
                DoctorDto doctor = await _doctorApiService.GetByIdAsync(doctorId.Value);

                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
        }

        [RequirePositiveIntParameters(
             "doctorId",
             ErrorMessage = "Please login before viewing doctor appointments.")]
        public async Task<ActionResult> UpcomingAppointments(int? doctorId, string query)
        {
            try
            {
                ViewBag.DoctorId = doctorId.Value;
                ViewBag.Doctor = await _doctorApiService.GetByIdAsync(doctorId.Value);

                var appointments = string.IsNullOrWhiteSpace(query)
                    ? await _appointmentApiService.GetUpcomingByDoctorAsync(doctorId.Value)
                    : await _appointmentApiService.SearchUpcomingByDoctorAsync(doctorId.Value, query);

                ViewBag.SearchQuery = query;

                return View(appointments);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public async Task<ActionResult> ConfirmAppointment(int? appointmentId, int? doctorId)
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
                var dto = new ConfirmAppointmentDto
                {
                    DoctorId = doctorId.Value
                };

                await _appointmentApiService.ConfirmAsync(appointmentId.Value, dto);

                TempData["SuccessMessage"] = "Appointment confirmed successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(
                "UpcomingAppointments",
                new { doctorId = doctorId.Value });
        }

        [HttpPost]
        public async Task<ActionResult> CancelAppointment(
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
                var dto = new CancelByDoctorDto
                {
                    DoctorId = doctorId.Value,
                    Reason = reason
                };

                await _appointmentApiService.CancelByDoctorAsync(appointmentId.Value, dto);

                TempData["SuccessMessage"] = "Appointment cancelled successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(
                "UpcomingAppointments",
                new { doctorId = doctorId.Value });
        }

        [HttpPost]
        public async Task<ActionResult> CompleteAppointment(int? appointmentId, int? doctorId)
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
                var dto = new CompleteAppointmentDto
                {
                    DoctorId = doctorId.Value
                };

                AppointmentDto appointment =
                    await _appointmentApiService.CompleteAsync(appointmentId.Value, dto);

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
            catch (Exception ex)
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
        public async Task<ActionResult> AddHealthRecord(int? appointmentId, int? doctorId)
        {
            try
            {
                await _doctorApiService.GetByIdAsync(doctorId.Value);

                AppointmentDto appointment =
                    await _appointmentApiService.GetByIdAsync(appointmentId.Value);

                if (appointment.DoctorId != doctorId.Value)
                {
                    TempData["ErrorMessage"] =
                        "This appointment does not belong to the selected doctor.";

                    return RedirectToAction(
                        "UpcomingAppointments",
                        new { doctorId = doctorId.Value });
                }

                ViewBag.DoctorId = doctorId.Value;
                ViewBag.Appointment = appointment;

                var dto = new AddHealthRecordDto
                {
                    AppointmentId = appointment.AppointmentId
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction(
                    "UpcomingAppointments",
                    new { doctorId = doctorId.Value });
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddHealthRecord(
            AddHealthRecordDto dto,
            int? doctorId)
        {
            if (!doctorId.HasValue || doctorId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Doctor ID is required.";

                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.DoctorId = doctorId.Value;
                return View(dto);
            }

            try
            {
                await _healthRecordApiService.AddAsync(dto);

                TempData["SuccessMessage"] = "Health record added successfully.";

                return RedirectToAction(
                    "HealthRecords",
                    new { doctorId = doctorId.Value });
            }
            catch (Exception ex)
            {
                ViewBag.DoctorId = doctorId.Value;

                ModelState.AddModelError("", "Failed to add health record. " + ex.Message);

                return View(dto);
            }
        }
        [RequirePositiveIntParameters(
             "doctorId",
             ErrorMessage = "Please login before viewing doctor health records.")]
        public async Task<ActionResult> HealthRecords(
             int? doctorId,
             string recordQuery,
             string appointmentQuery)
        {
            try
            {
                ViewBag.DoctorId = doctorId.Value;
                ViewBag.Doctor = await _doctorApiService.GetByIdAsync(doctorId.Value);

                var records = string.IsNullOrWhiteSpace(recordQuery)
                    ? await _healthRecordApiService.GetByDoctorAsync(doctorId.Value)
                    : await _healthRecordApiService.SearchByDoctorAsync(
                        doctorId.Value,
                        recordQuery);

                if (string.IsNullOrWhiteSpace(appointmentQuery))
                {
                    ViewBag.CancelledAppointments =
                        await _appointmentApiService.GetCancelledByDoctorAsync(doctorId.Value);
                }
                else
                {
                    ViewBag.CancelledAppointments =
                        await _appointmentApiService.SearchCancelledByDoctorAsync(
                            doctorId.Value,
                            appointmentQuery);
                }

                ViewBag.RecordSearchQuery = recordQuery;
                ViewBag.AppointmentSearchQuery = appointmentQuery;

                return View(records);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Index", "Home");
            }
        }

        [RequirePositiveIntParameters(
            "id",
            "doctorId",
            ErrorMessage = "Please select a valid health record to edit.")]
        public async Task<ActionResult> EditHealthRecord(int? id, int? doctorId)
        {
            try
            {
                HealthRecordDto record =
                    await _healthRecordApiService.GetByIdAsync(id.Value);

                if (record.DoctorId != doctorId.Value)
                {
                    TempData["ErrorMessage"] =
                        "This health record does not belong to the selected doctor.";

                    return RedirectToAction(
                        "HealthRecords",
                        new { doctorId = doctorId.Value });
                }

                var dto = new UpdateHealthRecordDto
                {
                    Diagnosis = record.Diagnosis,
                    Prescription = record.Prescription,
                    Notes = record.Notes
                };

                ViewBag.HealthRecordId = record.HealthRecordId;
                ViewBag.DoctorId = doctorId.Value;

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction(
                    "HealthRecords",
                    new { doctorId = doctorId.Value });
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditHealthRecord(
            int id,
            int? doctorId,
            UpdateHealthRecordDto dto)
        {
            if (!doctorId.HasValue || doctorId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Doctor ID is required.";

                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.HealthRecordId = id;
                ViewBag.DoctorId = doctorId.Value;

                return View(dto);
            }

            try
            {
                await _healthRecordApiService.UpdateAsync(id, dto);

                TempData["SuccessMessage"] = "Health record updated successfully.";

                return RedirectToAction(
                    "HealthRecords",
                    new { doctorId = doctorId.Value });
            }
            catch (Exception ex)
            {
                ViewBag.HealthRecordId = id;
                ViewBag.DoctorId = doctorId.Value;

                ModelState.AddModelError("", ex.Message);

                return View(dto);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeactivateSelf(int? doctorId)
        {
            if (!doctorId.HasValue || doctorId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Doctor ID is required.";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                await _doctorApiService.DeactivateAsync(doctorId.Value);

                TempData["SuccessMessage"] =
                    "Doctor account deactivated successfully.";

                return RedirectToAction(
                    "Dashboard",
                    new { doctorId = doctorId.Value });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction(
                    "Dashboard",
                    new { doctorId = doctorId.Value });
            }
        }

        [HttpPost]
        public async Task<ActionResult> ReactivateSelf(int? doctorId)
        {
            if (!doctorId.HasValue || doctorId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Doctor ID is required.";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                await _doctorApiService.ReactivateAsync(doctorId.Value);

                TempData["SuccessMessage"] =
                    "Doctor account reactivated successfully.";

                return RedirectToAction(
                    "Dashboard",
                    new { doctorId = doctorId.Value });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction(
                    "Dashboard",
                    new { doctorId = doctorId.Value });
            }
        }
        public async Task<ActionResult> HealthRecordDetails(int id, int doctorId)
        {
            try
            {
                HealthRecordDto record = await _healthRecordApiService.GetByIdAsync(id);

                if (record.DoctorId != doctorId)
                {
                    TempData["ErrorMessage"] = "You are not allowed to view this health record.";

                    return RedirectToAction("HealthRecords", new { doctorId = doctorId });
                }

                ViewBag.DoctorId = doctorId;

                return View(record);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("HealthRecords", new { doctorId = doctorId });
            }
        }
    }
}
