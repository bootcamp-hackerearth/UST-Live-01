using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Collections.Generic; // Added for List<string>
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class AppointmentController : Controller
    {
        // ==================================
        // CONSTANTS (Fixes Sonar S1192)
        // ==================================
        private const string ErrorKey = "Error";
        private const string SuccessKey = "Success";
        private const string DetailsAction = "Details";
        private const string IndexAction = "Index";
        private const string FullNameField = "FullName";
        private const string DoctorIdField = "DoctorId";
        private const string PatientIdField = "PatientId";
        private const string ReferenceIdKey = "ReferenceId";

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

        // ==================================
        // HELPER METHODS
        // ==================================

        // Extracted to prevent Sonar Duplicate Code violations
        private async Task PopulateDropdownsAsync()
        {
            var doctors = (await _doctorService.GetAllDoctorsAsync()).Where(d => d.IsActive);
            var patients = await _patientService.GetAllPatientsAsync();

            ViewBag.Doctors = new SelectList(doctors, DoctorIdField, FullNameField);
            ViewBag.Patients = new SelectList(patients, PatientIdField, FullNameField);

            // Added TimeSlots here so it is always available for both GET and POST requests
            var timeSlots = new List<string>
            {
                "09:00 AM - 09:30 AM",
                "09:30 AM - 10:00 AM",
                "10:00 AM - 10:30 AM",
                "10:30 AM - 11:00 AM",
                "11:00 AM - 11:30 AM",
                "11:30 AM - 12:00 PM",
                "12:00 PM - 12:30 PM",
                "12:30 PM - 01:00 PM",
                "02:00 PM - 02:30 PM",
                "02:30 PM - 03:00 PM",
                "03:00 PM - 03:30 PM",
                "03:30 PM - 04:00 PM",
                "04:00 PM - 04:30 PM",
                "04:30 PM - 05:00 PM"
            };
            ViewBag.TimeSlots = new SelectList(timeSlots);
        }

        // ==================================
        // ADMIN / COMMON
        // ==================================

        public async Task<ActionResult> Index()
        {
            try
            {
                var appointments = await _appointmentService.GetAllAppointmentsAsync();
                return View(IndexAction, appointments);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return View(IndexAction, Enumerable.Empty<AppointmentDto>());
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
                return View(DetailsAction, appointment);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return RedirectToAction(IndexAction);
            }
        }

        // ==================================
        // CREATE
        // ==================================

        public async Task<ActionResult> Create()
        {
            await PopulateDropdownsAsync();

            return View("Create", new CreateAppointmentDto
            {
                ScheduledDate = DateTime.Today
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View("Create", dto);
            }

            try
            {
                int appointmentId = await _appointmentService.CreateAppointmentAsync(dto);
                TempData[SuccessKey] = "Appointment created successfully.";
                return RedirectToAction(DetailsAction, new { id = appointmentId });
            }
            catch (Exception ex)
            {
                await PopulateDropdownsAsync();
                ModelState.AddModelError("", ex.Message);
                return View("Create", dto);
            }
        }

        // ==================================
        // UPDATE (EDIT)
        // ==================================

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentByIdAsync(id);

                // Map existing values so the view populates the Date and Dropdowns correctly
                var updateDto = new UpdateAppointmentDto
                {
                    DoctorId = appointment.DoctorId,
                    ScheduledDate = appointment.ScheduledDate,
                    TimeSlot = appointment.TimeSlot
                };

                await PopulateDropdownsAsync();
                return View("Edit", updateDto);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return RedirectToAction(IndexAction);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UpdateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View("Edit", dto);
            }

            try
            {
                await _appointmentService.UpdateAppointmentAsync(id, dto);
                TempData[SuccessKey] = "Appointment updated successfully.";
                return RedirectToAction(DetailsAction, new { id });
            }
            catch (Exception ex)
            {
                await PopulateDropdownsAsync();
                ModelState.AddModelError("", ex.Message);
                return View("Edit", dto);
            }
        }

        // ==================================
        // DELETE
        // ==================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _appointmentService.DeleteAppointmentAsync(id);
                TempData[SuccessKey] = "Appointment deleted permanently.";
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
            }

            return RedirectToAction(IndexAction);
        }

        // ==================================
        // PATIENT ACTIONS
        // ==================================

        public async Task<ActionResult> MyAppointments()
        {
            try
            {
                if (Session[ReferenceIdKey] == null)
                {
                    return RedirectToAction("Login", "User");
                }

                int patientId = Convert.ToInt32(Session[ReferenceIdKey]);
                var appointments = await _appointmentService.GetAppointmentsByPatientAsync(patientId);

                return View("MyAppointments", appointments);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return RedirectToAction("Dashboard", "Patient");
            }
        }

        // ==================================
        // STATUS WORKFLOW ACTIONS
        // ==================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(int id)
        {
            try
            {
                await _appointmentService.ConfirmAppointmentAsync(id);
                TempData[SuccessKey] = "Appointment confirmed.";
                return RedirectToAction(DetailsAction, new { id });
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return RedirectToAction(DetailsAction, new { id });
            }
        }

        public async Task<ActionResult> Cancel(int id)
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
                return View("Cancel", appointment);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return RedirectToAction(IndexAction);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(int id, string reason)
        {
            try
            {
                await _appointmentService.CancelAppointmentAsync(id, reason);
                TempData[SuccessKey] = "Appointment cancelled.";
                return RedirectToAction(DetailsAction, new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
                return View("Cancel", appointment);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Complete(int id)
        {
            try
            {
                await _appointmentService.CompleteAppointmentAsync(id);
                TempData[SuccessKey] = "Appointment completed successfully.";
                return RedirectToAction(IndexAction);
            }
            catch (Exception ex)
            {
                TempData[ErrorKey] = ex.Message;
                return RedirectToAction(DetailsAction, new { id });
            }
        }
    }
}