using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System;
using System.Linq;
using System.Web.Mvc;

namespace HealthAxis.Mvc.Controllers
{
    public class HealthRecordsController : Controller
    {
        private readonly IHealthRecordMvcService _records;
        private readonly IPatientMvcService _patients;

        public HealthRecordsController(IHealthRecordMvcService records, IPatientMvcService patient)
        {
            _records = records;
            _patients = patient;
        }

        public ActionResult PatientIdRequired()
        {
            return View();
        }

        public ActionResult PatientHistory(int? patientId, int? selectedPatientId)
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

            var records = _records.GetByPatient(finalPatientId.Value);

            if (records == null || !records.Any())
            {
                TempData["Error"] = "No health records found for " + patient.FullName + ".";
                return RedirectToAction("Index", "Patients");
            }

            return View(records);
        }

        public ActionResult Create(int patientId, int doctorId, int? appointmentId)
        {
            if (appointmentId.HasValue)
            {
                var existingRecord = _records.GetByAppointmentId(appointmentId.Value);

                if (existingRecord != null)
                {
                    TempData["Error"] = "A health record already exists for this appointment.";
                    return RedirectToAction(
                        "DoctorAppointments",
                        "Appointments",
                        new { doctorId = doctorId });
                }
            }

            var dto = new HealthRecordDto
            {
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentId = appointmentId,
                VisitDate = DateTime.Now
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            string errorMessage;

            bool result = _records.Create(dto, out errorMessage);

            if (!result)
            {
                ModelState.AddModelError("", errorMessage);
                return View(dto);
            }

            TempData["Success"] = "Health record added successfully.";

            return RedirectToAction(
                "PatientHistory",
                new { patientId = dto.PatientId });
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