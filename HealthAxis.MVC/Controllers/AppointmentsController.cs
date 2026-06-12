using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HealthAxis.Mvc.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentMvcService _appointments;
        private readonly IDoctorMvcService _doctors;
        private readonly IPatientMvcService _patients;

        public AppointmentsController(
            IAppointmentMvcService appointments,
            IDoctorMvcService doctors,
            IPatientMvcService patients)
        {
            _appointments = appointments;
            _doctors = doctors;
            _patients = patients;
        }

        private void LoadDropdowns(int? selectedDoctorId = null)
        {
            var doctors = _doctors.GetAll(null, true);

            ViewBag.DoctorId = new SelectList(
                doctors,
                "DoctorId",
                "FullName",
                selectedDoctorId);

            ViewBag.TimeSlots = new SelectList(new List<string>
            {
                "09:00 AM - 09:30 AM",
                "09:30 AM - 10:00 AM",
                "10:00 AM - 10:30 AM",
                "10:30 AM - 11:00 AM",
                "11:00 AM - 11:30 AM",
                "02:00 PM - 02:30 PM",
                "02:30 AM - 03:00 PM",
                "03:00 PM - 03:30 PM"
            });
        }

        public ActionResult Book(int? doctorId)
        {
            LoadDropdowns(doctorId);

            var dto = new AppointmentDto();

            if (doctorId.HasValue)
            {
                dto.DoctorId = doctorId.Value;
            }

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book(AppointmentDto dto)
        {
            string errorMessage;

            bool success = _appointments.Book(dto, out errorMessage);

            if (!success)
            {
                TempData["Error"] = errorMessage;
                return RedirectToAction("Book");
            }

            TempData["Success"] = "Appointment booked successfully.";
            return RedirectToAction("MyAppointments", new { patientId = dto.PatientId });
        }

        public ActionResult MyAppointments(int? patientId, int? selectedPatientId)
        {
            int? finalPatientId = selectedPatientId ?? patientId;

            if (!finalPatientId.HasValue)
            {
                TempData["Error"] = "Please select a patient.";
                return RedirectToAction("Index", "Patients");
            }

            var patient = _patients.GetById(finalPatientId.Value);

            if (patient == null)
            {
                TempData["Error"] = "Patient not found.";
                return RedirectToAction("Index", "Patients");
            }

            var appointments = _appointments.GetByPatient(finalPatientId.Value);

            if (appointments == null || !appointments.Any())
            {
                TempData["Error"] = "No appointments found for " + patient.FullName + ".";
                return RedirectToAction("Index", "Patients");
            }

            return View(appointments);
        }

        public ActionResult DoctorAppointments(int? doctorId, int? selectedDoctorId)
        {
            int? finalDoctorId = selectedDoctorId ?? doctorId;

            if (!finalDoctorId.HasValue)
            {
                TempData["Error"] = "Please select a doctor.";
                return RedirectToAction("Index", "Doctors");
            }

            var doctor = _doctors.GetById(finalDoctorId.Value);

            if (doctor == null)
            {
                TempData["Error"] = "Doctor not found.";
                return RedirectToAction("Index", "Doctors");
            }

            var appointments = _appointments.GetByDoctor(finalDoctorId.Value);

            if (appointments == null || !appointments.Any())
            {
                TempData["Error"] = "No appointments found for " + doctor.FullName + ".";
                return RedirectToAction("Index", "Doctors");
            }

            return View(appointments);
        }

        public ActionResult TodaySchedule(int? doctorId, int? selectedDoctorId)
        {
            int? finalDoctorId = selectedDoctorId ?? doctorId;

            if (!finalDoctorId.HasValue)
            {
                TempData["Error"] = "Please select a doctor.";
                return RedirectToAction("Index", "Doctors");
            }

            var doctor = _doctors.GetById(finalDoctorId.Value);

            if (doctor == null)
            {
                TempData["Error"] = "Doctor not found.";
                return RedirectToAction("Index", "Doctors");
            }

            var appointments = _appointments.Today(finalDoctorId.Value);

            if (appointments == null || !appointments.Any())
            {
                TempData["Error"] = "No appointments scheduled today for " + doctor.FullName + ".";
                return RedirectToAction("Index", "Doctors");
            }

            return View("DoctorAppointments", appointments);
        }

        public ActionResult WeeklySchedule(
    int? doctorId,
    int? selectedDoctorId,
    DateTime? startDate)
        {
            int? finalDoctorId = selectedDoctorId ?? doctorId;

            if (!finalDoctorId.HasValue)
            {
                TempData["Error"] = "Please select a doctor.";
                return RedirectToAction("Index", "Doctors");
            }

            var doctor = _doctors.GetById(finalDoctorId.Value);

            if (doctor == null)
            {
                TempData["Error"] = "Doctor not found.";
                return RedirectToAction("Index", "Doctors");
            }

            var weekStartDate = startDate ?? DateTime.Today;

            var appointments = _appointments.Weekly(
                finalDoctorId.Value,
                weekStartDate);

            if (appointments == null || !appointments.Any())
            {
                TempData["Error"] = "No weekly appointments found for " + doctor.FullName + ".";
                return RedirectToAction("Index", "Doctors");
            }

            return View("DoctorAppointments", appointments);
        }

        public ActionResult PatientIdRequired()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(
            AppointmentStatusUpdateDto dto,
            int patientId,
            int doctorId)
        {
            string errorMessage;

            bool result = _appointments.UpdateStatus(
                dto.AppointmentId,
                dto,
                out errorMessage);

            TempData[result ? "Success" : "Error"] =
                result ? "Status updated." : errorMessage;

            if (result && dto.Status == AppointmentStatusEnum.Completed)
            {
                return RedirectToAction(
                    "Create",
                    "HealthRecords",
                    new
                    {
                        patientId = patientId,
                        doctorId = doctorId,
                        appointmentId = dto.AppointmentId
                    });
            }

            return RedirectToAction(
                "DoctorAppointments",
                new { doctorId = doctorId });
        }
        private PatientDto ResolvePatient(string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                return null;
            }

            var patients = _patients.Search(searchValue.Trim());

            return patients.FirstOrDefault();
        }
    }
}