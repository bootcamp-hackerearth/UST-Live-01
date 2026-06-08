using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.DTOs.UserDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal_MVC.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiService _userService;
        private readonly IPatientApiService _patientService;

        public UserController(
            IUserApiService userService,
            IPatientApiService patientService)
        {
            _userService = userService;
            _patientService = patientService;
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

            return View(new LoginDto());
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

            try
            {
                var user =
                    await _userService
                        .GetUserByCodeAsync(
                            dto.UserCode);

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
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(dto);
            }
        }

        // ==========================
        // REGISTER
        // ==========================

        [HttpGet]
        public ActionResult Register()
        {
            return View(
                new CreatePatientDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(
            CreatePatientDto patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            try
            {
                await _patientService
                    .CreatePatientAsync(
                        patient);

                TempData["SuccessMessage"] =
                    "Registration successful. Please login using your User Code.";

                return RedirectToAction(
                    "Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(patient);
            }
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
            if (Session["Role"] == null)
            {
                Session.Clear();

                return RedirectToAction(
                    "Login");
            }

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