using HealthAppMVC.Services.Interface;
using HealthAppWebAPI.Models.Dtos;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAppMVC.Controllers
{
    public class HealthRecordController : Controller
    {
        private readonly IHealthRecordService _healthRecordService;
        private readonly IAppointmentService _appointmentService;

        public HealthRecordController(
            IHealthRecordService healthRecordService,
            IAppointmentService appointmentService)
        {
            _healthRecordService = healthRecordService;
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<ActionResult> Create(int appointmentId)
        {
            try
            {
                bool healthRecordExists =
                    await _appointmentService.HealthRecordExistsAsync(appointmentId);

                if (healthRecordExists)
                {
                    return Content(
                        "<div class='modal-header healthrecord-modal-header'>" +
                            "<h5 class='modal-title'>" +
                                "<i class='bi bi-journal-medical'></i> Health Record" +
                            "</h5>" +
                            "<button type='button' class='btn-close btn-close-white' data-bs-dismiss='modal' aria-label='Close'></button>" +
                        "</div>" +

                        "<div class='modal-body'>" +
                            "<div class='alert alert-warning'>" +
                                "Health record already exists for this appointment." +
                            "</div>" +
                        "</div>" +

                        "<div class='modal-footer'>" +
                            "<button type='button' class='btn btn-secondary btn-rounded' data-bs-dismiss='modal'>Close</button>" +
                        "</div>"
                    );
                }

                var model = new CreateHealthRecordDto
                {
                    AppointmentId = appointmentId
                };

                return PartialView("_CreateHealthRecordModal", model);
            }
            catch (Exception ex)
            {
                return Content(
                    "<div class='modal-header healthrecord-modal-header'>" +
                        "<h5 class='modal-title'>" +
                            "<i class='bi bi-journal-medical'></i> Health Record" +
                        "</h5>" +
                        "<button type='button' class='btn-close btn-close-white' data-bs-dismiss='modal' aria-label='Close'></button>" +
                    "</div>" +

                    "<div class='modal-body'>" +
                        "<div class='alert alert-danger'>" +
                            Server.HtmlEncode(ex.Message) +
                        "</div>" +
                    "</div>" +

                    "<div class='modal-footer'>" +
                        "<button type='button' class='btn btn-secondary btn-rounded' data-bs-dismiss='modal'>Close</button>" +
                    "</div>"
                );
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateHealthRecordModal", dto);
            }

            try
            {
                await _healthRecordService.AddHealthRecordAsync(dto);

                return Json(new
                {
                    success = true,
                    message = "Health record added successfully. Appointment completed."
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return PartialView("_CreateHealthRecordModal", dto);
            }
        }
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var record = await _healthRecordService.GetByIdAsync(id);

                return View(record);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("Index", "Patient");
            }
        }

    }
}