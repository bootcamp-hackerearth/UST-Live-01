using HealthApp.Service.Interface;
using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthApp.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentApiService _service;

        public AppointmentController(IAppointmentApiService service)
        {
            _service = service;
        }

        // ✅ GET ALL
        public async Task<ActionResult> AppointmentIndex()
        {
            try
            {
                var list = await _service.GetAll();
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<AppointmentDto>());
            }
        }

        // ✅ GET BY ID
        public async Task<ActionResult> GetById(int? id)
        {
            try
            {
                if (!id.HasValue)
                    return RedirectToAction("AppointmentIndex");

                var data = await _service.GetById(id.Value);

                return View("AppointmentIndex", new List<AppointmentDto> { data });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("AppointmentIndex");
            }
        }

        // ✅ GET BY PATIENT
        public async Task<ActionResult> GetByPatientID(int? patientId)
        {
            try
            {
                if (!patientId.HasValue)
                    return RedirectToAction("AppointmentIndex");

                var list = await _service.GetByPatient(patientId.Value);

                return View("AppointmentIndex", list);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("AppointmentIndex");
            }
        }

        // ✅ CONFIRM
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(int id)
        {
            try
            {
                await _service.Confirm(id);

                TempData["Success"] = "Appointment confirmed!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("AppointmentIndex");
        }

        // ✅ CANCEL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(int id)
        {
            try
            {
                await _service.Cancel(id, "Cancelled by user");

                TempData["Success"] = "Appointment cancelled!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("AppointmentIndex");
        }

        // ✅ CREATE - GET
        public ActionResult Create()
        {
            return View();
        }

        // ✅ CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AppointmentDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Please fill all required fields correctly!";
                    return View(dto);
                }

                await _service.Create(dto);

                TempData["Success"] = "Appointment booked successfully!";
                return RedirectToAction("AppointmentIndex");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(dto);
            }
        }

        // ✅ CHECK AVAILABILITY
        public async Task<ActionResult> CheckAvailability(int? doctorId, DateTime? date)
        {
            try
            {
                if (!doctorId.HasValue || !date.HasValue)
                    return RedirectToAction("AppointmentIndex");

                var slots = await _service.CheckAvailability(doctorId.Value, date.Value);

                return View(slots);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("AppointmentIndex");
            }
        }

        // ✅ ✅ ✅ GET PATIENT NAME (AJAX)

        public async Task<JsonResult> GetPatientName(int id)
        {
            var patient = await _service.GetPatient(id);
            return Json(patient, JsonRequestBehavior.AllowGet);
        }



        // ✅ ✅ ✅ GET DOCTORS BY SPECIALIZATION (AJAX)

        public async Task<JsonResult> GetDoctors(string specialization)
        {
            var doctors = await _service.GetDoctors(specialization);
            return Json(doctors, JsonRequestBehavior.AllowGet);
        }



        // ✅ ✅ ✅ GET AVAILABLE SLOTS (AJAX)
        public async Task<JsonResult> GetSlots(int doctorId, DateTime date)
        {
            try
            {
                var slots = await _service.GetAvailableSlots(doctorId, date);
                return Json(slots, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
