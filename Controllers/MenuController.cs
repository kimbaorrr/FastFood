using FastFood.DB;
using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Areas.Controllers
{
    [Route("thuc-don")]
    public class MenuController : SessionController
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.Title = "Thực đơn";
            return View();
        }
        [HttpGet("chi-tiet-mon-an/{id}")]
        public IActionResult Detail(int id)
        {
            Sanpham? sp = FastFood_SanPham.getSanPhamDaDuyet().FirstOrDefault(x => x.Masanpham == id);

            if (sp == null)
                return BadRequest();

            ViewBag.Title = "Thông tin món ăn";
            ViewBag.SanPham = sp;
            ViewBag.ReturnUrl = GetAbsoluteUri();

            return View();

        }

        [HttpPost("chi-tiet-mon-an/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Detail(int id, [FromForm] FastFood_SanPham_DanhGiaSanPham a)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                Danhgiasanpham dg = new Danhgiasanpham()
                {
                    Makhachhang = a.MaKhachHang,
                    Danhgia = a.NoiDung,
                    Masanpham = id,
                    Ngaytao = DateTime.Now,
                    Xephangsao = a.XepHangSao
                };
                e.Danhgiasanphams.Add(dg);
                e.SaveChanges();
                return View();
            }

        }
    }
}