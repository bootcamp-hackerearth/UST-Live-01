using HealthAxis.Shared.Dtos;
using HealthAxis.Web.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

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
    public async Task<ActionResult> MyAppointments(int id)
    {
        var data = await _service.GetByPatient(id);
        return View(data);
    }

    public ActionResult Book(int patientId)
    {
        return View(new BookAppointmentDto { PatientId = patientId });
    }

    [HttpPost]
    public async Task<ActionResult> Book(BookAppointmentDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _service.Book(dto);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            return View(dto);
        }

        return RedirectToAction("MyAppointments", new { id = dto.PatientId });
    }

    public async Task<ActionResult> DoctorSchedule(int id)
    {
        var data = await _service.GetByDoctor(id);
        return View(data);
    }
    public ActionResult Redirect(string role, int id)
    {
        if (role == "patient")
        {
            return RedirectToAction("MyAppointments", new { id = id });
        }
        else if (role == "doctor")
        {
            return RedirectToAction("DoctorSchedule", new { id = id });
        }

        return RedirectToAction("Index");
    }
}
