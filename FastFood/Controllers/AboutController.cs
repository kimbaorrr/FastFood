using System.Web.Mvc;

namespace FastFood.Controllers
{
    public class AboutController : SessionController
    {
        // GET: About
        public ActionResult Index()
        {
            ViewBag.Title = "Về chúng tôi";
            return View();
        }
    }
}