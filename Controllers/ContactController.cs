using FastFood.DB;
using FastFood.Models;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Areas.Controllers
{
    [Route("lien-he")]
    public class ContactController : SessionController
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.Title = "Liên hệ FastFood";
            return View();
        }
        [HttpPost("gui-cau-hoi")]
        [ValidateAntiForgeryToken]
        public string Question(FastFood_KhachHang_GuiCauHoi a)
        {
            if (a.NoiDung.Length > 255)
                JsonMessage(message: "Độ dài nội dung phải < 255 kí tự !");
            if (a.Email.Length > 100 || !a.Email.Contains("@"))
                JsonMessage(message: "Email phải bao gồm @ và < 100 kí tự !");
            if (a.TenKhachHang.Length > 100)
                JsonMessage(message: "Độ dài tên phải < 100 kí tự !");

            using (FastFoodEntities e = new FastFoodEntities())
            {
                Cauhoi ch = new Cauhoi()
                {
                    Email = a.Email,
                    Sodienthoai = a.SoDienThoai,
                    Tenkhachhang = a.TenKhachHang,
                    Noidung = a.NoiDung
                };
                e.Cauhois.Add(ch);
                e.SaveChanges();
                return JsonMessage(true, "Câu hỏi của bạn đã được ghi nhận. Xin cám ơn !");
            }
        }

    }
}