using HealthcareWeb.Filters;
using HealthcareWeb.Services;
using SharedClasses.Dtos;
using SharedClasses.Enums;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthcareWeb.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientApiService _patientApiService;
        private readonly IDoctorApiService _doctorApiService;
        private readonly IAppointmentApiService _appointmentApiService;
        private readonly IHealthRecordApiService _healthRecordApiService;

        public PatientController(
            IPatientApiService patientApiService,
            IDoctorApiService doctorApiService,
            IAppointmentApiService appointmentApiService,
            IHealthRecordApiService healthRecordApiService)
        {
            _patientApiService = patientApiService;
            _doctorApiService = doctorApiService;
            _appointmentApiService = appointmentApiService;
            _healthRecordApiService = healthRecordApiService;
        }

        public ActionResult Register()
        {
            return View(new CreatePatientDto
            {
                Gender = Gender.Other
            });
        }

        [HttpPost]
        public async Task<ActionResult> Register(CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                PatientDto createdPatient = await _patientApiService.AddAsync(dto);

                TempData["SuccessMessage"] = "Patient registered successfully.";
                return RedirectToAction("Dashboard", new { patientId = createdPatient.PatientId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Failed to register patient. " + ex.Message);
                return View(dto);
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(int? patientId)
        {
            if (!patientId.HasValue || patientId.Value <= 0)
            {
                ModelState.AddModelError("", "Valid Patient ID is required.");
                return View();
            }

            try
            {
                await _patientApiService.GetByIdAsync(patientId.Value);

                return RedirectToAction("Dashboard", new { patientId = patientId.Value });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [RequirePositiveIntParameters(
            "patientId",
            ErrorMessage = "Please login before accessing the patient dashboard.")]
        public async Task<ActionResult> Dashboard(int? patientId)
        {
            try
            {
                PatientDto patient = await _patientApiService.GetByIdAsync(patientId.Value);
                return View(patient);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
        [RequirePositiveIntParameters(
                "patientId",
                ErrorMessage = "Please login before editing your profile.")]
        public async Task<ActionResult> EditProfile(int? patientId)
        {
            try
            {
                PatientDto patient = await _patientApiService.GetByIdAsync(patientId.Value);

                var dto = new UpdatePatientDto
                {
                    FullName = patient.FullName,
                    DateOfBirth = patient.DateOfBirth,
                    Gender = patient.Gender,
                    PhoneNumber = patient.PhoneNumber,
                    Email = patient.Email,
                    InsuranceId = patient.InsuranceId
                };

                ViewBag.PatientId = patient.PatientId;
                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditProfile(int id, UpdatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PatientId = id;
                return View(dto);
            }

            try
            {
                await _patientApiService.UpdateAsync(id, dto);

                TempData["SuccessMessage"] = "Profile updated successfully.";
                return RedirectToAction("Dashboard", new { patientId = id });
            }
            catch (Exception ex)
            {
                ViewBag.PatientId = id;
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        [RequirePositiveIntParameters(
            "patientId",
            ErrorMessage = "Please login before viewing appointments.")]
        public async Task<ActionResult> UpcomingAppointments(int? patientId)
        {
            try
            {
                ViewBag.PatientId = patientId.Value;
                ViewBag.Patient = await _patientApiService.GetByIdAsync(patientId.Value);

                var appointments =
                    await _appointmentApiService.GetUpcomingByPatientAsync(patientId.Value);

                return View(appointments);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [RequirePositiveIntParameters(
            "appointmentId",
            "patientId",
            ErrorMessage = "Please select a valid appointment to cancel.")]
        public async Task<ActionResult> CancelAppointment(int? appointmentId, int? patientId)
        {
            try
            {
                await _patientApiService.GetByIdAsync(patientId.Value);

                AppointmentDto appointment =
                    await _appointmentApiService.GetByIdAsync(appointmentId.Value);

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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction(
                    "UpcomingAppointments",
                    new { patientId = patientId.Value });
            }
        }

        [HttpPost]
        [ActionName("CancelAppointment")]
        public async Task<ActionResult> ConfirmCancelAppointment(
            int? appointmentId,
            int? patientId,
            string reason)
        {
            if (!appointmentId.HasValue ||
                appointmentId.Value <= 0 ||
                !patientId.HasValue ||
                patientId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Please select a valid appointment to cancel.";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var dto = new CancelByPatientDto
                {
                    PatientId = patientId.Value,
                    Reason = reason
                };

                await _appointmentApiService.CancelByPatientAsync(appointmentId.Value, dto);

                TempData["SuccessMessage"] = "Appointment cancelled successfully.";
            }
            catch (Exception ex)
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
        public async Task<ActionResult> BookAppointment(
             int? patientId,
             Specialisation? specialisation,
             DateTime? scheduledDate,
             string doctorName)
        {
            try
            {
                await _patientApiService.GetByIdAsync(patientId.Value);

                var dto = new BookAppointmentDto
                {
                    PatientId = patientId.Value,
                    ScheduledDate = scheduledDate.HasValue
                        ? scheduledDate.Value.Date
                        : DateTime.Today.AddDays(1)
                };

                ViewBag.PatientId = patientId.Value;
                ViewBag.SelectedSpecialisation = specialisation;
                ViewBag.DoctorNameQuery = doctorName;
                ViewBag.Doctors = await _doctorApiService.SearchActiveAsync(
                    doctorName,
                    specialisation); 

                if (specialisation.HasValue || !string.IsNullOrWhiteSpace(doctorName))
                {
                    ViewBag.Doctors = await _doctorApiService.SearchActiveAsync(
                        doctorName,
                        specialisation);
                }

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction(
                    "Dashboard",
                    new { patientId = patientId.Value });
            }
        }

        [HttpPost]
        public async Task<ActionResult> BookAppointment(BookAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PatientId = dto.PatientId;
                ViewBag.SelectedSpecialisation = null;
                ViewBag.Doctors = null;

                return View(dto);
            }

            try
            {
                await _appointmentApiService.BookAsync(dto);

                TempData["SuccessMessage"] = "Appointment booked successfully.";

                return RedirectToAction(
                    "UpcomingAppointments",
                    new { patientId = dto.PatientId });
            }
            catch (Exception ex)
            {
                ViewBag.PatientId = dto.PatientId;
                ViewBag.SelectedSpecialisation = null;
                ViewBag.Doctors = null;

                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        [RequirePositiveIntParameters(
            "patientId",
            ErrorMessage = "Please login before viewing health records.")]
        public async Task<ActionResult> HealthRecords(
            int? patientId,
            string recordQuery,
            string appointmentQuery)
        {
            try
            {
                ViewBag.PatientId = patientId.Value;
                ViewBag.Patient = await _patientApiService.GetByIdAsync(patientId.Value);

                if (string.IsNullOrWhiteSpace(appointmentQuery))
                {
                    ViewBag.CancelledAppointments =
                        await _appointmentApiService.GetCancelledByPatientAsync(patientId.Value);
                }
                else
                {
                    ViewBag.CancelledAppointments =
                        await _appointmentApiService.SearchCancelledByPatientAsync(
                            patientId.Value,
                            appointmentQuery);
                }

                var records = string.IsNullOrWhiteSpace(recordQuery)
                    ? await _healthRecordApiService.GetByPatientAsync(patientId.Value)
                    : await _healthRecordApiService.SearchByPatientAsync(
                        patientId.Value,
                        recordQuery);

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
     
        public async Task<ActionResult> HealthRecordDetails(int id, int patientId)
        {
            try
            {
                HealthRecordDto record = await _healthRecordApiService.GetByIdAsync(id);

                if (record.PatientId != patientId)
                {
                    TempData["ErrorMessage"] = "You are not allowed to view this health record.";

                    return RedirectToAction("HealthRecords", new { patientId = patientId });
                }

                ViewBag.PatientId = patientId;

                return View(record);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("HealthRecords", new { patientId = patientId });
            }
        }
    }
}
