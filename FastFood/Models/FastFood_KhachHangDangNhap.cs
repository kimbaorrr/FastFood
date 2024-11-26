using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class FastFood_KhachHangDangNhap_DangKiMoi
    {
        [Display(Name = "Họ đệm")]
        [DataType(DataType.Text)]
        public string HoDem { get; set; } = string.Empty;
        [Display(Name = "Tên khách hàng")]
        [DataType(DataType.Text)]
        public string TenKhachHang { get; set; } = string.Empty;
        [Display(Name = "Email")]
        [DataType(DataType.Text)]
        public string Email { get; set; } = string.Empty;
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.Text)]
        public string SoDienThoai { get; set; } = string.Empty;
        [Display(Name = "Tên đăng nhập")]
        [DataType(DataType.Text)]
        public string TenDangNhap { get; set; } = string.Empty;
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; } = string.Empty;
        public FastFood_KhachHangDangNhap_DangKiMoi() { }
        public FastFood_KhachHangDangNhap_DangKiMoi(FastFood_KhachHangDangNhap_DangKiMoi a)
        {
            TenDangNhap = a.TenDangNhap;
            Email = a.Email;
            SoDienThoai = a.SoDienThoai;
            MatKhau = a.MatKhau;
            TenKhachHang = a.TenKhachHang;
            HoDem = a.HoDem;
        }
    }
}