using HealthAxis.Shared.Dtos;
using HealthAxis.Web.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAxis.Web.Controllers
{
    public class HealthRecordController : Controller
    {
        private readonly IHealthRecordApiService _healthService;
        private readonly IAppointmentApiService _appointmentService;

        public HealthRecordController(
            IHealthRecordApiService healthService,
            IAppointmentApiService appointmentService)
        {
            _healthService = healthService;
            _appointmentService = appointmentService;
        }

        public async Task<ActionResult> Create(int appointmentId)
        {
            var appointment = await _appointmentService.GetById(appointmentId);

            if (appointment == null)
            {
                TempData["Error"] = "Invalid appointment";
                return RedirectToAction("Index", "Doctor");
            }

            var dto = new HealthRecordDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                VisitDate = System.DateTime.Now
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Create(HealthRecordDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _healthService.AddHealthRecord(dto);

            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View(dto);
            }

            TempData["Success"] = "Health record added successfully!";

            return RedirectToAction("ByDoctor", "Appointment", new { id = dto.DoctorId });
        }
        public async Task<ActionResult> GetByPatient(int patientId)
        {
            var records = await _healthService.GetByPatient(patientId);

            if (records == null || records.Count == 0)
            {
                ViewBag.Error = "No health records found";
                return View(new System.Collections.Generic.List<HealthRecordDto>());
            }

            return View(records);
        }
    }
}