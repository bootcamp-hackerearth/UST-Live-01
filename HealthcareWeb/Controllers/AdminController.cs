using HealthcareWeb.Filters;
using HealthcareWeb.Services;
using SharedClasses.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthcareWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPatientApiService _patientApiService;
        private readonly IDoctorApiService _doctorApiService;
        private readonly IAppointmentApiService _appointmentApiService;
        private readonly IHealthRecordApiService _healthRecordApiService;

        public AdminController(
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

        // =========================================================
        // Admin Dashboard
        // =========================================================

        public ActionResult Index()
        {
            return View();
        }

        // =========================================================
        // Patients
        // =========================================================

        public async Task<ActionResult> Patients(string searchText)
        {
            List<PatientDto> patients = await _patientApiService.GetAllAsync();

            ViewBag.SearchText = searchText;
            ViewBag.PatientSuggestions = patients;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.Trim();

                int patientId;
                bool isId = int.TryParse(searchText, out patientId);

                patients = patients
                    .Where(p =>
                        (isId && p.PatientId == patientId) ||
                        (!string.IsNullOrWhiteSpace(p.FullName) &&
                            p.FullName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrWhiteSpace(p.Email) &&
                            p.Email.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrWhiteSpace(p.PhoneNumber) &&
                            p.PhoneNumber.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrWhiteSpace(p.InsuranceId) &&
                            p.InsuranceId.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();
            }

            return View(patients);
        }

        public ActionResult CreatePatient()
        {
            return View(new CreatePatientDto());
        }

        [HttpPost]
        public async Task<ActionResult> CreatePatient(CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _patientApiService.AddAsync(dto);

                TempData["SuccessMessage"] = "Patient created successfully.";
                return RedirectToAction("Patients");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "Patients",
            ErrorMessage = "Please select a valid patient.")]
        public async Task<ActionResult> EditPatient(int? id)
        {
            try
            {
                PatientDto patient = await _patientApiService.GetByIdAsync(id.Value);

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
                return RedirectToAction("Patients");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditPatient(int id, UpdatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PatientId = id;
                return View(dto);
            }

            try
            {
                await _patientApiService.UpdateAsync(id, dto);

                TempData["SuccessMessage"] = "Patient updated successfully.";
                return RedirectToAction("Patients");
            }
            catch (Exception ex)
            {
                ViewBag.PatientId = id;
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "Patients",
            ErrorMessage = "Please select a valid patient to delete.")]
        public async Task<ActionResult> DeletePatient(int? id)
        {
            try
            {
                PatientDto patient = await _patientApiService.GetByIdAsync(id.Value);
                return View(patient);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Patients");
            }
        }

        [HttpPost]
        [ActionName("DeletePatient")]
        public async Task<ActionResult> ConfirmDeletePatient(int? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Patient ID is required.";
                return RedirectToAction("Patients");
            }

            try
            {
                await _patientApiService.DeleteAsync(id.Value);

                TempData["SuccessMessage"] = "Patient deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Patients");
        }

        // =========================================================
        // Doctors
        // =========================================================

        public async Task<ActionResult> Doctors(string searchText)
        {
            List<DoctorDto> doctors = await _doctorApiService.GetAllAsync();

            ViewBag.SearchText = searchText;
            ViewBag.DoctorSuggestions = doctors;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.Trim();

                int doctorId;
                bool isId = int.TryParse(searchText, out doctorId);

                doctors = doctors
                    .Where(d =>
                        (isId && d.DoctorId == doctorId) ||
                        (!string.IsNullOrWhiteSpace(d.FullName) &&
                            d.FullName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        d.Specialisation.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            return View(doctors);
        }

        public ActionResult CreateDoctor()
        {
            return View(new CreateDoctorDto
            {
                PracticeStartDate = DateTime.Today.AddYears(-1)
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateDoctor(CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _doctorApiService.AddAsync(dto);

                TempData["SuccessMessage"] = "Doctor created successfully.";
                return RedirectToAction("Doctors");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "Doctors",
            ErrorMessage = "Please select a valid doctor.")]
        public async Task<ActionResult> EditDoctor(int? id)
        {
            try
            {
                DoctorDto doctor = await _doctorApiService.GetByIdAsync(id.Value);

                var dto = new UpdateDoctorDto
                {
                    FullName = doctor.FullName,
                    Specialisation = doctor.Specialisation,
                    PracticeStartDate = doctor.PracticeStartDate,
                    ConsultationFee = doctor.ConsultationFee,
                    IsActive = doctor.IsActive
                };

                ViewBag.DoctorId = doctor.DoctorId;
                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Doctors");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditDoctor(int id, UpdateDoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DoctorId = id;
                return View(dto);
            }

            try
            {
                await _doctorApiService.UpdateAsync(id, dto);

                TempData["SuccessMessage"] = "Doctor updated successfully.";
                return RedirectToAction("Doctors");
            }
            catch (Exception ex)
            {
                ViewBag.DoctorId = id;
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "Doctors",
            ErrorMessage = "Please select a valid doctor to reactivate.")]
        public async Task<ActionResult> ReactivateDoctor(int? id)
        {
            try
            {
                DoctorDto doctor = await _doctorApiService.GetByIdAsync(id.Value);
                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Doctors");
            }
        }

        [HttpPost]
        [ActionName("ReactivateDoctor")]
        public async Task<ActionResult> ConfirmReactivateDoctor(int? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Doctor ID is required.";
                return RedirectToAction("Doctors");
            }

            try
            {
                await _doctorApiService.ReactivateAsync(id.Value);

                TempData["SuccessMessage"] = "Doctor reactivated successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Doctors");
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "Doctors",
            ErrorMessage = "Please select a valid doctor to deactivate.")]
        public async Task<ActionResult> DeactivateDoctor(int? id)
        {
            try
            {
                DoctorDto doctor = await _doctorApiService.GetByIdAsync(id.Value);
                return View(doctor);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Doctors");
            }
        }

        [HttpPost]
        [ActionName("DeactivateDoctor")]
        public async Task<ActionResult> ConfirmDeactivateDoctor(int? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Doctor ID is required.";
                return RedirectToAction("Doctors");
            }

            try
            {
                await _doctorApiService.DeactivateAsync(id.Value);

                TempData["SuccessMessage"] = "Doctor deactivated successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Doctors");
        }

        // =========================================================
        // Appointments
        // =========================================================

        public async Task<ActionResult> Appointments(string searchText)
        {
            List<AppointmentDto> appointments = await _appointmentApiService.GetAllAsync();
            List<PatientDto> patients = await _patientApiService.GetAllAsync();
            List<DoctorDto> doctors = await _doctorApiService.GetAllAsync();

            ViewBag.SearchText = searchText;
            ViewBag.PatientSuggestions = patients;
            ViewBag.DoctorSuggestions = doctors;
            ViewBag.Patients = patients;
            ViewBag.Doctors = doctors;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.Trim();

                int id;
                bool isId = int.TryParse(searchText, out id);

                List<int> matchingPatientIds = patients
                    .Where(p =>
                        !string.IsNullOrWhiteSpace(p.FullName) &&
                        p.FullName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Select(p => p.PatientId)
                    .ToList();

                List<int> matchingDoctorIds = doctors
                    .Where(d =>
                        !string.IsNullOrWhiteSpace(d.FullName) &&
                        d.FullName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Select(d => d.DoctorId)
                    .ToList();

                appointments = appointments
                    .Where(a =>
                        (isId && a.AppointmentId == id) ||
                        (isId && a.PatientId == id) ||
                        (isId && a.DoctorId == id) ||
                        matchingPatientIds.Contains(a.PatientId) ||
                        matchingDoctorIds.Contains(a.DoctorId))
                    .ToList();
            }

            return View(appointments);
        }

        public async Task<ActionResult> CreateAppointment()
        {
            ViewBag.Patients = await _patientApiService.GetAllAsync();
            ViewBag.Doctors = await _doctorApiService.GetAllActiveAsync();

            return View(new BookAppointmentDto
            {
                ScheduledDate = DateTime.Today.AddDays(1)
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateAppointment(BookAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _patientApiService.GetAllAsync();
                ViewBag.Doctors = await _doctorApiService.GetAllActiveAsync();

                return View(dto);
            }

            try
            {
                await _appointmentApiService.BookAsync(dto);

                TempData["SuccessMessage"] = "Appointment created successfully.";
                return RedirectToAction("Appointments");
            }
            catch (Exception ex)
            {
                ViewBag.Patients = await _patientApiService.GetAllAsync();
                ViewBag.Doctors = await _doctorApiService.GetAllActiveAsync();

                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "Appointments",
            ErrorMessage = "Please select a valid appointment.")]
        public async Task<ActionResult> EditAppointment(int? id)
        {
            try
            {
                AppointmentDto appointment = await _appointmentApiService.GetByIdAsync(id.Value);

                ViewBag.AppointmentId = appointment.AppointmentId;
                ViewBag.Patients = await _patientApiService.GetAllAsync();
                ViewBag.Doctors = await _doctorApiService.GetAllActiveAsync();

                var dto = new UpdateAppointmentDto
                {
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    ScheduledDate = appointment.ScheduledDate,
                    SlotNumber = appointment.SlotNumber
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Appointments");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditAppointment(int id, UpdateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AppointmentId = id;
                ViewBag.Patients = await _patientApiService.GetAllAsync();
                ViewBag.Doctors = await _doctorApiService.GetAllActiveAsync();

                return View(dto);
            }

            try
            {
                await _appointmentApiService.UpdateAsync(id, dto);

                TempData["SuccessMessage"] = "Appointment updated successfully.";
                return RedirectToAction("Appointments");
            }
            catch (Exception ex)
            {
                ViewBag.AppointmentId = id;
                ViewBag.Patients = await _patientApiService.GetAllAsync();
                ViewBag.Doctors = await _doctorApiService.GetAllActiveAsync();

                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "Appointments",
            ErrorMessage = "Please select a valid appointment to delete.")]
        public async Task<ActionResult> DeleteAppointment(int? id)
        {
            try
            {
                AppointmentDto appointment = await _appointmentApiService.GetByIdAsync(id.Value);
                return View(appointment);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Appointments");
            }
        }

        [HttpPost]
        [ActionName("DeleteAppointment")]
        public async Task<ActionResult> ConfirmDeleteAppointment(int? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Appointment ID is required.";
                return RedirectToAction("Appointments");
            }

            try
            {
                await _appointmentApiService.DeleteAsync(id.Value);

                TempData["SuccessMessage"] = "Appointment deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Appointments");
        }

        public async Task<ActionResult> AppointmentsByPatient(int? patientId)
        {
            if (!patientId.HasValue || patientId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Patient ID is required.";
                return RedirectToAction("Appointments");
            }

            try
            {
                List<AppointmentDto> appointments =
                    await _appointmentApiService.GetByPatientAsync(patientId.Value);

                ViewBag.Patient = await _patientApiService.GetByIdAsync(patientId.Value);

                return View("Appointments", appointments);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Appointments");
            }
        }

        public async Task<ActionResult> AppointmentsByDoctor(int? doctorId)
        {
            if (!doctorId.HasValue || doctorId.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Doctor ID is required.";
                return RedirectToAction("Appointments");
            }

            try
            {
                List<AppointmentDto> appointments =
                    await _appointmentApiService.GetByDoctorAsync(doctorId.Value);

                ViewBag.Doctor = await _doctorApiService.GetByIdAsync(doctorId.Value);

                return View("Appointments", appointments);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Appointments");
            }
        }

        // =========================================================
        // Health Records
        // =========================================================

        public async Task<ActionResult> HealthRecords(string searchText)
        {
            List<HealthRecordDto> records = await _healthRecordApiService.GetAllAsync();
            List<PatientDto> patients = await _patientApiService.GetAllAsync();
            List<DoctorDto> doctors = await _doctorApiService.GetAllAsync();

            ViewBag.SearchText = searchText;
            ViewBag.PatientSuggestions = patients;
            ViewBag.DoctorSuggestions = doctors;
            ViewBag.Patients = patients;
            ViewBag.Doctors = doctors;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.Trim();

                int id;
                bool isId = int.TryParse(searchText, out id);

                List<int> matchingPatientIds = patients
                    .Where(p =>
                        !string.IsNullOrWhiteSpace(p.FullName) &&
                        p.FullName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Select(p => p.PatientId)
                    .ToList();

                List<int> matchingDoctorIds = doctors
                    .Where(d =>
                        !string.IsNullOrWhiteSpace(d.FullName) &&
                        d.FullName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Select(d => d.DoctorId)
                    .ToList();

                records = records
                    .Where(r =>
                        (isId && r.HealthRecordId == id) ||
                        (isId && r.AppointmentId == id) ||
                        (isId && r.PatientId == id) ||
                        (isId && r.DoctorId == id) ||
                        matchingPatientIds.Contains(r.PatientId) ||
                        matchingDoctorIds.Contains(r.DoctorId))
                    .ToList();
            }

            return View(records);
        }

        public ActionResult CreateHealthRecord()
        {
            return View(new AddHealthRecordDto());
        }

        [HttpPost]
        public async Task<ActionResult> CreateHealthRecord(AddHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _healthRecordApiService.AddAsync(dto);

                TempData["SuccessMessage"] = "Health record created successfully.";
                return RedirectToAction("HealthRecords");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "HealthRecords",
            ErrorMessage = "Please select a valid health record.")]
        public async Task<ActionResult> EditHealthRecord(int? id)
        {
            try
            {
                HealthRecordDto record = await _healthRecordApiService.GetByIdAsync(id.Value);

                var dto = new UpdateHealthRecordDto
                {
                    Diagnosis = record.Diagnosis,
                    Prescription = record.Prescription,
                    Notes = record.Notes
                };

                ViewBag.HealthRecordId = record.HealthRecordId;
                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("HealthRecords");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditHealthRecord(int id, UpdateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.HealthRecordId = id;
                return View(dto);
            }

            try
            {
                await _healthRecordApiService.UpdateAsync(id, dto);

                TempData["SuccessMessage"] = "Health record updated successfully.";
                return RedirectToAction("HealthRecords");
            }
            catch (Exception ex)
            {
                ViewBag.HealthRecordId = id;
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "HealthRecords",
            ErrorMessage = "Please select a valid health record to delete.")]
        public async Task<ActionResult> DeleteHealthRecord(int? id)
        {
            try
            {
                HealthRecordDto record = await _healthRecordApiService.GetByIdAsync(id.Value);
                return View(record);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("HealthRecords");
            }
        }

        [HttpPost]
        [ActionName("DeleteHealthRecord")]
        public async Task<ActionResult> ConfirmDeleteHealthRecord(int? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Health Record ID is required.";
                return RedirectToAction("HealthRecords");
            }

            try
            {
                await _healthRecordApiService.DeleteAsync(id.Value);

                TempData["SuccessMessage"] = "Health record deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("HealthRecords");
        }
    }
}