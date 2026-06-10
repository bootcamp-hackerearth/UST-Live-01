using AutoMapper;
using HealthAxis.Api.Database;
using HealthAxis.Api.Models;
using HealthAxis.Shared.Dtos;
using System.Linq;
using System.Web.Http;

namespace HealthAxis.Api.Controllers
{
    [RoutePrefix("api/healthrecord")]
    public class HealthRecordController : ApiController
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public HealthRecordController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost, Route("")]
        public IHttpActionResult Add(HealthRecordDto dto)
        {
            var appointment = _context.Appointments.Find(dto.AppointmentId);

            if (appointment == null)
            {
                return Ok(new ApiResponseDto
                {
                    Success = false,
                    Message = "Invalid appointment"
                });
            }

            if (appointment.Status != AppointmentStatus.Completed.ToString())
            {
                return Ok(new ApiResponseDto
                {
                    Success = false,
                    Message = "Health record can be added only after completion"
                });
            }

            bool exists = _context.HealthRecords
                .Any(h => h.AppointmentId == dto.AppointmentId);

            if (exists)
            {
                return Ok(new ApiResponseDto
                {
                    Success = false,
                    Message = "Health record already exists"
                });
            }

            var record = _mapper.Map<HealthRecord>(dto);
            record.VisitDate = System.DateTime.Now;

            _context.HealthRecords.Add(record);
            _context.SaveChanges();

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Health record added successfully"
            });
        }

        [HttpGet, Route("patient/{patientId}")]
        public IHttpActionResult GetByPatient(int patientId)
        {
            var records = _context.HealthRecords
                .Where(h => h.PatientId == patientId)
                .OrderByDescending(h => h.VisitDate)
                .ToList();

            var result = _mapper.Map<System.Collections.Generic.List<HealthRecordDto>>(records);

            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var record = _context.HealthRecords.Find(id);

            if (record == null)
                return NotFound();

            return Ok(_mapper.Map<HealthRecordDto>(record));
        }
    }
}