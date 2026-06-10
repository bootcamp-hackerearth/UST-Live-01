using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Shared.Dtos;
using HealthAxis.Api.Database;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HealthAxis.Api.Controllers
{
    [RoutePrefix("api/doctor")]
    public class DoctorController : ApiController
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public DoctorController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet, Route("")]
        public IHttpActionResult GetAll()
        {
            var doctors = _context.Doctors.ToList();
            var result = _mapper.Map<List<DoctorDto>>(doctors);
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var doctor = _context.Doctors.Find(id);
            if (doctor == null)
                return NotFound();

            return Ok(_mapper.Map<DoctorDto>(doctor));
        }

        [HttpPost, Route("")]
        public IHttpActionResult Add(DoctorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            var doctor = _mapper.Map<Doctor>(dto);
            doctor.IsActive = true;

            _context.Doctors.Add(doctor);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut, Route("{id}")]
        public IHttpActionResult Update(int id, DoctorDto dto)
        {
            var doctor = _context.Doctors.Find(id);
            if (doctor == null)
                return NotFound();

            _mapper.Map(dto, doctor);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet, Route("specialisation/{spec}")]
        public IHttpActionResult GetBySpec(string spec)
        {
            var doctors = _context.Doctors
                .Where(d => d.Specialisation == spec && d.IsActive)
                .ToList();

            var result = _mapper.Map<List<DoctorDto>>(doctors);
            return Ok(result);
        }
    }
}