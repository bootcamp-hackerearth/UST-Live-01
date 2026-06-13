using HealthCare.Shared;
using HealthCare.Shared.DTOs.Appointment;
using HealthCare.Web.Services;
using HealthCare.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HealthCare.Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _service;
        private const int PageSize = 10;

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }

        //  LIST (Patient appointments)
        public async Task<ActionResult> List(
            int? patientId,
            int? doctorId,
            string status,
            int pageNumber = 1)
        {
            var result = await _service.GetAppointmentsAsync(
                patientId,
                doctorId,
                status,
                pageNumber,
                PageSize
            );

            return View(result);
        }

        //  BOOK (GET)
        public ActionResult Book()
        {
            return View("Book");
        }

        //  BOOK (POST)
        [HttpPost]
        public async Task<ActionResult> Book(AppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var result = await _service.BookAsync(dto);

                if (result)
                {
                    TempData["Success"] = "Appointment booked successfully.";
                    return RedirectToAction("List", new { patientId = dto.PatientId });
                }
            }
            catch (Exception ex)
            {
                // SHOW API ERROR HERE

                // Best: show under TimeSlot
                ModelState.AddModelError("TimeSlot", ex.Message);

                // OR (if you want top-level)
                
            }

            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(int id, int patientId)
        {
            await _service.ConfirmAsync(id);

            TempData["Success"] = "Appointment confirmed successfully";

            return RedirectToAction("List", new { patientId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(int id, int patientId, string reason)
        {
            System.Diagnostics.Debug.WriteLine(patientId, reason);
            await _service.CancelAsync(id, reason);

            TempData["Success"] = "Appointment cancelled successfully";

            return RedirectToAction("List", new { patientId });
        }

        //  BY DATE
        public async Task<ActionResult> ByDate(DateTime date)
        {
            var data = await _service.GetByDateAsync(date);
            return View("ByDate", data);
        }

        [HttpGet]
        public async Task<ActionResult> GetSlots(int doctorId, DateTime date)
        {
            var slots = await _service
                .GetAvailableSlotsAsync(doctorId, date);

            return Json(slots, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Upcoming(
            int doctorId,
            string status,
            int pageNumber = 1)
        {
            try
            {
                ViewBag.ActiveTab = "Upcoming";

                var result = await _service.GetUpcomingAppointmentsAsync(
                    null,
                    doctorId,
                    status,
                    pageNumber,
                    PageSize);

                return View("List", result);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("List", new List<AppointmentDto>());
            }
        }

        public ActionResult BookPartial()
        {
            return PartialView("_BookPartial");
        }
    }
}