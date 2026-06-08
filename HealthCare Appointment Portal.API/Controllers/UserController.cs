using System.Threading.Tasks;
using System.Web.Http;
using HealthCare_Appointment_Portal.Interfaces;

namespace HealthCare_Appointment_Portal.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(
            IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult>
            GetById(int id)
        {
            var user =
                await _userService
                    .GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        [Route("code/{userCode}")]
        public async Task<IHttpActionResult>
            GetByUserCode(string userCode)
        {
            var user =
                await _userService
                    .GetByUserCodeAsync(userCode);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        [Route("email/{email}")]
        public async Task<IHttpActionResult>
            GetByEmail(string email)
        {
            var user =
                await _userService
                    .GetByEmailAsync(email);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}