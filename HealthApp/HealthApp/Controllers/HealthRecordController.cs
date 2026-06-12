using HealthApp.Service.Interface;
using HealthApp.Shared.DTOs;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthApp.Controllers
{
    public class HealthRecordController : Controller
    {
        private readonly IHealthRecordApiService _service;
        private readonly IAppointmentApiService _appointmentService;

        public HealthRecordController(
            IHealthRecordApiService service,
            IAppointmentApiService appointmentService)
        {
            _service = service;
            _appointmentService = appointmentService;
        }

        // ✅ GET ALL
        public async Task<ActionResult> HealthRecordsIndex()
        {
            var records = await _service.GetAll();
            return View(records);
        }

        // ✅ CREATE (GET)
        public ActionResult Create()
        {
            return View();
        }

        // ✅ CREATE (POST)
        [HttpPost]
        public async Task<ActionResult> Create(HealthRecordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                await _service.Create(dto);

                TempData["Success"] = "Health Record added!";
                return RedirectToAction("HealthRecordsIndex");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(dto);
            }
        }

        // ✅ FROM APPOINTMENT (GET)
        public async Task<ActionResult> CreateFromAppointment(int appointmentId)
        {
            var appointment = await _appointmentService.GetById(appointmentId);

            var model = new HealthRecordDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                PatientName = appointment.PatientName,
                DoctorName = appointment.DoctorName,
                VisitDate = appointment.ScheduledDate
            };

            return View(model);
        }

        // ✅ FROM APPOINTMENT (POST)
        [HttpPost]
        public async Task<ActionResult> CreateFromAppointment(HealthRecordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                await _service.Create(dto);

                await _appointmentService.MarkCompleted(dto.AppointmentId);

                TempData["Success"] = "Health Record added!";
                return RedirectToAction("AppointmentIndex", "Appointment");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(dto);
            }
        }

        // ✅ ✅ ✅ SEARCH BY DOCTOR + PATIENT 
        [HttpGet]
        public async Task<ActionResult> GetRecordsByDoctorAndPatient(int? doctorId, int? patientId)
        {
            try
            {
                // ✅ ✅ VALIDATION (IMPORTANT)
                if (!doctorId.HasValue && !patientId.HasValue)
                {
                    TempData["Error"] = "Please enter Doctor Id or Patient Id";
                    return RedirectToAction("HealthRecordsIndex");
                }

                var records = await _service.GetRecordsByDoctorAndPatient(doctorId, patientId);

                return View("HealthRecordsIndex", records);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("HealthRecordsIndex");
            }
        }


        // ✅ ✅ ✅ GET PATIENT RECORDS 
        [HttpGet]
        public async Task<ActionResult> GetPatientRecords(int patientId)
        {
            try
            {
                var records = await _service.GetPatientRecords(patientId);

                return View("GetPatientRecords", records);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("HealthRecordsIndex");
            }
        }
    }
}