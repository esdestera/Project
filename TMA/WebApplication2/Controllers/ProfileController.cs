using System.Web.Mvc;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

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

        public ActionResult ViewAccounts()
        {
            return View();
        }

        public ActionResult NewAccount()
        {
            return View();
        }
        
    }
}