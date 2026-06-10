using HealthCare.Shared;
using HealthCare.Shared.DTOs.HealthRecord;
using HealthCare.Web.Services;
using HealthCare.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Mvc;

public class HealthRecordController : Controller
{
    private readonly IHealthRecordService _service;
    private const int PageSize = 10;

    public HealthRecordController()
    {
        _service = new HealthRecordService();
    }

    // ✅ LIST / HISTORY
    // → HealthRecord/List.cshtml
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

    // ✅ ADD (GET)
    // → HealthRecord/Create.cshtml
    public ActionResult Create(int patientId)
    {
        var model = new CreateHealthRecordDto
        {
            PatientId = patientId
        };

        return View("Create", model);
    }

    public async Task<ActionResult> Single(int id)
    {
        try
        {
            var record = await _service.GetByIdAsync(id);

            if (record == null)
            {
                ViewBag.Error = "Health record not found.";
                return View();
            }

            return View(record);
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View();
        }

        
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<HealthRecordDto> CreateAsync(HealthRecordDto dto)
    {
        if (!ModelState.IsValid)
            return View("Create", dto);

        var result = await _service.CreateAsync(dto);

        if (result != null)
        {
            TempData["Success"] = "Health record added successfully.";
            return RedirectToAction("List", new { patientId = dto.PatientId });
        }

        ModelState.AddModelError("", "Failed to create record");
        return View("Create", dto);
    }

}