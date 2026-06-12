using System.Threading.Tasks;
using System.Web.Mvc;
using Healthaxis2.Shared.DTOs;
using Healthaxis2.Web.Services.Interfaces;

namespace Healthaxis2.Web.Controllers
{
    public class HealthRecordController : Controller
    {
        private readonly IHealthRecordService _service;

        public HealthRecordController(IHealthRecordService service)
        {
            _service = service;
        }

        // ✅ CREATE RECORD (After appointment completion)
        public ActionResult Create(int appointmentId)
        {
            var dto = new HealthRecordDto
            {
                AppointmentId = appointmentId
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Create(HealthRecordDto dto)
        {
            await _service.Create(dto);

            return RedirectToAction("Index", "Doctor");
        }

        // ✅ PATIENT HISTORY
        public ActionResult PatientHistory()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PatientHistory(int patientId)
        {
            var records = await _service.GetByPatient(patientId);
            return View("PatientHistoryResult", records);
        }

        // ✅ DOCTOR VIEW RECORDS
        public async Task<ActionResult> DoctorRecords(int doctorId)
        {
            var records = await _service.GetByPatient(doctorId);

            return View(records);
        }
    }
}