using System.Web.Mvc;

namespace FastFood.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Trang chủ";
            return View();
        }
    }
}