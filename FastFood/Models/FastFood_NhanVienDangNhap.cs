using FastFood.DB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FastFood.Models
{
    /// <summary>
    /// Lớp này đại diện cho thông tin đăng nhập của nhân viên.
    /// </summary>
    public class FastFood_NhanVienDangNhap
    {
        /// <summary>
        /// Tên đăng nhập của nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Trường này không được để trống !")]
        [MaxLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 kí tự !")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu của nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Trường này không được để trống !")]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = string.Empty;

        /// <summary>
        /// Cờ để ghi nhớ đăng nhập.
        /// </summary>
        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool GhiNhoDangNhap { get; set; } = false;

        /// <summary>
        /// Khởi tạo đối tượng FastFood_NhanVienDangNhap với các giá trị mặc định.
        /// </summary>
        public FastFood_NhanVienDangNhap()
        {

        }

        /// <summary>
        /// Khởi tạo đối tượng FastFood_NhanVienDangNhap từ một đối tượng khác.
        /// </summary>
        /// <param name="a">Đối tượng FastFood_NhanVienDangNhap để sao chép thông tin.</param>
        public FastFood_NhanVienDangNhap(FastFood_NhanVienDangNhap a)
        {
            TenDangNhap = a.TenDangNhap.Trim().Normalize().ToLower();
            MatKhau = a.MatKhau;
            GhiNhoDangNhap = a.GhiNhoDangNhap;
        }

        private static FastFoodEntities context => new FastFoodEntities();

        private static IQueryable<NhanVienDangNhap> nhanVienDangNhaps => context.NhanVienDangNhaps;

        private static IQueryable<QuyenHanNhanVien> quyenHanNhanViens => context.QuyenHanNhanViens;

        /// <summary>
        /// Lấy danh sách nhân viên đăng nhập.
        /// </summary>
        /// <returns>Danh sách nhân viên đăng nhập.</returns>
        public static IQueryable<NhanVienDangNhap> getNhanVienDangNhap()
        {
            return nhanVienDangNhaps;
        }

        /// <summary>
        /// Lấy danh sách quyền hạn của nhân viên.
        /// </summary>
        /// <returns>Danh sách quyền hạn nhân viên.</returns>
        public static IQueryable<QuyenHanNhanVien> getQuyenHanNhanVien()
        {
            return quyenHanNhanViens;
        }

        /// <summary>
        /// Lấy mô tả quyền hạn từ các ID quyền hạn.
        /// </summary>
        /// <param name="ids">Danh sách ID quyền hạn.</param>
        /// <returns>Danh sách mô tả quyền hạn tương ứng.</returns>
        public static IEnumerable<string> getMoTaQuyenHan(int[] ids)
        {
            return getQuyenHanNhanVien().Where(qh => ids.Contains(qh.MaQuyenHan)).Select(qh => qh.MoTa);
        }

        /// <summary>
        /// Kiểm tra quyền hạn của nhân viên.
        /// </summary>
        /// <param name="maNV">Mã nhân viên.</param>
        /// <param name="maQuyenHan">Mã quyền hạn.</param>
        /// <returns>Trả về true nếu nhân viên có quyền hạn, ngược lại false.</returns>
        public static bool CheckPermission(string maNV, int maQuyenHan)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                string quyenHan = e.NhanVienDangNhaps.Where(x => x.MaNhanVien.ToString().Equals(maNV)).Select(x => x.QuyenHan).FirstOrDefault();
                if (quyenHan == null)
                    return false;
                bool hasPermission = quyenHan.Split(',').Contains(maQuyenHan.ToString());
                return hasPermission;
            }
        }

        /// <summary>
        /// Kiểm tra vai trò của nhân viên.
        /// </summary>
        /// <param name="maNV">Mã nhân viên.</param>
        /// <returns>Trả về vai trò của nhân viên.</returns>
        public static bool CheckRole(string maNV)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                return e.NhanVienDangNhaps.Select(x => x.VaiTro).FirstOrDefault();
            }
        }
    }

    /// <summary>
    /// Lớp này đại diện cho thông tin đổi mật khẩu của nhân viên.
    /// </summary>
    public class FastFood_NhanVienDangNhap_DoiMatKhau
    {
        /// <summary>
        /// Mật khẩu cũ của nhân viên.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Mật khẩu cũ")]
        public string MatKhauCu { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu mới của nhân viên.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Mật khẩu mới")]
        public string MatKhauMoi { get; set; } = string.Empty;

        /// <summary>
        /// Nhập lại mật khẩu mới của nhân viên.
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nhập lại mật khẩu")]
        public string NhapLaiMatKhau { get; set; } = string.Empty;

        /// <summary>
        /// Khởi tạo đối tượng FastFood_NhanVienDangNhap_DoiMatKhau với các giá trị mặc định.
        /// </summary>
        public FastFood_NhanVienDangNhap_DoiMatKhau()
        {

        }

        /// <summary>
        /// Khởi tạo đối tượng FastFood_NhanVienDangNhap_DoiMatKhau từ một đối tượng khác.
        /// </summary>
        /// <param name="a">Đối tượng FastFood_NhanVienDangNhap_DoiMatKhau để sao chép thông tin.</param>
        public FastFood_NhanVienDangNhap_DoiMatKhau(FastFood_NhanVienDangNhap_DoiMatKhau a)
        {
            MatKhauCu = a.MatKhauCu;
            MatKhauMoi = a.MatKhauMoi;
            NhapLaiMatKhau = a.NhapLaiMatKhau;
        }
    }

    /// <summary>
    /// Lớp này đại diện cho thông tin quên mật khẩu của nhân viên.
    /// </summary>
    public class FastFood_NhanVienDangNhap_QuenMatKhau
    {
        /// <summary>
        /// Email của nhân viên.
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Tên đăng nhập của nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; } = string.Empty;

        /// <summary>
        /// Khởi tạo đối tượng FastFood_NhanVienDangNhap_QuenMatKhau với các giá trị mặc định.
        /// </summary>
        public FastFood_NhanVienDangNhap_QuenMatKhau()
        {

        }

        /// <summary>
        /// Khởi tạo đối tượng FastFood_NhanVienDangNhap_QuenMatKhau từ một đối tượng khác.
        /// </summary>
        /// <param name="a">Đối tượng FastFood_NhanVienDangNhap_QuenMatKhau để sao chép thông tin.</param>
        public FastFood_NhanVienDangNhap_QuenMatKhau(FastFood_NhanVienDangNhap_QuenMatKhau a)
        {
            Email = a.Email;
            TenDangNhap = a.TenDangNhap;
        }
    }

    /// <summary>
    /// Lớp này đại diện cho thông tin tạo tài khoản nhân viên.
    /// </summary>
    public class FastFood_NhanVienDangNhap_TaoTaiKhoan
    {
        /// <summary>
        /// Mã nhân viên.
        /// </summary>
        public int MaNhanVien { get; set; } = 0;

        /// <summary>
        /// Tên đăng nhập của nhân viên.
        /// </summary>
        [Display(Name = "Tên đăng nhập")]
        [DataType(DataType.Text)]
        public string TenDangNhap { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu của nhân viên.
        /// </summary>
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; } = string.Empty;

        /// <summary>
        /// Cờ xác nhận mật khẩu.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không giống nhau !")]
        public string XacNhanMatKhau { get; set; } = string.Empty;

        /// <summary>
        /// Email của nhân viên.
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Vai trò nhân viên
        /// </summary>
        [Display(Name = "Vai trò")]
        public bool VaiTro { get; set; } = false;

        /// <summary>
        /// Khởi tạo đối tượng FastFood_NhanVienDangNhap_TaoTaiKhoan với các giá trị mặc định.
        /// </summary>
        public FastFood_NhanVienDangNhap_TaoTaiKhoan()
        {

        }

        /// <summary>
        /// Khởi tạo đối tượng FastFood_NhanVienDangNhap_TaoTaiKhoan từ một đối tượng khác.
        /// </summary>
        /// <param name="a">Đối tượng FastFood_NhanVienDangNhap_TaoTaiKhoan để sao chép thông tin.</param>
        public FastFood_NhanVienDangNhap_TaoTaiKhoan(FastFood_NhanVienDangNhap_TaoTaiKhoan a)
        {
            TenDangNhap = a.TenDangNhap;
            MatKhau = a.MatKhau;
            XacNhanMatKhau = a.XacNhanMatKhau;
            Email = a.Email;
            VaiTro = a.VaiTro;
        }
    }
}
