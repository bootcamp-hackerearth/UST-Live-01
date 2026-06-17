using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [ExcludeFromCodeCoverage]
    public class DoctorsController :
        ControllerBase
    {
        #region Fields

        private readonly IDoctorService
            _doctorService;

        #endregion

        #region Constructor

        public DoctorsController(
            IDoctorService doctorService)
        {
            _doctorService =
                doctorService;
        }

        #endregion

        #region Endpoints

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult>
            GetDoctors(
                [FromQuery]
                PaginationParams pagination,
                [FromQuery]
                string? specialisation)
        {
            if (!string.IsNullOrWhiteSpace(
                specialisation))
            {
                var filteredDoctors =
                    await _doctorService
                        .GetBySpecialisationAsync(
                            specialisation);

                return Ok(filteredDoctors);
            }

            var doctors =
                await _doctorService
                    .GetPagedAsync(
                        pagination);

            return Ok(doctors);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult>
            GetDoctor(
                int id)
        {
            var doctor =
                await _doctorService
                    .GetByIdAsync(id);

            return Ok(doctor);
        }

        [HttpGet("{id:int}/availability")]
        [AllowAnonymous]
        public async Task<IActionResult>
            GetAvailability(
                int id)
        {
            var availability =
                await _doctorService
                    .GetAvailabilityAsync(id);

            return Ok(availability);
        }

        #endregion
    }
}
