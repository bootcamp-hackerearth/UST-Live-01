using HealthAxis.Shared.Dtos;
using System;
using System.Web.Http;

[RoutePrefix("api/appointment")]
public class AppointmentController : ApiController
{
    private readonly IAppointmentService _service;

    public AppointmentController(IAppointmentService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("patient/{id}")]
    public IHttpActionResult GetByPatient(int id)
    {
        var result = _service.GetByPatient(id);
        return Ok(result);
    }

    [HttpGet]
    [Route("doctor/{id}")]
    public IHttpActionResult GetByDoctor(int id)
    {
        var result = _service.GetByDoctor(id);
        return Ok(result);
    }

    [HttpPost]
    [Route("")]
    public IHttpActionResult Book(BookAppointmentDto dto)
    {
        if (dto == null)
            return BadRequest("Invalid request");

        var result = _service.Book(dto);

        if (!result.Success)
            return Content(System.Net.HttpStatusCode.BadRequest, result);

        return Ok(result);
    }

    [HttpPut]
    [Route("{id}/status")]
    public IHttpActionResult UpdateStatus(int id, UpdateAppointmentStatusDto dto)
    {
        if (dto == null)
            return BadRequest("Invalid request");

        var result = _service.UpdateStatus(id, dto);

        if (!result.Success)
            return Content(System.Net.HttpStatusCode.BadRequest, result);

        return Ok(result);
    }

    [HttpGet]
    [Route("GetSlots")]
    public IHttpActionResult GetSlots(int doctorId, DateTime date)
    {
        if (doctorId <= 0)
            return BadRequest("Invalid doctorId");

        var slots = _service.GetSlots(doctorId, date);

        return Ok(slots);
    }
}
