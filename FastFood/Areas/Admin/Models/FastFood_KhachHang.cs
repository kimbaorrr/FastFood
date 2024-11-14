using FastFood.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastFood.Areas.Admin.Models
{
    /// <summary>
    /// Lớp này đại diện cho khách hàng trong hệ thống cửa hàng thức ăn nhanh, 
    /// cung cấp các phương thức để truy xuất thông tin khách hàng, thống kê, và quản lý lịch sử truy cập.
    /// </summary>
    public class FastFood_KhachHang
    {
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public int MaKhachHang { get; set; }

        /// <summary>
        /// Đường dẫn đến ảnh đại diện của khách hàng
        /// </summary>
        public string AnhDD { get; set; }

        /// <summary>
        /// Context kết nối đến cơ sở dữ liệu.
        /// </summary>
        private static FastFoodEntities context => new FastFoodEntities();

        /// <summary>
        /// Lấy danh sách tất cả khách hàng.
        /// </summary>
        private static IQueryable<KhachHang> khachHangs => context.KhachHangs;

        /// <summary>
        /// Lấy danh sách tất cả thông tin đăng nhập của khách hàng.
        /// </summary>
        private static IQueryable<KhachHangDangNhap> khachHangDangNhaps => context.KhachHangDangNhaps;

        /// <summary>
        /// Lấy danh sách tất cả lịch sử truy cập của khách hàng.
        /// </summary>
        private static IQueryable<LichSuTruyCap> lichSuTruyCaps => context.LichSuTruyCaps;

        /// <summary>
        /// Lấy danh sách khách hàng.
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        public static IQueryable<KhachHang> getKhachHang()
        {
            return khachHangs;
        }

        /// <summary>
        /// Lấy tổng số khách hàng trong hệ thống.
        /// </summary>
        /// <returns>Số lượng khách hàng</returns>
        public static int tongKhachHang()
        {
            using (FastFoodEntities e = new FastFoodEntities())
                return e.KhachHangs.Count();
        }

        /// <summary>
        /// Lấy họ tên của khách hàng dựa trên mã khách hàng.
        /// </summary>
        /// <param name="maKH">Mã khách hàng</param>
        /// <returns>Họ tên của khách hàng</returns>
        public static string getHoTen(int maKH)
        {
            string hoTen = getKhachHang()
                .Where(x => x.MaKhachHang == maKH)
                .Select(m => new { HoTen = m.HoDem + " " + m.TenKhachHang })
                .Select(x => x.HoTen)
                .FirstOrDefault();
            return hoTen ?? string.Empty;
        }

        /// <summary>
        /// Lấy danh sách thông tin đăng nhập của khách hàng.
        /// </summary>
        /// <returns>Danh sách thông tin đăng nhập</returns>
        public static IQueryable<KhachHangDangNhap> getKhachHangDangNhap()
        {
            return khachHangDangNhaps;
        }

        /// <summary>
        /// So sánh tổng số khách hàng trong tháng này và tháng trước.
        /// </summary>
        /// <returns>Phần trăm thay đổi số khách hàng</returns>
        public static double soSanhTongKhachHang()
        {
            IEnumerable<KhachHang> dsKhachHang = getKhachHang().AsEnumerable();
            DateTime ngayHienTai = DateTime.Now;
            DateTime dauThangNay = new DateTime(ngayHienTai.Year, ngayHienTai.Month, 1);
            DateTime dauThangTruoc = dauThangNay.AddMonths(-1);
            DateTime cuoiThangTruoc = dauThangNay.AddDays(-1);

            int tongThangNay = dsKhachHang.Count(kh => kh.NgayTao >= dauThangNay && kh.NgayTao < dauThangNay.AddMonths(1));
            int tongThangTruoc = dsKhachHang.Count(kh => kh.NgayTao >= dauThangTruoc && kh.NgayTao <= cuoiThangTruoc);

            if (tongThangTruoc == 0)
            {
                return tongThangNay > 0 ? 100 : 0;
            }

            int phanTramThayDoi = (tongThangNay - tongThangTruoc) / tongThangTruoc * 100;

            return phanTramThayDoi;
        }

        /// <summary>
        /// Lấy danh sách khách hàng tiềm năng dựa trên các đơn hàng và lịch sử truy cập.
        /// </summary>
        /// <returns>Danh sách khách hàng tiềm năng</returns>
        public static IEnumerable<FastFood_KhachHang_DanhSachKhachHang> getKhachHangTiemNang()
        {
            IEnumerable<KhachHang> dsKhachHang = getKhachHang().Where(x => x.DonHangs.Any(a => a.TrangThaiDon == 7)).AsEnumerable();
            IEnumerable<LichSuTruyCap> lsTruyCap = getLichSuTruyCap().AsEnumerable();
            IEnumerable<FastFood_KhachHang_DanhSachKhachHang> danhSachKhachHang = dsKhachHang.Select(kh => new FastFood_KhachHang_DanhSachKhachHang
            {
                MaKhachHang = kh.MaKhachHang,
                HoTenKhachHang = kh.HoDem + " " + kh.TenKhachHang,
                AnhDD = kh.AnhDD,
                NgayTao = kh.NgayTao,
                ThoiGianGanBo = (DateTime.Now - kh.NgayTao).Days,
                NgaySinh = kh.NgaySinh,
                SoLanMua = kh.DonHangs.Count(),
                TongHoaDon = kh.DonHangs.Sum(x => x.TongTienSanPham),
                HoaDonLonNhat = kh.DonHangs.Max(x => x.TongTienSanPham),
                LanMuaGanNhat = kh.DonHangs.OrderByDescending(dh => dh.NgayDat).Select(x => x.NgayDat).FirstOrDefault(),
                LanTruyCapCuoi = lsTruyCap.Where(x => x.MaNguoiDung == kh.MaKhachHang).OrderByDescending(x => x.ThoiGianTruyCap).Select(x => x.ThoiGianTruyCap).FirstOrDefault()
            });

            return danhSachKhachHang;
        }

        /// <summary>
        /// Lấy lịch sử truy cập của khách hàng.
        /// </summary>
        /// <returns>Danh sách lịch sử truy cập</returns>
        public static IQueryable<LichSuTruyCap> getLichSuTruyCap()
        {
            return lichSuTruyCaps.Where(x => x.LoaiNguoiDung == true);
        }
    }


    public class FastFood_KhachHang_DanhSachKhachHang
    {
        public int MaKhachHang { get; set; }
        public string HoTenKhachHang { get; set; }
        public string AnhDD { get; set; }
        public int SoLanMua { get; set; }
        public int TongHoaDon { get; set; }
        public int? HoaDonLonNhat { get; set; }
        public DateTime? LanMuaGanNhat { get; set; }
        public DateTime NgayTao { get; set; }
        public int ThoiGianGanBo { get; set; }
        public DateTime? NgaySinh { get; set; }
        public DateTime? LanTruyCapCuoi { get; set; }

    }

    public class FastFood_KhachHang_ChiTiet
    {
        public int MaKhachHang { get; set; }
        public string HoTenKhachHang { get; set; }
        public string NgaySinh { get; set; }
        public string AnhDD { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public DateTime? NgayTao { get; set; }
        public int TongChiTieu { get; set; }
        public int TongHoaDon { get; set; }
        public int? ChiTieuLonNhat { get; set; }
        public int HoatDongGanDay { get; set; }
        public IEnumerable<DonHang> DonHangs { get; set; }

        public FastFood_KhachHang_ChiTiet()
        {
            MaKhachHang = -1;
            HoTenKhachHang = string.Empty;
            DiaChi = string.Empty;
            Email = string.Empty;
            SoDienThoai = string.Empty;
            AnhDD = string.Empty;
            TongChiTieu = 0;
            TongHoaDon = 0;
            NgaySinh = string.Empty;
            NgayTao = null;
            ChiTieuLonNhat = 0;
            HoatDongGanDay = 0;
            DonHangs = Enumerable.Empty<DonHang>();
        }

        public FastFood_KhachHang_ChiTiet(FastFood_KhachHang_ChiTiet a)
        {
            MaKhachHang = a.MaKhachHang;
            HoTenKhachHang = a.HoTenKhachHang;
            NgaySinh = a.NgaySinh;
            NgayTao = a.NgayTao;
            AnhDD = a.AnhDD;
            TongChiTieu = a.TongChiTieu;
            TongHoaDon = a.TongHoaDon;
            DiaChi = a.DiaChi;
            Email = a.Email;
            SoDienThoai = a.SoDienThoai;
            DonHangs = a.DonHangs;
            ChiTieuLonNhat = a.ChiTieuLonNhat;
            HoatDongGanDay = a.HoatDongGanDay;
        }

    }
}