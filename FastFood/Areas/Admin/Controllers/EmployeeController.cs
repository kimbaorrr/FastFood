using FastFood.DB;
using FastFood.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Mvc;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{
    public class EmployeeController : SessionController
    {
        /// <summary>
        /// Hiển thị trang bảng chấm công của nhân viên.
        /// </summary>
        /// <returns>Trang TimeSheet.</returns>
        [HttpGet]
        public ActionResult TimeSheet()
        {
            return View();
        }
        /// <summary>
        /// Hiển thị trang phân quyền cho nhân viên.
        /// </summary>
        /// <returns>Trang phân quyền.</returns>
        [HttpGet]
        public ActionResult Permission()
        {
            if (!FastFood_NhanVienDangNhap.CheckRole(Session["MaNhanVien"] as string))
                return HttpNotFound();

            FastFood_NhanVienDangNhap_TaoTaiKhoan a = new FastFood_NhanVienDangNhap_TaoTaiKhoan();
            return View(a);
        }
        /// <summary>
        /// Hiển thị danh sách nhân viên với phân trang.
        /// </summary>
        /// <param name="page">Số trang hiện tại.</param>
        /// <param name="size">Số lượng nhân viên trên mỗi trang.</param>
        /// <returns>Danh sách nhân viên với phân trang.</returns>
        [HttpGet]
        public ActionResult List(int page = 1, int size = 10)
        {
            ViewBag.Title = "Danh sách nhân viên";
            X.PagedList.IPagedList<NhanVien> nhanVien = FastFood_NhanVien.getNhanVien().OrderBy(m => m.MaNhanVien).ToPagedList(page, size);
            ViewBag.NhanVien = nhanVien;
            ViewBag.CurrentPage = nhanVien.PageNumber;
            ViewBag.TotalPages = nhanVien.PageCount;
            return View();
        }
        /// <summary>
        /// Lấy danh sách quyền hạn của nhân viên.
        /// </summary>
        /// <returns>Danh sách quyền hạn dưới dạng JSON.</returns>
        [HttpGet]
        public ActionResult GetListPermission()
        {
            return Json(FastFood_NhanVienDangNhap.getQuyenHanNhanVien().AsEnumerable().Select(x => new { x.MaQuyenHan, x.MoTa }), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Lấy danh sách nhân viên chưa có thông tin đăng nhập.
        /// </summary>
        /// <returns>Danh sách nhân viên chưa có thông tin đăng nhập dưới dạng JSON.</return
        [HttpGet]
        public ActionResult GetNonInfo()
        {
            var dsNhanVienNonInfo = FastFood_NhanVien.getNhanVien()
                .Where(x => x.NhanVienDangNhap == null)
                .Select(x => new { MaNhanVien = x.MaNhanVien, HoTen = x.HoDem + " " + x.TenNhanVien });
            return Json(dsNhanVienNonInfo, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Tạo tài khoản đăng nhập cho nhân viên.
        /// </summary>
        /// <param name="a">Thông tin tài khoản nhân viên.</param>
        /// <param name="permissions">Danh sách quyền hạn của nhân viên.</param>
        /// <returns>Thông báo kết quả việc tạo tài khoản.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FastFood_NhanVienDangNhap_TaoTaiKhoan a, string permissions)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                if (!e.NhanViens.Any(x => x.MaNhanVien == a.MaNhanVien))
                    return HttpNotFound();
                IQueryable<NhanVienDangNhap> dsNhanVien = FastFood_NhanVienDangNhap.getNhanVienDangNhap();
                if (dsNhanVien.Any(x => x.MaNhanVien == a.MaNhanVien))
                    return JsonMessage(false, "Nhân viên này đã tồn tại thông tin đăng nhập !");
                if (dsNhanVien.Any(x => x.TenDangNhap.Equals(a.TenDangNhap)))
                    return JsonMessage(false, "Tên đăng nhập đã tồn tại !");
                if (a.MatKhau.Length < 8)
                    return JsonMessage(false, "Mật khẩu tối thiểu 8 kí tự !");
                string quyenHan = null;
                if (!string.IsNullOrEmpty(permissions))
                {
                    string[] permissionArray = JsonConvert.DeserializeObject<string[]>(permissions);
                    quyenHan = string.Join(",", permissionArray);
                }
                NhanVienDangNhap dnm = new NhanVienDangNhap()
                {
                    MaNhanVien = a.MaNhanVien,
                    TenDangNhap = a.TenDangNhap,
                    MatKhau = FastFood_Tools.HashPassword(a.MatKhau),
                    NgayTao = DateTime.Now,
                    MatKhauTamThoi = true,
                    QuyenHan = quyenHan,
                    VaiTro = a.VaiTro
                };
                e.NhanVienDangNhaps.Add(dnm);
                e.SaveChanges();
            }
            return JsonMessage(true, "Tạo tài khoản thành công !");
        }
        /// <summary>
        /// Lấy danh sách nhân viên đã có tài khoản đăng nhập.
        /// </summary>
        /// <returns>Danh sách nhân viên dưới dạng JSON.</returns>
        [HttpGet]
        public ActionResult GetList()
        {
            var dsNhanVien = FastFood_NhanVien.getNhanVien().AsEnumerable()
                .Where(x => x.NhanVienDangNhap != null)
                .OrderByDescending(x => x.NhanVienDangNhap.NgayTao)
                .Select(x => new
                {
                    x.MaNhanVien,
                    HoTen = x.HoDem + " " + x.TenNhanVien,
                    x.NhanVienDangNhap.TenDangNhap,
                    NgayTao = x.NgayTao.ToString("dd/MM/yyyy HH:mm:ss"),
                    QuyenHan = x.NhanVienDangNhap.QuyenHan ?? string.Empty,
                    VaiTro = x.NhanVienDangNhap.VaiTro ? "Quản trị viên" : "Người dùng"
                });
            return Json(dsNhanVien, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Xóa tài khoản đăng nhập của nhân viên.
        /// </summary>
        /// <param name="employeeId">Mã nhân viên cần xóa.</param>
        /// <returns>Thông báo kết quả xóa nhân viên.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int employeeId)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                NhanVienDangNhap dn = e.NhanVienDangNhaps.FirstOrDefault(x => x.MaNhanVien == employeeId);
                if (dn == null)
                    return HttpNotFound();
                if (Session["MaNhanVien"].Equals(employeeId.ToString()))
                    return JsonMessage(false, $"Bạn đang đăng nhập bằng tài khoản nhân viên {employeeId}. Không thể xóa !");
                e.NhanVienDangNhaps.Remove(dn);
                e.SaveChanges();
                return JsonMessage(true, $"Đã xóa nhân viên {employeeId} khỏi hệ thống!");
            }
        }
        /// <summary>
        /// Chỉnh sửa thông tin tài khoản nhân viên.
        /// </summary>
        /// <param name="employeeId">Mã nhân viên cần chỉnh sửa.</param>
        /// <param name="a">Thông tin tài khoản nhân viên.</param>
        /// <param name="role">Vai trò của nhân viên.</param>
        /// <param name="permissions">Danh sách quyền hạn của nhân viên.</param>
        /// <returns>Thông báo kết quả chỉnh sửa tài khoản.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int employeeId, FastFood_NhanVienDangNhap a, bool role, string permissions)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                NhanVienDangNhap dn = e.NhanVienDangNhaps.FirstOrDefault(x => x.MaNhanVien == employeeId);
                string hashedPassword = FastFood_Tools.HashPassword(a.MatKhau);
                if (dn == null)
                    return HttpNotFound();
                if (!dn.TenDangNhap.Equals(a.TenDangNhap))
                    dn.TenDangNhap = a.TenDangNhap;
                if (!FastFood_Tools.CheckPassword(a.MatKhau, dn.MatKhau))
                    dn.MatKhau = hashedPassword;
                string quyenHan = null;
                if (!string.IsNullOrEmpty(permissions))
                {
                    string[] permissionArray = JsonConvert.DeserializeObject<string[]>(permissions);
                    quyenHan = string.Join(",", permissionArray);
                }
                dn.QuyenHan = quyenHan;
                dn.VaiTro = role;
                e.SaveChanges();
                return JsonMessage(true, $"Đã thay đổi thông tin tài khoản nhân viên {employeeId} !");
            }
        }


    }
}