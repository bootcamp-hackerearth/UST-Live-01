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

        public ActionResult MyAppointments(int? patientId)
        {
            if (patientId == null)
            {
                TempData["Error"] = "Please enter a Patient ID.";
                return RedirectToAction("Index","Patients");
            }

            var patient = _patients.GetById(patientId.Value);

            if (patient == null)
            {
                TempData["Error"] = "Patient ID " + patientId.Value + " does not exist.";
                return RedirectToAction("Index","Patients");
            }

            var appointments = _appointments.GetByPatient(patientId.Value);

            if (appointments == null || !appointments.Any())
            {
                TempData["Error"] = "No appointments found for Patient ID " + patientId.Value + ".";
                return RedirectToAction("Index","Patients");
            }

            return View(appointments);
        }

        public ActionResult DoctorAppointments(int? doctorId, int? cancelId = null)
        {
            ViewBag.CancelId = cancelId;

            if (doctorId == null)
            {
                return View("DoctorIdRequired");
            }

            var appointments = _appointments.GetByDoctor(doctorId.Value);

            return View(appointments);
        }

        public ActionResult TodaySchedule(int? doctorId)
        {
            if (doctorId == null)
            {
                return View("DoctorIdRequired");
            }

            var appointments = _appointments.Today(doctorId.Value);

            return View("DoctorAppointments", appointments);
        }

        public ActionResult WeeklySchedule(int? doctorId, DateTime? startDate)
        {
            if (doctorId == null)
            {
                return View("DoctorIdRequired");
            }

            var weekStartDate = startDate ?? DateTime.Today;

            var appointments = _appointments.Weekly(
                doctorId.Value,
                weekStartDate);

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
    }
}