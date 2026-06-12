using HealthAppWebAPI.Models.Dtos;
using HealthAppWebAPI.Services.Impl;
using HealthAppWebAPI.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

[RoutePrefix("api/appointments")]
public class AppointmentController : ApiController
{
    private readonly IAppointmentService _service;
    private readonly IHealthRecordService _healthRecordService;

    public AppointmentController(IAppointmentService service, IHealthRecordService healthRecordService)
    {
        _service = service;
        _healthRecordService=healthRecordService;
    }
    [HttpGet]
    [Route("")]
    public async Task<IHttpActionResult> GetAll()
    {
        var data = await _service.GetAllAppointmentsAsync();
        return Ok(data);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IHttpActionResult> GetById(int id)
    {
        var all = await _service.GetAllAppointmentsAsync();
        var item = all.Find(a => a.AppointmentId == id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPost]
    [Route("")]
    public async Task<IHttpActionResult> Book(CreateAppointmentDto dto)
    {
        await _service.BookAppointmentAsync(dto);
        return Ok();
    }

    [HttpPost]
    [Route("{id:int}/confirm")]
    public async Task<IHttpActionResult> Confirm(int id)
    {
        await _service.ConfirmAppointmentAsync(id);
        return Ok();
    }

    [HttpPost]
    [Route("{id:int}/cancel")]
    public async Task<IHttpActionResult> Cancel(int id, CancelAppointmentDto dto)
    {
        await _service.CancelAppointmentAsync(id, dto.CancellationReason);
        return Ok();
    }
    [HttpGet]
    [Route("patient/{patientId:int}")]
    public async Task<IHttpActionResult> GetByPatient(int patientId)
    {
        var data = await _service.GetAppointmentsForPatientAsync(patientId);
        return Ok(data);
    }

    [HttpGet]
    [Route("doctor/{doctorId:int}")]
    public async Task<IHttpActionResult> GetByDoctor(int doctorId)
    {
        var data = await _service.GetUpcomingAppointmentsForDoctorAsync(doctorId);
        return Ok(data);
    }

    [HttpGet]
    [Route("upcoming")]
    public async Task<IHttpActionResult> GetUpcoming()
    {
        var data = await _service.GetAllAppointmentsAsync();


        var upcoming = data.Where(a =>
        {
            DateTime date;
            return DateTime.TryParse(a.ScheduledDate, out date) &&
                   date.Date >= DateTime.Today;
        }).ToList();


        return Ok(upcoming);
    }

    [HttpGet]
    [Route("upcoming/doctor/{doctorId:int}")]
    public async Task<IHttpActionResult> GetUpcomingByDoctor(int doctorId)
    {
        var data = await _service.GetUpcomingAppointmentsForDoctorAsync(doctorId);
        return Ok(data);
    }

    [HttpGet]
    [Route("slots")]
    public IHttpActionResult GetAvailableSlots(int doctorId, string date)
    {
        DateTime scheduledDate;

        if (!DateTime.TryParse(date, out scheduledDate))
            return BadRequest("Invalid date format.");

        var slots = HealthAppWebAPI.Constants.TimeSlots.Slots;

        return Ok(slots);
    }

    [HttpGet]
    [Route("search")]
    public async Task<IHttpActionResult> Search(string patientName)
    {
        var data = await _service.GetAllAppointmentsAsync();

        var result = data.FindAll(a =>
            a.PatientName.ToLower().Contains(patientName.ToLower()));

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}/healthrecord")]
    public async Task<IHttpActionResult> HealthRecordExists(int id)
    {
        bool exists = await _healthRecordService.ExistsByAppointmentIdAsync(id);

        return Ok(exists);
    }
}