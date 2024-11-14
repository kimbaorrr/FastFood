using System.Web.Mvc;

namespace FastFood.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Detail()
        {
            ViewBag.Title = "Chi tiết sản phẩm";
            return View();
        }
    }
}