using AutoMapper;
using HealthCare.Shared;
using HealthCare.Shared.DTOs.Appointment;
using HealthCareApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthCareApi.Controllers
{
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
        // POST: api/appointments
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
                return BadRequest(ex.Message);
            }
        }

        // 2. GET PATIENT APPOINTMENTS (Paginated + Filter)
        // GET: api/appointments/patient/5?status=Pending&pageNumber=1&pageSize=10
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

            // map ONLY Items
            var dtos = _mapper.Map<IEnumerable<AppointmentDto>>(result.Items);

            // return paged result
            return Ok(new PagedResult<AppointmentDto>
            {
                Items = dtos.ToList(),
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            });
        }


        // 3. DOCTOR TODAY APPOINTMENTS
       
        [HttpGet]
        [Route("doctor/{doctorId:int}/today")]
        public async Task<IHttpActionResult> GetTodayAppointments(int doctorId)
        {
            var appointments = await _appointmentService
                .GetTodayAppointmentsAsync(doctorId);

            var dtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

            return Ok(dtos);
        }

        // 4. DOCTOR WEEK APPOINTMENTS
      
        [HttpGet]
        [Route("doctor/{doctorId:int}/week")]
        public async Task<IHttpActionResult> GetWeeklyAppointments(int doctorId)
        {
            var appointments = await _appointmentService
                .GetWeeklyAppointmentsAsync(doctorId);

            var dtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

            return Ok(dtos);
        }

        // 5. GET APPOINTMENTS BY DATE
        // GET: api/appointments/date?date=2026-06-15
        [HttpGet]
        [Route("date")]
        public async Task<IHttpActionResult> GetByDate(DateTime date)
        {
            var appointments = await _appointmentService
                .GetAppointmentsByDateAsync(date);

            var dtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

            return Ok(dtos);
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

        //cancel

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
                return BadRequest(ex.Message);
            }
        }

        //Available slots


        [HttpGet]
        [Route("doctor/{doctorId:int}/available-slots")]
        public async Task<IHttpActionResult> GetAvailableSlots(int doctorId, DateTime date)
        {
            var slots = await _appointmentService.GetAvailableSlotsAsync(doctorId, date);
            return Ok(slots);
        }


    }
}