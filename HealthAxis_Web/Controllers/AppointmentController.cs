using HealthAxis.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPut]
    [Route("{id}/status")]
    public IHttpActionResult UpdateStatus(int id, UpdateAppointmentStatusDto dto)
    {
        if (dto == null)
            return BadRequest("Invalid request");

        if (dto.Status == AppointmentStatus.Cancelled &&
            string.IsNullOrEmpty(dto.CancellationReason))
        {
            return BadRequest("Cancellation reason required");
        }

        var result = _service.UpdateStatus(id, dto);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpGet]
    [Route("available-slots")]
    public IHttpActionResult GetAvailableSlots(int doctorId, DateTime date)
    {
        if (doctorId <= 0)
            return BadRequest("Invalid doctorId");

        var allSlots = new List<string>
        {
            "10:00 AM",
            "11:00 AM",
            "2:00 PM"
        };

        var bookedSlots = _service.GetBookedSlots(doctorId, date);

        var availableSlots = allSlots.Except(bookedSlots).ToList();

        if (!availableSlots.Any())
        {
            return Ok(new
            {
                Full = true,
                Message = "No slots available"
            });
        }

        return Ok(new
        {
            Full = false,
            Slots = availableSlots
        });
    }
}