using HealthAxis.Shared.Dtos;
using HealthAxis.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAxis.Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentApiService _service;

        public AppointmentController(IAppointmentApiService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Book(int patientId)
        {
            var dto = new BookAppointmentDto
            {
                PatientId = patientId
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Book(BookAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result = await _service.Book(dto);

            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View(dto);
            }

            return RedirectToAction("MyAppointments", new { id = dto.PatientId });
        }

        public async Task<ActionResult> DoctorSchedule(int id)
        {
            var appointments = await _service.GetByDoctor(id);

            if (appointments == null || appointments.Count == 0)
            {
                ViewBag.Error = "No appointments found";
                return View(new List<AppointmentDto>());
            }

            return View(appointments);
        }

        public ActionResult Redirect(string role, int id)
        {
            if (role == "patient")
            {
                return RedirectToAction("MyAppointments", new { id = id });
            }

            if (role == "doctor")
            {
                return RedirectToAction("DoctorSchedule", new { id = id });
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public JsonResult GetSlots()
        {
            var slots = new List<string>
            {
                "10:00 - 11:00 AM",
                "11:00 - 12:00 PM",
                "2:00 - 3:00 PM"
            };

            return Json(slots, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> MyAppointments(int id)
        {
            ViewBag.PatientId = id; 

    var appointments = await _service.GetByPatient(id);

            if (appointments == null)
                appointments = new List<AppointmentDto>();

            appointments = appointments
                .OrderByDescending(a => a.ScheduledDate)
                .ThenByDescending(a => a.AppointmentId)
                .ToList();

            return View(appointments);
        }
        [HttpPost]
        public async Task<JsonResult> Cancel(int id, string reason, string role)
        {
            var dto = new UpdateAppointmentStatusDto
            {
                Status = AppointmentStatus.Cancelled,
                CancellationReason = reason,
                CancelledBy = role
            };

            var result = await _service.Cancel(id, dto);

            return Json(result.Success);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateStatus(int id, string Status, string CancellationReason, string CancelledBy)
        {
            var dto = new UpdateAppointmentStatusDto
            {
                Status = (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), Status),
                CancellationReason = CancellationReason,
                CancelledBy = CancelledBy
            };

            var result = await _service.UpdateStatus(id, dto);

            return Json(new
            {
                Success = result.Success,
                Message = result.Message
            });
        }
    }
}