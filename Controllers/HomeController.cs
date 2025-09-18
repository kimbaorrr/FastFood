using Microsoft.AspNetCore.Mvc;

namespace FastFood.Areas.Controllers
{
    public class HomeController : SessionController
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "Trang chủ";
            return View();
        }
    }
}