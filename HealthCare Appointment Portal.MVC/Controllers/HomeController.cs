using System.Web.Mvc;

namespace HealthCare_Appointment_Portal_MVC.Controllers
{
    public class HomeController
        : Controller
    {
        public ActionResult
            Index()
        {
            //return RedirectToAction(
            //    "Login",
            //    "User");
            return View();
        }
    }
}