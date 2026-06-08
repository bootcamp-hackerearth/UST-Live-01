using HealthCare_Appointment_Portal.DTOs.UserDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiService
            _userService;

        public UserController(
            IUserApiService userService)
        {
            _userService =
                userService;
        }

        // ==========================
        // LOGIN
        // ==========================

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserId"] != null)
            {
                return RedirectUserByRole();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(
            LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var user =
                await _userService
                    .GetUserByCodeAsync(
                        dto.UserCode);

            if (user == null)
            {
                ModelState.AddModelError(
                    "",
                    "Invalid User Code.");

                return View(dto);
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError(
                    "",
                    "User account is inactive.");

                return View(dto);
            }

            Session["UserId"] =
                user.UserId;

            Session["UserCode"] =
                user.UserCode;

            Session["ReferenceId"] =
                user.ReferenceId;

            Session["Role"] =
                user.Role;

            return RedirectUserByRole();
        }

        // ==========================
        // REGISTER
        // ==========================

        [HttpGet]
        public ActionResult Register()
        {
            return RedirectToAction(
                "Create",
                "Patient");
        }

        // ==========================
        // LOGOUT
        // ==========================

        public ActionResult Logout()
        {
            Session.Clear();

            Session.Abandon();

            return RedirectToAction(
                "Login");
        }

        // ==========================
        // PRIVATE METHODS
        // ==========================

        private ActionResult RedirectUserByRole()
        {
            Role role =
                (Role)Session["Role"];

            switch (role)
            {
                case Role.Admin:

                    return RedirectToAction(
                        "Dashboard",
                        "Admin");

                case Role.Doctor:

                    return RedirectToAction(
                        "Dashboard",
                        "Doctor");

                case Role.Patient:

                    return RedirectToAction(
                        "Dashboard",
                        "Patient");

                default:

                    Session.Clear();

                    return RedirectToAction(
                        "Login");
            }
        }
    }
}