using FastFood.DB;
using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/quan-ly-nhan-vien")]
    public class EmployeeController : SessionController
    {
        /// <summary>
        /// Hiển thị trang bảng chấm công của nhân viên.
        /// </summary>
        /// <returns>Trang TimeSheet.</returns>
        [HttpGet("cham-cong")]
        public IActionResult TimeSheet()
        {
            return View();
        }
        /// <summary>
        /// Hiển thị trang phân quyền cho nhân viên.
        /// </summary>
        /// <returns>Trang phân quyền.</returns>
        [HttpGet("quyen-han")]
        public IActionResult Permission()
        {
            if (!FastFood_NhanVienDangNhap.CheckRole(this.MaNhanVien))
                return BadRequest();

            FastFood_NhanVienDangNhap_TaoTaiKhoan a = new FastFood_NhanVienDangNhap_TaoTaiKhoan();
            return View(a);
        }
        /// <summary>
        /// Hiển thị danh sách nhân viên với phân trang.
        /// </summary>
        /// <param name="page">Số trang hiện tại.</param>
        /// <param name="size">Số lượng nhân viên trên mỗi trang.</param>
        /// <returns>Danh sách nhân viên với phân trang.</returns>
        [HttpGet("danh-sach-quyen-han-nhan-vien")]
        public IActionResult List([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            ViewBag.Title = "Danh sách nhân viên";
            IPagedList<Nhanvien> nhanVien = FastFood_NhanVien.getNhanVien().Include(x => x.Nhanviendangnhap).OrderBy(m => m.Manhanvien).ToPagedList(page, size);
            ViewBag.NhanVien = nhanVien;
            ViewBag.CurrentPage = nhanVien.PageNumber;
            ViewBag.TotalPages = nhanVien.PageCount;
            return View();

        }
        /// <summary>
        /// Lấy danh sách quyền hạn của nhân viên.
        /// </summary>
        /// <returns>Danh sách quyền hạn dưới dạng JSON.</returns>
        [HttpGet("danh-sach-quyen-han")]
        public string GetListPermission()
        {
            return JsonConvert.SerializeObject(FastFood_NhanVienDangNhap.getQuyenHanNhanVien()
                .Select(x => new { MaQuyenHan = x.Maquyenhan, MoTa = x.Mota }));
        }
        /// <summary>
        /// Lấy danh sách nhân viên chưa có thông tin đăng nhập.
        /// </summary>
        /// <returns>Danh sách nhân viên chưa có thông tin đăng nhập dưới dạng JSON.</return
        [HttpGet("chua-co-thong-tin")]
        public string GetNonInfo()
        {
            var dsNhanVienNonInfo = FastFood_NhanVien.getNhanVien().Include(x => x.Nhanviendangnhap)
                .Where(x => x.Nhanviendangnhap == null)
                .Select(x => new { MaNhanVien = x.Manhanvien, HoTen = x.Hodem + " " + x.Tennhanvien });

            return JsonConvert.SerializeObject(dsNhanVienNonInfo);
        }
        /// <summary>
        /// Tạo tài khoản đăng nhập cho nhân viên.
        /// </summary>
        /// <param name="a">Thông tin tài khoản nhân viên.</param>
        /// <param name="permissions">Danh sách quyền hạn của nhân viên.</param>
        /// <returns>Thông báo kết quả việc tạo tài khoản.</returns>
        [HttpPost("them-thong-tin-dang-nhap")]
        [ValidateAntiForgeryToken]
        public string Create([FromForm] FastFood_NhanVienDangNhap_TaoTaiKhoan a, [FromForm] string permissions)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                if (!e.Nhanviens.Any(x => x.Manhanvien == a.MaNhanVien))
                    return JsonMessage(message: "ID không hợp lệ !");

                if (e.Nhanviendangnhaps.Any(x => x.Manhanvien == a.MaNhanVien))
                    return JsonMessage(message: "Nhân viên này đã có thông tin đăng nhập !");

                if (e.Nhanviendangnhaps.Any(x => x.Tendangnhap.Equals(a.TenDangNhap)))
                    return JsonMessage(message: "Tên đăng nhập đã tồn tại !");

                if (a.MatKhau.Length < 8)
                    return JsonMessage(message: "Mật khẩu tối thiểu 8 kí tự !");

                string quyenHan = string.Empty;

                if (!string.IsNullOrEmpty(permissions))
                {
                    string[] permissionArray = JsonConvert.DeserializeObject<string[]>(permissions)!;
                    quyenHan = string.Join(",", permissionArray);
                }
                Nhanviendangnhap dnm = new Nhanviendangnhap()
                {
                    Manhanvien = a.MaNhanVien,
                    Tendangnhap = a.TenDangNhap,
                    Matkhau = FastFood_Tools.HashPassword(a.MatKhau),
                    Ngaytao = DateTime.Now,
                    Matkhautamthoi = true,
                    Quyenhan = quyenHan,
                    Vaitro = a.VaiTro
                };

                e.Nhanviendangnhaps.Add(dnm);
                e.SaveChanges();

            }
            return JsonMessage(true, "Tạo tài khoản thành công !");

        }
        /// <summary>
        /// Lấy danh sách nhân viên đã có tài khoản đăng nhập.
        /// </summary>
        /// <returns>Danh sách nhân viên dưới dạng JSON.</returns>
        [HttpGet("da-co-thong-tin")]
        public string GetList()
        {
            var dsNhanVien = FastFood_NhanVien.getNhanVien().Include(x => x.Nhanviendangnhap)
                .Where(x => x.Nhanviendangnhap != null)
                .OrderByDescending(x => x.Nhanviendangnhap!.Ngaytao)
                .Select(x => new
                {
                    MaNhanVien = x.Manhanvien,
                    HoTen = x.Hodem + " " + x.Tennhanvien,
                    TenDangNhap = x.Nhanviendangnhap!.Tendangnhap,
                    NgayTao = x.Ngaytao.ToString("dd/MM/yyyy HH:mm:ss"),
                    QuyenHan = FastFood_NhanVienDangNhap.ListQuyenHan(x.Nhanviendangnhap.Quyenhan!),
                    VaiTro = x.Nhanviendangnhap.Vaitro ? "Quản trị viên" : "Người dùng"
                });
            return JsonConvert.SerializeObject(dsNhanVien);
        }
        /// <summary>
        /// Xóa tài khoản đăng nhập của nhân viên.
        /// </summary>
        /// <param name="employeeId">Mã nhân viên cần xóa.</param>
        /// <returns>Thông báo kết quả xóa nhân viên.</returns>
        [HttpPost("xoa-tai-khoan")]
        [ValidateAntiForgeryToken]
        public string Delete([FromForm] int employeeId)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                Nhanviendangnhap? dn = e.Nhanviendangnhaps.FirstOrDefault(x => x.Manhanvien == employeeId);

                if (dn == null)
                    return JsonMessage(message: "ID không hợp lệ !");

                if (this.MaNhanVien.Equals(employeeId.ToString()))
                    return JsonMessage(message: $"Bạn đang đăng nhập bằng tài khoản nhân viên {employeeId} này. Không thể xóa !");

                e.Nhanviendangnhaps.Remove(dn);
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
        [HttpPost("sua-tai-khoan")]
        [ValidateAntiForgeryToken]
        public string Edit([FromForm] string employeeId, [FromForm] FastFood_NhanVienDangNhap a, [FromForm] bool role, [FromForm] string permissions)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                Nhanviendangnhap? dn = e.Nhanviendangnhaps.FirstOrDefault(x => x.Manhanvien.ToString().Equals(employeeId));

                if (dn == null)
                    return JsonMessage(message: "ID không hợp lệ !");

                if (!string.IsNullOrEmpty(a.MatKhau) && FastFood_Tools.CheckPassword(a.MatKhau, dn.Matkhau))
                    dn.Matkhau = FastFood_Tools.HashPassword(a.MatKhau);

                if (!dn.Tendangnhap.Equals(a.TenDangNhap))
                    dn.Tendangnhap = a.TenDangNhap;

                string quyenHan = string.Empty;

                if (!string.IsNullOrEmpty(permissions))
                {
                    string[] permissionArray = JsonConvert.DeserializeObject<string[]>(permissions)!;
                    quyenHan = string.Join(",", permissionArray);
                }

                dn.Quyenhan = quyenHan;
                dn.Vaitro = role;

                e.SaveChanges();

                return JsonMessage(true, $"Đã thay đổi thông tin tài khoản nhân viên {employeeId} !");
            }
        }


    }
}