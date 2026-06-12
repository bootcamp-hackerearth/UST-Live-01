using HealthApp.Service.Interface;
using HealthApp.Shared.DTOs;
using System;
using System.Linq;
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

        // GET ALL
        public async Task<ActionResult> AppointmentIndex()
        {
            var appointments = await _service.GetAll();
            return View(appointments);
        }

        // SEARCH (Professional Combined Filter)
        public async Task<ActionResult> Search(string query, string status)
        {
            try
            {
                var appointments = await _service.GetAll();

                // normalize input
                query = (query ?? "").Trim().ToLower();
                status = (status ?? "").Trim().ToLower();

                // SEARCH (Patient / Doctor / ID)
                if (!string.IsNullOrWhiteSpace(query))
                {
                    appointments = appointments.Where(a =>
                        (a.AppointmentId.ToString() == query) ||

                        (!string.IsNullOrWhiteSpace(a.PatientName) &&
                         a.PatientName.ToLower().Contains(query)) ||

                        (!string.IsNullOrWhiteSpace(a.DoctorName) &&
                         a.DoctorName.ToLower().Contains(query))
                    ).ToList();
                }

                // STATUS FILTER
                if (!string.IsNullOrWhiteSpace(status))
                {
                    appointments = appointments.Where(a =>
                        !string.IsNullOrWhiteSpace(a.Status) &&
                        a.Status.Trim().ToLower() == status
                    ).ToList();
                }

                return View("AppointmentIndex", appointments);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("AppointmentIndex");
            }
        }

        // CONFIRM
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(int id)
        {
            try
            {
                await _service.Confirm(id);
                TempData["Success"] = "Appointment confirmed successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("AppointmentIndex");
        }

        // CANCEL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(int id)
        {
            try
            {
                await _service.Cancel(id, "Cancelled by user");
                TempData["Success"] = "Appointment cancelled successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("AppointmentIndex");
        }

        // CREATE
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AppointmentDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(dto);

                await _service.Create(dto);

                TempData["Success"] = "Appointment booked successfully.";
                return RedirectToAction("AppointmentIndex");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(dto);
            }
        }
    }
}