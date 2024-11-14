using System.ComponentModel.DataAnnotations;

namespace FastFood.Areas.Admin.Models
{
    public class FastFood_KhachHangDangNhap
    {

    }
    public class FastFood_KhachHangDangNhap_DangKiMoi
    {
        [Display(Name = "Họ đệm")]
        [DataType(DataType.Text)]
        public string HoDem { get; set; }
        [Display(Name = "Tên khách hàng")]
        [DataType(DataType.Text)]
        public string TenKhachHang { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.Text)]
        public string Email { get; set; }
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.Text)]
        public string SoDienThoai { get; set; }
        [Display(Name = "Tên đăng nhập")]
        [DataType(DataType.Text)]
        public string TenDangNhap { get; set; }
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Text)]
        public string MatKhau { get; set; }
        public FastFood_KhachHangDangNhap_DangKiMoi()
        {
            HoDem = string.Empty;
            TenDangNhap = string.Empty;
            Email = string.Empty;
            SoDienThoai = string.Empty;
            MatKhau = string.Empty;
            TenKhachHang = string.Empty;
        }
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