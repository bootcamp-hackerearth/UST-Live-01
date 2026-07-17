using HealthAxis.API.Data;
using HealthAxis.API.DTOs.DoctorDtos;
using HealthAxis.API.Services;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HealthAxis.API.Controller
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private const string InvalidToken = "Invalid token";

        private readonly ApplicationDbContext _context;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;

        public DoctorController(
            ApplicationDbContext context,
            IDoctorService doctorService,
            IPatientService patientService)
        {
            _context = context;
            _doctorService = doctorService;
            _patientService = patientService;
        }

        [HttpGet("me")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetMyDoctorProfile()
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var doctor = await _doctorService.GetByUserIdAsync(userId);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message =
                        "Doctor profile not found. Please link this doctor account with the doctor table."
                });
            }

            return Ok(doctor);
        }

        [HttpPut("me/status")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateMyStatus(
            [FromBody] UpdateDoctorStatusDto dto)
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(item =>
                    item.UserId == userId);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message =
                        "Doctor profile not found. Please link this doctor account with the doctor table."
                });
            }

            doctor.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = dto.IsActive
                    ? "You are now active. Patients can book appointments."
                    : "You are now inactive. Patients cannot book new appointments.",
                isActive = doctor.IsActive
            });
        }

        [HttpGet("me/patients")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetMyPatients()
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var doctor = await _doctorService.GetByUserIdAsync(userId);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = "Doctor profile not found"
                });
            }

            var patients =
                await _patientService.GetPatientsForDoctorAsync(
                    doctor.DoctorId);

            return Ok(patients);
        }

        [HttpGet("me/patients/{patientId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetMyPatientById(
            int patientId)
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var doctor = await _doctorService.GetByUserIdAsync(userId);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = "Doctor profile not found"
                });
            }

            var patient =
                await _patientService.GetPatientForDoctorAsync(
                    doctor.DoctorId,
                    patientId);

            if (patient == null)
            {
                return StatusCode(403, new
                {
                    message =
                        "You are not allowed to access this patient's details"
                });
            }

            return Ok(patient);
        }

        [HttpGet]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllAsync();
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            if (User.IsInRole("Doctor"))
            {
                var userId = GetLoggedInUserId();

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Unauthorized(new
                    {
                        message = InvalidToken
                    });
                }

                var loggedInDoctor =
                    await _doctorService.GetByUserIdAsync(userId);

                if (loggedInDoctor == null)
                {
                    return NotFound(new
                    {
                        message = "Doctor profile not found"
                    });
                }

                if (loggedInDoctor.DoctorId != id)
                {
                    return StatusCode(403, new
                    {
                        message =
                            "You are not allowed to access another doctor's details"
                    });
                }

                return Ok(loggedInDoctor);
            }

            var doctor = await _doctorService.GetByIdAsync(id);
            return Ok(doctor);
        }

        [HttpGet("{id}/availability")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> GetDoctorAvailability(
            int id,
            [FromQuery] DateTime? date)
        {
            var availability =
                await _doctorService.GetAvailabilityAsync(
                    id,
                    date);

            return Ok(availability);
        }

        private string? GetLoggedInUserId()
        {
            return User.FindFirstValue(
                       ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")
                ?? User.FindFirstValue("nameid");
        }
    }
}