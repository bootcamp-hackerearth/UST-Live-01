using AutoMapper;
using HealthAxis.Api.Database;
using HealthAxis.Api.Models;
using HealthAxis.Shared;
using HealthAxis.Shared.Dtos;
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
        public IHttpActionResult GetAll(string specialisation = null)
        {
            var query = _context.Doctors.AsQueryable();

            if (!string.IsNullOrEmpty(specialisation))
            {
                query = query.Where(d => d.Specialisation == specialisation);
            }

            var doctors = query.ToList();

            var result = _mapper.Map<List<DoctorDto>>(doctors);
            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var doctor = _context.Doctors.Find(id);

            if (doctor == null)
                return NotFound();

            var doctorDto = _mapper.Map<DoctorDto>(doctor);

            var upcomingCount = _context.Appointments
                .Count(a => a.DoctorId == id &&
                            a.ScheduledDate >= System.DateTime.Today &&
                            a.Status != "Cancelled");

            doctorDto.UpcomingAppointments = upcomingCount;

            return Ok(doctorDto);
        }

        [HttpPost, Route("")]
        public IHttpActionResult Add(CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            var doctor = new Doctor
            {
                FullName = dto.FullName,
                Specialisation = dto.Specialisation.ToString(),
                YearsOfExperience = dto.YearsOfExperience.Value,
                ConsultationFee = dto.ConsultationFee.Value,
                IsActive = true
            };

            _context.Doctors.Add(doctor);
            _context.SaveChanges();

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Doctor added successfully"
            });
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, UpdateDoctorDto dto)
        {
            var doctor = _context.Doctors.Find(id);

            if (doctor == null)
                return NotFound();

            if (dto.ConsultationFee <= 0)
                return BadRequest("Invalid fee");

            doctor.FullName = dto.FullName;
            doctor.Specialisation = dto.Specialisation.ToString();
            doctor.YearsOfExperience = dto.YearsOfExperience;
            doctor.ConsultationFee = dto.ConsultationFee;
            doctor.IsActive = dto.IsActive;

            _context.SaveChanges();

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Doctor updated successfully"
            });
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
        [HttpPut, Route("{id}/toggle")]
        public IHttpActionResult Toggle(int id)
        {
            var doctor = _context.Doctors.Find(id);

            if (doctor == null)
                return NotFound();

            doctor.IsActive = !doctor.IsActive;

            _context.SaveChanges();

            return Ok(new { doctor.IsActive });
        }

    }
}