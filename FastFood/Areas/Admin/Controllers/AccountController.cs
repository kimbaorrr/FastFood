using FastFood.DB;
using FastFood.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace FastFood.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Hiển thị trang đăng nhập.
        /// </summary>
        /// <returns>Trang đăng nhập.</returns>
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Title = "Đăng nhập hệ thống";
            ViewBag.Description = $"Ngày làm việc: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
            return View();
        }
        /// <summary>
        /// Xử lý yêu cầu đăng nhập.
        /// </summary>
        /// <param name="a">Thông tin đăng nhập của nhân viên.</param>
        /// <returns>Trả về thông báo JSON hoặc chuyển hướng đến trang chính nếu đăng nhập thành công.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FastFood_NhanVienDangNhap a)
        {
            ViewBag.Title = "Đăng nhập hệ thống";
            ViewBag.Description = $"Ngày làm việc: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

            if (ModelState.IsValid)
            {
                NhanVienDangNhap queryData = FastFood_NhanVienDangNhap.getNhanVienDangNhap().FirstOrDefault(nv => nv.TenDangNhap.Equals(a.TenDangNhap));
                bool isValid = queryData != null && FastFood_Tools.CheckPassword(a.MatKhau, queryData.MatKhau);

                if (!isValid)
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng !";
                }
                else
                {
                    FastFood_NhanVien b = new FastFood_NhanVien
                    {
                        MaNhanVien = queryData.MaNhanVien,
                        AnhDD = queryData.NhanVien.AnhDD ?? string.Empty
                    };

                    Session["MaNhanVien"] = b.MaNhanVien.ToString();
                    Session["AnhDD"] = b.AnhDD.ToString();

                    if (a.GhiNhoDangNhap)
                    {
                        HttpCookie cookie = new HttpCookie("LoginCookie")
                        {
                            Values = {
                            ["MaNhanVien"] = queryData.MaNhanVien.ToString(),
                            ["AnhDD"] = b.AnhDD.ToString()
                        },
                            Expires = DateTime.Now.AddDays(14),
                            Secure = true
                        };
                        Response.Cookies.Add(cookie);
                    }

                    FastFood_NhanVien.lichSuTruyCap(b.MaNhanVien, queryData.TenDangNhap, "Đăng nhập");
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }

            return View();
        }
        /// <summary>
        /// Đăng xuất khỏi hệ thống.
        /// </summary>
        /// <returns>Chuyển hướng về trang đăng nhập.</returns>
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            HttpCookie loginCookie = Request.Cookies["LoginCookie"];
            if (loginCookie != null)
            {
                loginCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(loginCookie);
            }
            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }
        /// <summary>
        /// Đổi mật khẩu cho tài khoản.
        /// </summary>
        /// <param name="a">Thông tin mật khẩu cũ và mật khẩu mới của nhân viên.</param>
        /// <returns>Trả về thông báo JSON.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("doi-mat-khau")]
        public ActionResult ChangePassword(FastFood_NhanVienDangNhap_DoiMatKhau a)
        {
            if (!ModelState.IsValid)
                return JsonMessage(false, "Thông tin không hợp lệ!");

            using (FastFoodEntities e = new FastFoodEntities())
            {
                string matKhauMoi = FastFood_Tools.HashPassword(a.MatKhauMoi);
                int maNhanVien = Convert.ToInt32(Session["MaNhanVien"]);

                if (a.MatKhauMoi.Length < 8 || a.MatKhauCu.Length < 8)
                    return JsonMessage(false, "Mật khẩu quá ngắn, tối thiểu từ 8 kí tự trở lên !");

                if (!a.MatKhauMoi.Equals(a.NhapLaiMatKhau))
                    return JsonMessage(false, "Mật khẩu mới và nhập lại không khớp nhau !");

                NhanVienDangNhap chg = e.NhanVienDangNhaps.FirstOrDefault(x => x.MaNhanVien == maNhanVien);
                if (chg == null || !FastFood_Tools.CheckPassword(a.MatKhauCu, chg.MatKhau))
                    return JsonMessage(false, "Mật khẩu cũ không đúng !");

                if (a.MatKhauCu.Equals(a.MatKhauMoi))
                    return JsonMessage(false, "Mật khẩu cũ và mật khẩu mới không được trùng nhau !");

                chg.MatKhau = matKhauMoi;
                chg.MatKhauTamThoi = false;
                e.SaveChanges();
                FastFood_NhanVien.lichSuTruyCap(maNhanVien, Session["TenDangNhap"] as string, "Đổi mật khẩu");
            }

            return JsonMessage(true, "Đổi mật khẩu thành công !");
        }
        /// <summary>
        /// Hiển thị trang quên mật khẩu.
        /// </summary>
        /// <returns>Trang khôi phục mật khẩu.</returns>
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            ViewBag.Title = "Khôi phục mật khẩu";
            ViewBag.Description = "Nhập email để lấy lại mật khẩu !";
            return View();
        }
        /// <summary>
        /// Xử lý yêu cầu lấy lại mật khẩu.
        /// </summary>
        /// <param name="a">Thông tin tài khoản cần khôi phục.</param>
        /// <returns>Trả về thông báo JSON hoặc chuyển hướng đến trang đăng nhập nếu thành công.</returns>
        [HttpPost]
        [Route("quen-mat-khau")]
        public ActionResult ForgetPassword(FastFood_NhanVienDangNhap_QuenMatKhau a)
        {
            ViewBag.Title = "Khôi phục mật khẩu";
            ViewBag.Description = "Nhập email để lấy lại mật khẩu !";

            using (FastFoodEntities e = new FastFoodEntities())
            {
                NhanVienDangNhap dn = e.NhanVienDangNhaps.FirstOrDefault(x => x.TenDangNhap.Equals(a.TenDangNhap) && x.NhanVien.Email.Equals(a.Email));
                if (dn == null)
                    ViewBag.ThongBao = "Sai tên đăng nhập hoặc email !";
                else
                {
                    string tempPassword = Guid.NewGuid().ToString().Replace("-", "");
                    string hashTempPassword = FastFood_Tools.HashPassword(tempPassword);
                    dn.MatKhau = hashTempPassword;
                    dn.MatKhauTamThoi = true;
                    e.SaveChanges();

                    SendResetPasswordEmail(a.Email, tempPassword);
                    return RedirectToAction("Login", "Account", new { area = "Admin" });
                }
                return View();

            }
        }
        /// <summary>
        /// Gửi email chứa mật khẩu tạm thời.
        /// </summary>
        /// <param name="email">Địa chỉ email của người nhận.</param>
        /// <param name="tempPassword">Mật khẩu tạm thời.</param>
        private void SendResetPasswordEmail(string email, string tempPassword)
        {
            using (MailMessage mail = new MailMessage())
            {
                string mailId = ConfigurationManager.AppSettings["mail_Id"];
                string mailPwd = ConfigurationManager.AppSettings["mail_Pwd"];

                mail.To.Add(email);
                mail.From = new MailAddress(mailId);
                mail.Subject = "Reset password";
                mail.Body = $"Your temp password is: {tempPassword}";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(mailId, mailPwd),
                    EnableSsl = true
                };

                smtp.Send(mail);
            }
        }
        /// <summary>
        /// Kiểm tra xem mật khẩu hiện tại có phải là mật khẩu tạm thời không.
        /// </summary>
        /// <returns>JSON xác định mật khẩu có phải tạm thời hay không.</returns>
        [HttpPost]
        public ActionResult CheckTempPassword()
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                bool isTempPassword = e.NhanVienDangNhaps.Select(x => x.MatKhauTamThoi).FirstOrDefault();
                return Json(new { is_temp_pwd = isTempPassword });
            }
        }

        /// <summary>
        /// Thông báo dạng JSON
        /// </summary>
        /// <param name="success">Trạng thái thực thi</param>
        /// <param name="message">Nội dung thông báo</param>
        /// <returns></returns>
        public JsonResult JsonMessage(bool success, string message)
        {
            return success
                ? Json(new { success = success, type = "var(--bs-success)", message = message }, JsonRequestBehavior.AllowGet)
                : Json(new { success = success, type = "var(--bs-danger)", message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}
