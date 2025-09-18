using FastFood.DB;
using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FastFood.Areas.Controllers
{
    [Route("khach-hang")]
    public class AccountController : Controller
    {
        private ISession Session => HttpContext.Session;
        private IResponseCookies Cookie => HttpContext.Response.Cookies;

        [HttpPost("dang-nhap-he-thong")]
        [ValidateAntiForgeryToken]
        public string Login(FastFood_NhanVienDangNhap a)
        {
            Khachhangdangnhap? queryData = FastFood_KhachHang.getKhachHangDangNhap().FirstOrDefault(x => x.Tendangnhap.Equals(a.TenDangNhap));

            if (queryData == null || !FastFood_Tools.CheckPassword(a.MatKhau, queryData.Matkhau))
                return JsonMessage(message: "Tên đăng nhập hoặc mật khẩu không đúng !");

            FastFood_KhachHang khachHang = new FastFood_KhachHang
            {
                MaKhachHang = queryData.Makhachhang,
                AnhDD = queryData.MakhachhangNavigation.Anhdd ?? string.Empty
            };

            Session.SetString("CustomerId", khachHang.MaKhachHang.ToString());
            Session.SetString("CustomerAvatar", khachHang.AnhDD.ToString());

            if (a.GhiNhoDangNhap)
            {
                var cookieData = new
                {
                    MaKhachHang = queryData.Makhachhang.ToString(),
                    AnhDD = khachHang.AnhDD.ToString()
                };

                Cookie.Append(
                    "KhachHangLoginCookie",
                    JsonConvert.SerializeObject(cookieData),
                    new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(14),
                        Secure = true
                    });
            }

            FastFood_NhanVien.lichSuTruyCap(khachHang.MaKhachHang.ToString(), queryData.Tendangnhap, "Đăng nhập");

            return JsonMessage(true, message: "Đăng nhập thành công!");
        }

        [HttpGet("dang-xuat")]
        public IActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("dang-ki")]
        [ValidateAntiForgeryToken]
        public string Register(FastFood_KhachHangDangNhap_DangKiMoi a)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                Khachhangdangnhap? existingUser = e.Khachhangdangnhaps.FirstOrDefault(x => x.Tendangnhap.Equals(a.TenDangNhap));
                if (existingUser != null)
                    return JsonMessage(message: "Tên đăng nhập đã tồn tại!");

                Khachhang khachHang = new Khachhang
                {
                    Hodem = a.HoDem,
                    Tenkhachhang = a.TenKhachHang,
                    Sodienthoai = a.SoDienThoai,
                    Email = a.Email,
                    Ngaytao = DateTime.Now
                };
                e.Khachhangs.Add(khachHang);
                e.SaveChanges();

                Khachhangdangnhap khachHangDangNhap = new Khachhangdangnhap
                {
                    Makhachhang = khachHang.Makhachhang,
                    Tendangnhap = a.TenDangNhap,
                    Matkhau = FastFood_Tools.HashPassword(a.MatKhau),
                    Ngaytao = DateTime.Now
                };
                e.Khachhangdangnhaps.Add(khachHangDangNhap);
                e.SaveChanges();

                return JsonMessage(true, "Đăng ký thành công!");
            }
        }
        /// <summary>
        /// Thông báo dạng JSON
        /// </summary>
        /// <param name="success">Trạng thái thực thi</param>
        /// <param name="message">Nội dung thông báo</param>
        /// <returns></returns>
        private string JsonMessage(bool success = false, string message = "", object data = null)
        {
            dynamic jsonMessage = new
            {
                success,
                type = success ? "var(--bs-success)" : "var(--bs-danger)",
                message = message ?? string.Empty,
                data = data ?? string.Empty
            };
            string jsonOutput = JsonConvert.SerializeObject(jsonMessage, Formatting.Indented);

            return jsonOutput;

        }

    }
}