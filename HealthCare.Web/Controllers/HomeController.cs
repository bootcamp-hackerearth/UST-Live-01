using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HealthCare.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("List", "Patient");

            if (User.IsInRole("Doctor"))
                return RedirectToAction("Profile", "Doctor");

            if (User.IsInRole("Patient"))
                return RedirectToAction("Book", "Appointment");

            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult LoginAs(string role)
        {
            string email;
            string fullName;
            int userId;

            switch (role)
            {
                case "Admin":
                    email = "admin@hospital.com";
                    fullName = "Admin User";
                    userId = 1;
                    break;

                case "Doctor":
                    email = "doctor@hospital.com";
                    fullName = "Dr. Smith";
                    userId = 2;
                    break;

                case "Patient":
                    email = "patient@hospital.com";
                    fullName = "John Doe";
                    userId = 3;
                    break;

                default:
                    return View("NotFound"); ;
            }

            string userData = role + "|" + fullName + "|" + userId;

            var ticket = new FormsAuthenticationTicket(
                1,
                email,
                DateTime.Now,
                DateTime.Now.AddHours(8),
                false,
                userData
            );

            string encrypted = FormsAuthentication.Encrypt(ticket);

            Response.Cookies.Add(new HttpCookie(
                FormsAuthentication.FormsCookieName, encrypted
            ));

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}