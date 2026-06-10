using HealthCare.Shared;
using HealthCare.Shared.DTOs.HealthRecord;
using HealthCare.Web.Services;
using HealthCare.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare.Web.Controllers
{
    public class HealthRecordController : Controller
    {
        private readonly IHealthRecordService _service;
        private const int PageSize = 10;

        public HealthRecordController()
        {
            _service = new HealthRecordService();
        }

        public async Task<ActionResult> List(int? patientId, int pageNumber = 1)
        {
            if (!patientId.HasValue)
            {
                return View(new PagedResult<HealthRecordDto>
                {
                    Items = new List<HealthRecordDto>(),
                    PageNumber = 1,
                    PageSize = PageSize
                });
            }

            var result = await _service.GetPatientHealthHistoryAsync(
                patientId.Value, pageNumber, PageSize);

            return View(result);
        }

        public ActionResult CreatePartial(int? patientId, int? doctorId, int? appointmentId)
        {
            var model = new CreateHealthRecordDto
            {
                PatientId = patientId ?? 0,
                DoctorId = doctorId ?? 0,
                AppointmentId = appointmentId ?? 0,
                VisitDate = DateTime.Now
            };

            return PartialView("_CreatePartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
                return PartialView("_CreatePartial", dto);

            var result = await _service.CreateAsync(dto);

            if (result)
                return Json(new { success = true, message = "Health record added successfully." });

            ModelState.AddModelError("", "Failed to create record. Please try again.");
            return PartialView("_CreatePartial", dto);
        }

        public async Task<ActionResult> ViewPartial(int id)
        {
            var record = await _service.GetByIdAsync(id);

            if (record == null)
                return HttpNotFound();

            return PartialView("_ViewPartial", record);
        }
    }
}
