using System;
using System.Web.Mvc;
using HealthcareMvcApp.Exceptions;
using HealthcareMvcApp.Filters;
using HealthcareMvcApp.Models;
using HealthcareMvcApp.Services;

namespace HealthcareMvcApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        private readonly IHealthRecordService _healthRecordService;

        public AdminController(
            IPatientService patientService,
            IDoctorService doctorService,
            IAppointmentService appointmentService,
            IHealthRecordService healthRecordService)
        {
            _patientService = patientService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            _healthRecordService = healthRecordService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Patients()
        {
            var patients = _patientService.GetAllPatients();

            return View(patients);
        }

        public ActionResult CreatePatient()
        {
            return View(new Patient());
        }

        [HttpPost]
        public ActionResult CreatePatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                _patientService.RegisterPatient(patient);

                TempData["SuccessMessage"] = "Patient created successfully.";

                return RedirectToAction("Patients");
            }
            catch (HealthcareAppException ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(patient);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "An unexpected error occurred while creating the patient.");

                return View(patient);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "Patients",
            ErrorMessage = "Please select a valid patient.")]
        public ActionResult EditPatient(int? id)
        {
            try
            {
                Patient patient = _patientService.GetPatientById(id.Value);

                return View(patient);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Patients");
            }
        }

        [HttpPost]
        public ActionResult EditPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                _patientService.UpdatePatient(patient);

                TempData["SuccessMessage"] = "Patient updated successfully.";

                return RedirectToAction("Patients");
            }
            catch (HealthcareAppException ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(patient);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "An unexpected error occurred while updating the patient.");

                return View(patient);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "Patients",
            ErrorMessage = "Please select a valid patient to delete.")]
        public ActionResult DeletePatient(int? id)
        {
            try
            {
                Patient patient = _patientService.GetPatientById(id.Value);

                return View(patient);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Patients");
            }
        }

        [HttpPost]
        [ActionName("DeletePatient")]
        public ActionResult ConfirmDeletePatient(int? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Patient ID is required.";

                return RedirectToAction("Patients");
            }

            try
            {
                _patientService.DeletePatient(id.Value);

                TempData["SuccessMessage"] = "Patient deleted successfully.";
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] =
                    "An unexpected error occurred while deleting the patient.";
            }

            return RedirectToAction("Patients");
        }

        public ActionResult Doctors()
        {
            var doctors = _doctorService.GetAllDoctors();

            return View(doctors);
        }

        public ActionResult CreateDoctor()
        {
            Doctor doctor = new Doctor
            {
                PracticeStartDate = DateTime.Today.AddYears(-1),
                IsActive = true
            };

            return View(doctor);
        }

        [HttpPost]
        public ActionResult CreateDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return View(doctor);
            }

            try
            {
                _doctorService.AddDoctor(doctor);

                TempData["SuccessMessage"] = "Doctor created successfully.";

                return RedirectToAction("Doctors");
            }
            catch (HealthcareAppException ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(doctor);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "An unexpected error occurred while creating the doctor.");

                return View(doctor);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "Doctors",
            ErrorMessage = "Please select a valid doctor.")]
        public ActionResult EditDoctor(int? id)
        {
            try
            {
                Doctor doctor = _doctorService.GetDoctorById(id.Value);

                return View(doctor);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Doctors");
            }
        }

        [HttpPost]
        public ActionResult EditDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return View(doctor);
            }

            try
            {
                _doctorService.UpdateDoctor(doctor);

                TempData["SuccessMessage"] = "Doctor updated successfully.";

                return RedirectToAction("Doctors");
            }
            catch (HealthcareAppException ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(doctor);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "An unexpected error occurred while updating the doctor.");

                return View(doctor);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "Doctors",
            ErrorMessage = "Please select a valid doctor to reactivate.")]
        public ActionResult ReactivateDoctor(int? id)
        {
            try
            {
                Doctor doctor = _doctorService.GetDoctorById(id.Value);

                return View(doctor);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Doctors");
            }
        }

        [HttpPost]
        [ActionName("ReactivateDoctor")]
        public ActionResult ConfirmReactivateDoctor(int? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Doctor ID is required.";

                return RedirectToAction("Doctors");
            }

            try
            {
                _doctorService.ReactivateDoctor(id.Value);

                TempData["SuccessMessage"] = "Doctor reactivated successfully.";
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] =
                    "An unexpected error occurred while reactivating the doctor.";
            }

            return RedirectToAction("Doctors");
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "Doctors",
            ErrorMessage = "Please select a valid doctor to deactivate.")]
        public ActionResult DeactivateDoctor(int? id)
        {
            try
            {
                Doctor doctor = _doctorService.GetDoctorById(id.Value);

                return View(doctor);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Doctors");
            }
        }

        [HttpPost]
        [ActionName("DeactivateDoctor")]
        public ActionResult ConfirmDeactivateDoctor(int? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Doctor ID is required.";

                return RedirectToAction("Doctors");
            }

            try
            {
                _doctorService.DeactivateDoctor(id.Value);

                TempData["SuccessMessage"] = "Doctor deactivated successfully.";
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] =
                    "An unexpected error occurred while deactivating the doctor.";
            }

            return RedirectToAction("Doctors");
        }

        public ActionResult Appointments()
        {
            var appointments = _appointmentService.GetAllAppointments();

            return View(appointments);
        }

        public ActionResult CreateAppointment()
        {
            ViewBag.Patients = _patientService.GetAllPatients();
            ViewBag.Doctors = _doctorService.GetAllActiveDoctors();

            Appointment appointment = new Appointment
            {
                ScheduledDate = DateTime.Today.AddDays(1),
                SlotNumber = 1
            };

            return View(appointment);
        }

        [HttpPost]
        public ActionResult CreateAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = _patientService.GetAllPatients();
                ViewBag.Doctors = _doctorService.GetAllActiveDoctors();

                return View(appointment);
            }

            try
            {
                _appointmentService.BookAppointment(
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.ScheduledDate,
                    appointment.SlotNumber);

                TempData["SuccessMessage"] = "Appointment created successfully.";

                return RedirectToAction("Appointments");
            }
            catch (HealthcareAppException ex)
            {
                ViewBag.Patients = _patientService.GetAllPatients();
                ViewBag.Doctors = _doctorService.GetAllActiveDoctors();

                ModelState.AddModelError("", ex.Message);

                return View(appointment);
            }
            catch (Exception)
            {
                ViewBag.Patients = _patientService.GetAllPatients();
                ViewBag.Doctors = _doctorService.GetAllActiveDoctors();

                ModelState.AddModelError(
                    "",
                    "An unexpected error occurred while creating the appointment.");

                return View(appointment);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "Appointments",
            ErrorMessage = "Please select a valid appointment.")]
        public ActionResult EditAppointment(int? id)
        {
            try
            {
                Appointment appointment = _appointmentService.GetAppointmentById(id.Value);

                ViewBag.Patients = _patientService.GetAllPatients();
                ViewBag.Doctors = _doctorService.GetAllActiveDoctors();

                return View(appointment);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Appointments");
            }
        }

        [HttpPost]
        public ActionResult EditAppointment(Appointment postedAppointment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = _patientService.GetAllPatients();
                ViewBag.Doctors = _doctorService.GetAllActiveDoctors();

                return View(postedAppointment);
            }

            try
            {
                Appointment existingAppointment =
                    _appointmentService.GetAppointmentById(postedAppointment.AppointmentId);

                existingAppointment.PatientId = postedAppointment.PatientId;
                existingAppointment.DoctorId = postedAppointment.DoctorId;
                existingAppointment.ScheduledDate = postedAppointment.ScheduledDate;
                existingAppointment.SlotNumber = postedAppointment.SlotNumber;

                _appointmentService.UpdateAppointment(existingAppointment);

                TempData["SuccessMessage"] = "Appointment updated successfully.";

                return RedirectToAction("Appointments");
            }
            catch (HealthcareAppException ex)
            {
                ViewBag.Patients = _patientService.GetAllPatients();
                ViewBag.Doctors = _doctorService.GetAllActiveDoctors();

                ModelState.AddModelError("", ex.Message);

                return View(postedAppointment);
            }
            catch (Exception)
            {
                ViewBag.Patients = _patientService.GetAllPatients();
                ViewBag.Doctors = _doctorService.GetAllActiveDoctors();

                ModelState.AddModelError(
                    "",
                    "An unexpected error occurred while updating the appointment.");

                return View(postedAppointment);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "Appointments",
            ErrorMessage = "Please select a valid appointment to delete.")]
        public ActionResult DeleteAppointment(int? id)
        {
            try
            {
                Appointment appointment = _appointmentService.GetAppointmentById(id.Value);

                return View(appointment);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Appointments");
            }
        }

        [HttpPost]
        [ActionName("DeleteAppointment")]
        public ActionResult ConfirmDeleteAppointment(int? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Appointment ID is required.";

                return RedirectToAction("Appointments");
            }

            try
            {
                _appointmentService.DeleteAppointment(id.Value);

                TempData["SuccessMessage"] = "Appointment deleted successfully.";
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] =
                    "An unexpected error occurred while deleting the appointment.";
            }

            return RedirectToAction("Appointments");
        }

        [RequirePositiveIntParameters(
            "patientId",
            RedirectController = "Admin",
            RedirectAction = "Appointments",
            ErrorMessage = "Please select a valid patient.")]
        public ActionResult AppointmentsByPatient(int? patientId)
        {
            try
            {
                ViewBag.Patient = _patientService.GetPatientById(patientId.Value);

                var appointments =
                    _appointmentService.GetAppointmentsByPatient(patientId.Value);

                return View("Appointments", appointments);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Appointments");
            }
        }

        [RequirePositiveIntParameters(
            "doctorId",
            RedirectController = "Admin",
            RedirectAction = "Appointments",
            ErrorMessage = "Please select a valid doctor.")]
        public ActionResult AppointmentsByDoctor(int? doctorId)
        {
            try
            {
                ViewBag.Doctor = _doctorService.GetDoctorById(doctorId.Value);

                var appointments =
                    _appointmentService.GetAppointmentsByDoctor(doctorId.Value);

                return View("Appointments", appointments);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Appointments");
            }
        }

        public ActionResult HealthRecords()
        {
            var records = _healthRecordService.GetAllRecords();

            return View(records);
        }

        public ActionResult CreateHealthRecord()
        {
            return View(new HealthRecord());
        }

        [HttpPost]
        public ActionResult CreateHealthRecord(HealthRecord record)
        {
            ModelState.Remove("PatientId");
            ModelState.Remove("DoctorId");
            ModelState.Remove("VisitDate");

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

                TempData["SuccessMessage"] = "Health record created successfully.";

                return RedirectToAction("HealthRecords");
            }
            catch (HealthcareAppException ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(record);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "An unexpected error occurred while creating the health record.");

                return View(record);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "HealthRecords",
            ErrorMessage = "Please select a valid health record.")]
        public ActionResult EditHealthRecord(int? id)
        {
            try
            {
                HealthRecord record = _healthRecordService.GetRecordById(id.Value);

                return View(record);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("HealthRecords");
            }
        }

        [HttpPost]
        public ActionResult EditHealthRecord(HealthRecord record)
        {
            if (!ModelState.IsValid)
            {
                return View(record);
            }

            try
            {
                _healthRecordService.UpdateRecord(record);

                TempData["SuccessMessage"] = "Health record updated successfully.";

                return RedirectToAction("HealthRecords");
            }
            catch (HealthcareAppException ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(record);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "An unexpected error occurred while updating the health record.");

                return View(record);
            }
        }

        [RequirePositiveIntParameters(
            "id",
            RedirectController = "Admin",
            RedirectAction = "HealthRecords",
            ErrorMessage = "Please select a valid health record to delete.")]
        public ActionResult DeleteHealthRecord(int? id)
        {
            try
            {
                HealthRecord record = _healthRecordService.GetRecordById(id.Value);

                return View(record);
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("HealthRecords");
            }
        }

        [HttpPost]
        [ActionName("DeleteHealthRecord")]
        public ActionResult ConfirmDeleteHealthRecord(int? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                TempData["ErrorMessage"] = "Valid Health Record ID is required.";

                return RedirectToAction("HealthRecords");
            }

            try
            {
                _healthRecordService.DeleteRecord(id.Value);

                TempData["SuccessMessage"] = "Health record deleted successfully.";
            }
            catch (HealthcareAppException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] =
                    "An unexpected error occurred while deleting the health record.";
            }

            return RedirectToAction("HealthRecords");
        }
    }
}