using AutoMapper;
using HealthAxis.Shared.Dtos;
using HealthAxis.Api.Database;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HealthAxis.Api.Models;

namespace HealthAxis.Api.Controllers
{
    [RoutePrefix("api/patient")]
    public class PatientController : ApiController
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public PatientController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost, Route("")]
        public IHttpActionResult Add(PatientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            if (_context.Patients.Any(p => p.Email == dto.Email))
                return BadRequest("Email already exists");

            var patient = _mapper.Map<Patient>(dto);
            patient.CreatedDate = System.DateTime.Now;
            patient.IsActive = true;

            _context.Patients.Add(patient);
            _context.SaveChanges();

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Patient registered successfully"
            });
        }

        [HttpGet, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var p = _context.Patients.Find(id);
            if (p == null)
                return NotFound();

            return Ok(_mapper.Map<PatientDto>(p));
        }

        [HttpPut, Route("{id}")]
        public IHttpActionResult Update(int id, PatientDto dto)
        {
            var patient = _context.Patients.Find(id);

            if (patient == null)
            {
                return Ok(new ApiResponseDto
                {
                    Success = false,
                    Message = "Patient not found"
                });
            }

            _mapper.Map(dto, patient);
            _context.SaveChanges();

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Patient updated successfully"
            });
        }


        [HttpGet, Route("")]
        public IHttpActionResult GetAll()
        {
            var patients = _context.Patients.ToList();
            return Ok(_mapper.Map<List<PatientDto>>(patients));
        }
    }
}