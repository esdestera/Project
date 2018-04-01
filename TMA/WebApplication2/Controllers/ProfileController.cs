using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class ProfileController : Controller
    {
        public ActionResult MyProfile()
        {
            return View();
        }

        public ActionResult MyAccounts()
        {
            return View();
        }
    }
}