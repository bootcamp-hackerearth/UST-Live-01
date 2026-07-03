using HealthAxisApplicn.Dto.HealthRecords;
using HealthAxisApplicn.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisApplicn.Controllers
{
    [Route("api/healthrecords")]
    [ApiController]
    [Authorize]
    public class HealthRecordController(IHealthRecordService service) : ControllerBase
    {
        // Patient → own records
        [HttpGet("my")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyRecords()
        {
            var patientId = int.Parse(User.FindFirst("PatientId")!.Value);

            var result = await service.GetRecordsByPatientIdAsync(patientId);

            return Ok(result);
        }

        //Doctor → their patients
        [HttpGet("doctor/my")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorRecords()
        {
            var doctorId = int.Parse(User.FindFirst("DoctorId")!.Value);

            var result = await service.GetRecordsByDoctorIdAsync(doctorId);

            return Ok(result);
        }

        // Get specific record (safe)
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await service.GetByIdAsync(id);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        // Doctor fills diagnosis/prescription
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Update(int id, UpdateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await service.UpdateAsync(id, dto);

            if (result is null)
                return NotFound();

            return Ok(result);
        }


        [HttpGet("appointment/{appointmentId:int}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetByAppointment(int appointmentId)
        {
            var result = await service.GetByAppointmentIdAsync(appointmentId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("doctor/patients")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorPatients()
        {
            var doctorId = int.Parse(
                User.FindFirst("DoctorId")!.Value
            );

            var result =
                await service.GetDoctorPatientsAsync(doctorId);

            return Ok(result);
        }

    }
}
