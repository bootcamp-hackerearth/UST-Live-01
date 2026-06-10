using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System.Web.Mvc;
using System.Linq;

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

        public ActionResult PatientHistory(int? patientId)
        {
            if (patientId == null)
            {
                TempData["Error"] = "Please enter a Patient ID.";
                return RedirectToAction("PatientIdRequired");
            }

            var patient = _patients.GetById(patientId.Value);

            if (patient == null)
            {
                TempData["Error"] = "Patient ID " + patientId.Value + " does not exist.";
                return RedirectToAction("PatientIdRequired");
            }

            var records = _records.GetByPatient(patientId.Value);

            if (records == null || !records.Any())
            {
                TempData["Error"] = "No health records found for Patient ID " + patientId.Value + ".";
                return RedirectToAction("PatientIdRequired");
            }

            return View(records);
        }

        public ActionResult Create(int patientId, int doctorId)
        {
            var dto = new HealthRecordDto
            {
                PatientId = patientId,
                DoctorId = doctorId
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

            return RedirectToAction(
                "PatientHistory",
                new { patientId = dto.PatientId });
        }
    }
}