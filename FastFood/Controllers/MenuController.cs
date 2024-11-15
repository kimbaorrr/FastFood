
using FastFood.DB;
using FastFood.Models;
using System;
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
            FastFood_SanPham_DanhGiaSanPham dgsp = new FastFood_SanPham_DanhGiaSanPham()
            {
                TenKhachHang = FastFood.Areas.Admin.Models.FastFood_NhanVien.getHoTen(Session["MaKhachHang"] as string),
                MaKhachHang = Convert.ToInt32(Session["MaKhachHang"] as string)
            };
            ViewBag.Title = "Thông tin món ăn";
            ViewBag.SanPham = sp;
            ViewBag.ReturnUrl = return_url;
            return View(dgsp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detail(int productId, FastFood_SanPham_DanhGiaSanPham a)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                DanhGiaSanPham dg = new DanhGiaSanPham()
                {
                    MaKhachHang = a.MaKhachHang,
                    DanhGia = a.NoiDung,
                    MaSanPham = productId,
                    NgayTao = DateTime.Now,
                    XepHangSao = a.XepHangSao
                };
                e.DanhGiaSanPhams.Add(dg);
                e.SaveChanges();
                return Json(new { });
            }
        }
    }
}