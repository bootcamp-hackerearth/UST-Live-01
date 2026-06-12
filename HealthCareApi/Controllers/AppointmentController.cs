using AutoMapper;
using HealthCare.Shared.DTOs.Appointment;
using HealthCare.Shared;
using HealthCareApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HealthCareApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/appointments")]
    public class AppointmentController : ApiController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;

        public AppointmentController(
            IAppointmentService appointmentService,
            IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        // 1. BOOK APPOINTMENT
       
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Book(Appointment appointment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _appointmentService
                    .BookAppointmentAsync(appointment);

                var dto = _mapper.Map<AppointmentDto>(created);

                return Created($"api/appointments/{dto.AppointmentId}", dto);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        // GET PATIENT APPOINTMENTS (Paginated + Filter)
        
        [HttpGet]
        [Route("patient/{patientId:int}")]
        public async Task<IHttpActionResult> GetPatientAppointments(
            int patientId,
            string status = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var result = await _appointmentService
                .GetPatientAppointmentsAsync(patientId, status, pageNumber, pageSize);

            
            var dtos = _mapper.Map<IEnumerable<AppointmentDto>>(result.Items);

            
            return Ok(new PagedResult<AppointmentDto>
            {
                Items = dtos.ToList(),
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            });
        }

        [HttpGet]
        [Route("doctor/{doctorId:int}")]
        public async Task<IHttpActionResult> GetDoctorAppointments(
            int doctorId,
            string status = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var result = await _appointmentService
                    .GetDoctorAppointmentsAsync(doctorId, status, pageNumber, pageSize);

                return Ok(new PagedResult<AppointmentDto>
                {
                    Items = _mapper.Map<List<AppointmentDto>>(result.Items),
                    TotalCount = result.TotalCount,
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize
                });
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // GET APPOINTMENTS BY DATE
        
        [HttpGet]
        [Route("date")]
        public async Task<IHttpActionResult> GetByDate(DateTime date)
        {
            var appointments = await _appointmentService
                .GetAppointmentsByDateAsync(date);

            var dtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

            return Ok(dtos);
        }

        // GET AVAILABLE TIME SLOTS
        
        [HttpGet]
        [Route("slots")]
        public async Task<IHttpActionResult> GetAvailableSlots(int doctorId, DateTime date)
        {
            try
            {
                var slots = await _appointmentService
                    .GetAvailableSlotsAsync(doctorId, date);

                return Ok(slots); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}/confirm")]
        public async Task<IHttpActionResult> Confirm(int id)
        {
            try
            {
                var appointment = await _appointmentService
                    .ConfirmAppointmentAsync(id);

                var dto = _mapper.Map<AppointmentDto>(appointment);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}/cancel")]
        public async Task<IHttpActionResult> Cancel(int id, CancelAppointmentDto dto)
        {
            try
            {
                var appointment = await _appointmentService
                    .CancelAppointmentAsync(id, dto.Reason);

                var result = _mapper.Map<AppointmentDto>(appointment);

                return Ok(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        //  GET UPCOMING APPOINTMENTS (Paginated)
        
        [HttpGet]
        [Route("upcoming")]
        public async Task<IHttpActionResult> GetUpcomingAppointments(
            int? patientId = null,
            int? doctorId = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var result = await _appointmentService
                    .GetUpcomingAppointmentsAsync(patientId, doctorId, pageNumber, pageSize);

                var dtos = _mapper.Map<IEnumerable<AppointmentDto>>(result.Items);

                return Ok(new PagedResult<AppointmentDto>
                {
                    Items = dtos.ToList(),
                    TotalCount = result.TotalCount,
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}