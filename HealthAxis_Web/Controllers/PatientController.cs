using System.Linq;
using System.Web.Http;
using HealthAxis.Shared.Dtos;

[RoutePrefix("api/patient")]
public class PatientController : ApiController
{
    private readonly IPatientService _service;

    public PatientController(IPatientService service)
    {
        _service = service;
    }

    [HttpGet, Route("")]
    public IHttpActionResult GetAll()
    {
        return Ok(_service.GetAllPatients());
    }

    [HttpGet, Route("{id}")]
    public IHttpActionResult GetById(int id)
    {
        var patient = _service.GetById(id);
        if (patient == null) return NotFound();

        return Ok(patient);
    }

    [HttpPost, Route("")]
    public IHttpActionResult Add(PatientDto dto)
    {
        var result = _service.AddPatient(dto);
        return Ok(result);
    }

    [HttpPut, Route("{id}")]
    public IHttpActionResult Update(int id, PatientDto dto)
    {
        var result = _service.UpdatePatient(id, dto);
        if (result == null) return NotFound();

        return Ok(result);
    }
}
