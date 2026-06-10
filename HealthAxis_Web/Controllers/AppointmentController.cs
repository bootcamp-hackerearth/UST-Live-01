using AutoMapper;
using HealthAxis.Shared.Dtos;
using HealthAxis.Api.Models;
using System.Collections.Generic;
using HealthAxis.Api.Database;
using System.Linq;
using System.Web.Http;

namespace HealthAxis.Api.Controllers
{
    [RoutePrefix("api/appointment")]
    public class AppointmentController : ApiController
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public AppointmentController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet, Route("doctor/{doctorId}")]
        public IHttpActionResult GetByDoctor(int doctorId)
        {
            if (!_context.Doctors.Any(d => d.DoctorId == doctorId))
                return NotFound();

            var appointments = _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .ToList();

            var result = _mapper.Map<List<AppointmentDto>>(appointments);

            foreach (var item in result)
            {
                item.CanConfirm = item.Status == AppointmentStatus.Pending;
                item.CanComplete = item.Status == AppointmentStatus.Confirmed;
                item.CanCancel = item.Status == AppointmentStatus.Pending ||
                                 item.Status == AppointmentStatus.Confirmed;

                item.CanAddHealthRecord =
                    item.Status == AppointmentStatus.Completed &&
                    !_context.HealthRecords.Any(h => h.AppointmentId == item.AppointmentId);
            }

            return Ok(result);
        }

        [HttpGet, Route("patient/{patientId}")]
        public IHttpActionResult GetByPatient(int patientId)
        {
            if (!_context.Patients.Any(p => p.PatientId == patientId))
                return NotFound();

            var appointments = _context.Appointments
                .Where(a => a.PatientId == patientId)
                .ToList();

            var result = _mapper.Map<List<AppointmentDto>>(appointments);

            return Ok(result);
        }

        [HttpGet, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var appointment = _context.Appointments.Find(id);

            if (appointment == null)
                return NotFound();

            return Ok(_mapper.Map<AppointmentDto>(appointment));
        }

        [HttpPost, Route("book")]
        public IHttpActionResult Book(BookAppointmentDto dto)
        {
            if (dto.ScheduledDate < System.DateTime.Today)
            {
                return Ok(new ApiResponseDto
                {
                    Success = false,
                    Message = "Past date not allowed"
                });
            }

            var patient = _context.Patients.Find(dto.PatientId);
            if (patient == null || !patient.IsActive)
            {
                return Ok(new ApiResponseDto
                {
                    Success = false,
                    Message = "Invalid or inactive patient"
                });
            }

            bool exists = _context.Appointments.Any(a =>
                a.PatientId == dto.PatientId &&
                a.DoctorId == dto.DoctorId &&
                a.ScheduledDate == dto.ScheduledDate);

            if (exists)
            {
                return Ok(new ApiResponseDto
                {
                    Success = false,
                    Message = "Already booked with this doctor on the same day"
                });
            }

            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ScheduledDate = dto.ScheduledDate,
                TimeSlot = dto.TimeSlot,
                Status = AppointmentStatus.Pending.ToString()
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return Ok(new ApiResponseDto
            {
                Success = true,
                Message = "Appointment booked successfully"
            });
        }

        [HttpPut, Route("{id}/status")]
        public IHttpActionResult UpdateStatus(int id, UpdateAppointmentStatusDto dto)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment == null)
                return NotFound();

            var currentStatus = (AppointmentStatus)
                System.Enum.Parse(typeof(AppointmentStatus), appointment.Status);

            if (dto.Status == AppointmentStatus.Confirmed && currentStatus != AppointmentStatus.Pending)
                return BadRequest("Only pending appointments can be confirmed");

            if (dto.Status == AppointmentStatus.Completed && currentStatus != AppointmentStatus.Confirmed)
                return BadRequest("Only confirmed appointments can be completed");

            appointment.Status = dto.Status.ToString();
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut, Route("{id}/cancel")]
        public IHttpActionResult Cancel(int id, CancelAppointmentDto dto)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment == null)
                return NotFound();

            var currentStatus = (AppointmentStatus)
                System.Enum.Parse(typeof(AppointmentStatus), appointment.Status);

            if (currentStatus != AppointmentStatus.Pending &&
                currentStatus != AppointmentStatus.Confirmed)
            {
                return BadRequest("Only pending or confirmed appointments can be cancelled");
            }

            appointment.Status = AppointmentStatus.Cancelled.ToString();
            appointment.CancellationReason = dto.CancellationReason;

            _context.SaveChanges();

            return Ok();
        }
    }
}
