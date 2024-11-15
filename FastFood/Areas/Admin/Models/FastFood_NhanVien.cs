using FastFood.DB;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FastFood.Areas.Admin.Models
{
    /// <summary>
    /// Lớp FastFood_NhanVien đại diện cho thông tin của nhân viên trong hệ thống quản lý thức ăn nhanh.
    /// </summary>
    public class FastFood_NhanVien
    {
        /// <summary>
        /// Mã nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Mã nhân viên")]
        public int MaNhanVien { get; set; } = 0;

        /// <summary>
        /// Họ và đệm của nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Họ đệm")]
        public string HoDem { get; set; } = string.Empty;

        /// <summary>
        /// Tên nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Tên nhân viên")]
        public string TenNhanVien { get; set; } = string.Empty;

        /// <summary>
        /// Địa chỉ email của nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Số điện thoại của nhân viên.
        /// </summary>
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại")]
        [MinLength(10)]
        [MaxLength(12)]
        public string SoDienThoai { get; set; } = string.Empty;

        /// <summary>
        /// Địa chỉ của nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; } = string.Empty;

        /// <summary>
        /// Đường dẫn đến ảnh đại diện của nhân viên.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Ảnh đại diện")]
        public string AnhDD { get; set; } = string.Empty;

        /// <summary>
        /// Khởi tạo một đối tượng FastFood_NhanVien với các giá trị mặc định.
        /// </summary>
        public FastFood_NhanVien()
        {

        }

        /// <summary>
        /// Khởi tạo một đối tượng FastFood_NhanVien sao chép từ đối tượng khác.
        /// </summary>
        /// <param name="a">Đối tượng FastFood_NhanVien cần sao chép.</param>
        public FastFood_NhanVien(FastFood_NhanVien a)
        {
            MaNhanVien = a.MaNhanVien;
            HoDem = a.HoDem;
            TenNhanVien = a.TenNhanVien;
            Email = a.Email;
            SoDienThoai = a.SoDienThoai;
            AnhDD = a.AnhDD;
            DiaChi = a.DiaChi;
        }

        /// <summary>
        /// Lấy đối tượng FastFoodEntities để truy cập vào cơ sở dữ liệu.
        /// </summary>
        private static FastFoodEntities context => new FastFoodEntities();

        /// <summary>
        /// Truy vấn danh sách nhân viên.
        /// </summary>
        private static IQueryable<NhanVien> nhanViens => context.NhanViens;

        /// <summary>
        /// Lấy danh sách tất cả nhân viên.
        /// </summary>
        /// <returns>Danh sách nhân viên.</returns>
        public static IQueryable<NhanVien> getNhanVien()
        {
            return nhanViens;
        }

        /// <summary>
        /// Lấy họ và tên của nhân viên theo mã nhân viên.
        /// </summary>
        /// <param name="manv">Mã nhân viên cần lấy họ và tên.</param>
        /// <returns>Họ và tên của nhân viên.</returns>
        public static string getHoTen(string manv)
        {
            string hoTen = getNhanVien()
                .Where(x => x.MaNhanVien.ToString().Equals(manv))
                .Select(m => new { HoTen = m.HoDem + " " + m.TenNhanVien })
                .Select(x => x.HoTen)
                .FirstOrDefault();
            return hoTen ?? string.Empty;
        }

        /// <summary>
        /// Lưu lại lịch sử truy cập của nhân viên.
        /// </summary>
        /// <param name="maNV">Mã nhân viên.</param>
        /// <param name="tenDN">Tên đăng nhập của nhân viên.</param>
        /// <param name="hanhDong">Hành động của nhân viên (ví dụ: "Đăng nhập", "Xem báo cáo").</param>
        public static void lichSuTruyCap(int maNV, string tenDN, string hanhDong)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                LichSuTruyCap ls = new LichSuTruyCap()
                {
                    MaNguoiDung = maNV,
                    LoaiNguoiDung = false,
                    TenDangNhap = tenDN,
                    ThoiGianTruyCap = DateTime.Now,
                    HoatDong = hanhDong
                };
                e.LichSuTruyCaps.Add(ls);
                e.SaveChanges();
            }
        }
    }
}
