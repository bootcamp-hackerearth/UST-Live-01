using HealthCare.Shared;
using HealthCare.Shared.DTOs.HealthRecord;
using HealthCare.Web.Services;
using HealthCare.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Web.Mvc;

public class HealthRecordController : Controller
{
    private readonly IHealthRecordService _service;

    private const int PageSize = 10;

    public HealthRecordController(IHealthRecordService service)
    {
        _service = service;
    }

    //  LIST / HISTORY
    
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
            patientId.Value,
            pageNumber,
            PageSize);

        return View("List", result);
    }

    //  ADD (GET)
    
    public ActionResult Create(int appointmentId, int patientId, int doctorId)
    {
        var model = new CreateHealthRecordDto
        {
            AppointmentId = appointmentId,
            PatientId = patientId,
            DoctorId = doctorId,
        };

        return View(model);
    }


    //  ADD (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateHealthRecordDto dto)
    {
        System.Diagnostics.Debug.WriteLine(dto.AppointmentId);
        if (!ModelState.IsValid)
            return View("Create", dto);

        var result = await _service.CreateAsync(dto);

        if (result)
        {
            TempData["Success"] = "Health record added successfully.";
            return RedirectToAction("List", new { patientId = dto.PatientId });
        }

        ModelState.AddModelError("", "Failed to create record");
        return View("Create", dto);
    }

    public async Task<ActionResult> Single(int id)
    {
        try
        {
            var record = await _service.GetByAppointmentIdAsync(id);

            if (record == null)
            {
                ViewBag.Error = "Health record not found.";

                return View("List", new PagedResult<HealthRecordDto>
                {
                    Items = new List<HealthRecordDto>(),
                    PageNumber = 1,
                    PageSize = PageSize,
                    TotalCount = 0
                });
            }

            return View("List", new PagedResult<HealthRecordDto>
            {
                Items = new List<HealthRecordDto> { record },
                PageNumber = 1,
                PageSize = 1,
                TotalCount = 1
            });
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;

            return View("List", new PagedResult<HealthRecordDto>
            {
                Items = new List<HealthRecordDto>(),
                PageNumber = 1,
                PageSize = PageSize,
                TotalCount = 0
            });
        }
    }

    public async Task<ActionResult> GetRecord(int id)
    {
        try
        {
            var record = await _service.GetByIdAsync(id);

            if (record == null)
            {
                ViewBag.Error = "Health record not found.";

                return View("List", new PagedResult<HealthRecordDto>
                {
                    Items = new List<HealthRecordDto>(),
                    PageNumber = 1,
                    PageSize = PageSize,
                    TotalCount = 0
                });
            }

            return View("List", new PagedResult<HealthRecordDto>
            {
                Items = new List<HealthRecordDto> { record },
                PageNumber = 1,
                PageSize = 1,
                TotalCount = 1
            });
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;

            return View("List", new PagedResult<HealthRecordDto>
            {
                Items = new List<HealthRecordDto>(),
                PageNumber = 1,
                PageSize = PageSize,
                TotalCount = 0
            });
        }
    }
}