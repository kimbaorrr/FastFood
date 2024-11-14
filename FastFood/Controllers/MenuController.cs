using FastFood.Areas.Admin.Models;
using FastFood.DB;
using System.Linq;
using System.Web.Mvc;

namespace FastFood.Controllers
{
    public class MenuController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Thực đơn";
            return View();
        }
        [HttpGet]
        public ActionResult Detail(int id, string return_url)
        {
            SanPham sp = FastFood_SanPham.getSanPhamDaDuyet().FirstOrDefault(x => x.MaSanPham == id);
            if (sp == null)
                return HttpNotFound();
            ViewBag.Title = "Thông tin món ăn";
            ViewBag.SanPham = sp;
            ViewBag.ReturnUrl = return_url;
            return View();
        }
    }
}