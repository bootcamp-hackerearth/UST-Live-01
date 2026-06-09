using HealthAxis_MVC.Services;
using HealthAxis.Shared.Dtos;
using System.Linq;
using System.Web.Http;

namespace HealthAxis_Web.Controllers
{
    [RoutePrefix("api/doctor")] 
    public class DoctorController : ApiController
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllDoctors()
        {
            var doctors = _service.GetAllDoctors();
            return Ok(doctors);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetDoctorById(int id)
        {
            var doctor = _service.GetById(id);
            if (doctor == null)
                return NotFound();

            return Ok(doctor);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddDoctor(DoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _service.AddDoctor(doctorDto);
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateDoctor(int id, DoctorDto docDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _service.UpdateDoctor(id, docDto);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Route("specialisation/{spec}")]
        public IHttpActionResult GetBySpecialisation(DoctorDto.SpecialisationType spec)
        {
            var doctors = _service.GetAllDoctors()
                                  .Where(d => d.Specialisation == spec)
                                  .ToList();

            return Ok(doctors);
        }
    }
}