using HealthCare.Shared.DTOs.Appointment;
using HealthCare.Shared.DTOs.Doctor;
using HealthCare.Web.Services;
using HealthCare.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare.Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _service;
        private const int PageSize = 10;

        public AppointmentController()
        {
            _service = new AppointmentService();
        }

        //  INDEX
        public ActionResult Index()
        {
            int patientId = Convert.ToInt32(Session["PatientId"] ?? 1);
            return RedirectToAction("List", new { patientId });
        }

        //  BOOK
        [HttpPost]
        public async Task<ActionResult> Book(CreateAppointmentDto dto)
        {
            int patientId = Convert.ToInt32(Session["PatientId"] ?? 1);

            var appointment = new AppointmentDto
            {
                PatientId = patientId,
                DoctorId = dto.DoctorId,
                ScheduledDate = dto.ScheduledDate,
                TimeSlot = dto.TimeSlot
            };

            await _service.BookAsync(appointment);

            return RedirectToAction("List", new { patientId });
        }

        //  CONFIRM
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(int id, int patientId)
        {
            await _service.ConfirmAsync(id);
            return RedirectToAction("List", new { patientId });
        }

        //  CANCEL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(int id, int patientId, string reason)
        {
            await _service.CancelAsync(id, reason);
            return RedirectToAction("List", new { patientId });
        }

        //  COMPLETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Complete(int id, int patientId)
        {
            await _service.ConfirmAsync(id);
            return RedirectToAction("List", new { patientId });
        }

        //  LOAD DOCTORS BY SPECIALIZATION
        public async Task<ActionResult> GetDoctorsBySpecialisation(string specialization)
        {
            var doctorService = new DoctorService();
            var doctors = await doctorService.GetAllAsync();

            var filtered = doctors
                .Where(d => d.Specialisation == specialization)
                .ToList();

            return Json(filtered, JsonRequestBehavior.AllowGet);
        }

        //  LOAD AVAILABLE SLOTS
        public async Task<ActionResult> GetAvailableSlots(int doctorId, DateTime date)
        {
            var slots = await _service.GetAvailableSlotsAsync(doctorId, date);
            return Json(slots, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> List(int? patientId, string status, int pageNumber = 1)
        {
            //  Handle NULL safely
            if (!patientId.HasValue)
            {
                // fallback from session OR redirect
                int sessionPatientId = Convert.ToInt32(Session["PatientId"] ?? 0);

                if (sessionPatientId == 0)
                {
                    TempData["Error"] = "Please select a patient";
                    return RedirectToAction("List", "Patient");
                }

                patientId = sessionPatientId;
            }

            //  Call service
            var result = await _service.GetPatientAppointmentsAsync(
                patientId.Value,
                status,
                pageNumber,
                PageSize);

            //  Load doctors
            var doctorService = new DoctorService();
            var doctors = await doctorService.GetAllAsync();

            //  Send data to View
            ViewBag.Doctors = doctors ?? new List<DoctorDto>();
            ViewBag.PatientId = patientId.Value;

            return View(result);
        }
        public ActionResult Book()
        {
            return View(new AppointmentDto());
        }




    }
}
