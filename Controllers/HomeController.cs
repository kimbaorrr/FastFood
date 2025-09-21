using Microsoft.AspNetCore.Mvc;

namespace FastFood.Areas.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Trang chủ";
            return View();
        }
    }
}