using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class FastFood_ThanhToan
    {
    }

    public class FastFood_ThanhToan_TomTatThanhToan
    {
        public int TongTienSanPham { get; set; } = 0;
        public int PhiVanChuyen { get; set; } = 20000;
        public int MaKhuyenMai { get; set; } = 0;
        public int IDMaKhuyenMai { get; set; } = 0;
        public int TongThanhToan => TongTienSanPham + PhiVanChuyen - MaKhuyenMai;

        public FastFood_ThanhToan_TomTatThanhToan() { }

        public FastFood_ThanhToan_TomTatThanhToan(FastFood_ThanhToan_TomTatThanhToan a)
        {
            TongTienSanPham = a.TongTienSanPham;
            PhiVanChuyen = a.PhiVanChuyen;
            MaKhuyenMai = a.MaKhuyenMai;
            IDMaKhuyenMai = a.IDMaKhuyenMai;
        }
    }

    public class FastFood_ThanhToan_ThemDot : FastFood_ThanhToan_TomTatThanhToan
    {
        [Display(Name = "Họ đệm")]
        [DataType(DataType.Text)]
        public string HoDem { get; set; } = string.Empty;

        [Display(Name = "Tên khách hàng")]
        [DataType(DataType.Text)]
        public string TenKhachHang { get; set; } = string.Empty;

        public string HoTenKhachHang => $"{HoDem} {TenKhachHang}";

        [Display(Name = "Địa chỉ giao hàng")]
        [DataType(DataType.Text)]
        public string DiaChiGiaoHang { get; set; } = string.Empty;

        [Display(Name = "Thành phố")]
        [DataType(DataType.Text)]
        public string ThanhPho { get; set; } = string.Empty;

        [Display(Name = "Mã bưu điện")]
        [DataType(DataType.Text)]
        public string MaBuuDien { get; set; } = string.Empty;

        [Display(Name = "Email")]
        [DataType(DataType.Text)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.Text)]
        public string SoDienThoai { get; set; } = string.Empty;

        [Display(Name = "Ghi chú đơn hàng")]
        [DataType(DataType.MultilineText)]
        public string GhiChuDonHang { get; set; } = string.Empty;

        [Display(Name = "Phương thức thanh toán")]
        [DataType(DataType.Text)]
        public int MaNguoiMua { get; set; } = 0;

        public FastFood_ThanhToan_ThemDot() : base() { }

        public FastFood_ThanhToan_ThemDot(FastFood_ThanhToan_ThemDot a) : base(a)
        {
            HoDem = a.HoDem;
            TenKhachHang = a.TenKhachHang;
            DiaChiGiaoHang = a.DiaChiGiaoHang;
            ThanhPho = a.ThanhPho;
            MaBuuDien = a.MaBuuDien;
            Email = a.Email;
            SoDienThoai = a.SoDienThoai;
            GhiChuDonHang = a.GhiChuDonHang;
            MaNguoiMua = a.MaNguoiMua;
        }
    }

    public class FastFood_ThanhToan_KetQua
    {
        [Display(Name = "Mã đơn hàng")]
        [DataType(DataType.Text)]
        public int MaDonHang { get; set; } = 0;

        [Display(Name = "Tổng thanh toán")]
        [DataType(DataType.Text)]
        public int TongThanhToan { get; set; } = 0;

        [Display(Name = "Mã giao dịch")]
        [DataType(DataType.Text)]
        public long MaGiaoDich { get; set; } = 0;

        [Display(Name = "Trạng thái thanh toán")]
        [DataType(DataType.Text)]
        public string TrangThaiThanhToan { get; set; } = string.Empty;

        [Display(Name = "Trạng thái giao dịch")]
        [DataType(DataType.Text)]
        public string TrangThaiGiaoDich { get; set; } = string.Empty;
    }
}
