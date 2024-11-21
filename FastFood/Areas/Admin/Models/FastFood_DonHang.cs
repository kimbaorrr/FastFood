using FastFood.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace FastFood.Areas.Admin.Models
{
    public class FastFood_DonHang
    {
        private static FastFoodEntities context => new FastFoodEntities();
        private static IQueryable<DonHang> donHangs => context.DonHangs;
        private static IQueryable<TrangThaiDonHang> trangThaiDonHangs => context.TrangThaiDonHangs;

        /// <summary>
        /// Lấy danh sách tất cả các đơn hàng trong hệ thống.
        /// </summary>
        /// <returns>Danh sách các đơn hàng.</returns>
        public static IQueryable<DonHang> getDonHang()
        {
            return donHangs;
        }

        /// <summary>
        /// Lấy danh sách tất cả trạng thái đơn hàng.
        /// </summary>
        /// <returns>Danh sách trạng thái đơn hàng.</returns>
        public static IQueryable<TrangThaiDonHang> getTrangThaiDonHang()
        {
            return trangThaiDonHangs;
        }

        /// <summary>
        /// Lấy danh sách các đơn hàng đã giao (trạng thái đơn hàng = 7).
        /// </summary>
        /// <returns>Danh sách các đơn hàng đã giao.</returns>
        public static IEnumerable<DonHang> getDonHangDaGiao()
        {
            return getDonHang().Where(x => x.TrangThaiDon == 7) ?? Enumerable.Empty<DonHang>();
        }

        /// <summary>
        /// Lấy tên trạng thái đơn hàng theo mã trạng thái.
        /// </summary>
        /// <param name="maTT">Mã trạng thái đơn hàng.</param>
        /// <returns>Tên trạng thái đơn hàng.</returns>
        public static string getTenTrangThai(int maTT)
        {
            return getTrangThaiDonHang().Where(x => x.MaTrangThai == maTT).Select(x => x.TenTrangThai)?.FirstOrDefault() ?? string.Empty;
        }

        /// <summary>
        /// Đếm số lượng đơn hàng đã giao trong tuần hiện tại.
        /// </summary>
        /// <returns>Số lượng đơn hàng đã giao trong tuần.</returns>
        public static int demDonHangTrongTuan()
        {
            DateTime ngayHienTai = DateTime.Today;
            DateTime dauTuan = ngayHienTai.AddDays(-(int)ngayHienTai.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime cuoiTuan = dauTuan.AddDays(7);

            return getDonHangDaGiao()?.Count(x => x.NgayDat >= dauTuan && x.NgayDat < cuoiTuan) ?? 0;
        }

        /// <summary>
        /// Tính doanh thu trong ngày hôm nay từ các đơn hàng đã giao.
        /// </summary>
        /// <returns>Doanh thu hôm nay.</returns>
        public static int doanhThuHomNay()
        {
            return getDonHangDaGiao().Where(x => x.NgayDat == DateTime.Today)?.Sum(x => x.TongTienSanPham) ?? 0;
        }

        /// <summary>
        /// So sánh doanh thu giữa tháng này và tháng trước.
        /// </summary>
        /// <returns>Phần trăm thay đổi doanh thu giữa tháng này và tháng trước.</returns>
        public static double soSanhDoanhThuThang()
        {
            DateTime ngayHienTai = DateTime.Now;
            DateTime dauThangNay = new DateTime(ngayHienTai.Year, ngayHienTai.Month, 1);
            DateTime dauThangTruoc = dauThangNay.AddMonths(-1);
            DateTime cuoiThangTruoc = dauThangNay.AddDays(-1);
            IEnumerable<DonHang> dsDonHang = getDonHangDaGiao();

            decimal doanhThuThangNay = dsDonHang
                .Where(dh => dh.NgayDat >= dauThangNay && dh.NgayDat < dauThangNay.AddMonths(1))?
                .Sum(dh => dh.TongTienSanPham) ?? 0;

            decimal doanhThuThangTruoc = dsDonHang
                .Where(dh => dh.NgayDat >= dauThangTruoc && dh.NgayDat <= cuoiThangTruoc)?
                .Sum(dh => dh.TongTienSanPham) ?? 0;

            if (doanhThuThangTruoc == 0)
            {
                return doanhThuThangNay > 0 ? 100 : 0;
            }

            int phanTramThayDoi = (int)((doanhThuThangNay - doanhThuThangTruoc) / doanhThuThangTruoc) * 100;

            return phanTramThayDoi;
        }

        /// <summary>
        /// So sánh số lượng đơn hàng giữa tuần này và tuần trước.
        /// </summary>
        /// <returns>Phần trăm thay đổi số lượng đơn hàng giữa tuần này và tuần trước.</returns>
        public static double soSanhDonHangTuan()
        {
            IEnumerable<DonHang> dsDonHang = getDonHangDaGiao();
            DateTime ngayHienTai = DateTime.Today;

            DateTime dauTuanNay = ngayHienTai.AddDays(-(int)ngayHienTai.DayOfWeek);
            DateTime cuoiTuanNay = dauTuanNay.AddDays(7);

            DateTime dauTuanTruoc = dauTuanNay.AddDays(-7);
            DateTime cuoiTuanTruoc = dauTuanNay.AddDays(-1);

            int donHangTuanNay = dsDonHang
                .Count(dh => dh.NgayDat >= dauTuanNay && dh.NgayDat < cuoiTuanNay);

            int donHangTuanTruoc = dsDonHang
                .Count(dh => dh.NgayDat >= dauTuanTruoc && dh.NgayDat <= cuoiTuanTruoc);

            if (donHangTuanTruoc == 0)
            {
                return donHangTuanNay > 0 ? 100 : 0;
            }

            int phanTramThayDoi = (donHangTuanNay - donHangTuanTruoc) / donHangTuanTruoc * 100;

            return phanTramThayDoi;
        }

        /// <summary>
        /// Lấy doanh thu theo từng khoảng thời gian.
        /// </summary>
        /// <param name="loaiThoiGian">Loại thời gian để tính doanh thu (30 ngày, 6 tháng, 1 năm).</param>
        /// <returns>Doanh thu dưới dạng chuỗi JSON.</returns>
        public static string getDoanhThu(string loaiThoiGian)
        {
            IEnumerable<DonHang> dsDonHang = getDonHangDaGiao();
            DateTime homNay = DateTime.Today;
            SortedDictionary<DateTime, int> doanhThu = new SortedDictionary<DateTime, int>();

            switch (loaiThoiGian)
            {
                case "30_days":
                    for (int i = 0; i < 30; i++)
                    {
                        DateTime ngay = homNay.AddDays(-i);
                        int doanhThuNgay = dsDonHang
                            .Where(x => x.NgayDat.Date == ngay.Date)?
                            .Sum(x => x.TongTienSanPham) ?? 0;

                        doanhThu[ngay] = doanhThuNgay;
                    }
                    break;

                case "6_months":
                    for (int i = 0; i < 6; i++)
                    {
                        DateTime thang = homNay.AddMonths(-i);
                        int doanhThuThang = dsDonHang
                            .Where(x => x.NgayDat.Year == thang.Year && x.NgayDat.Month == thang.Month)?
                            .Sum(x => x.TongTienSanPham) ?? 0;

                        doanhThu[thang] = doanhThuThang;
                    }
                    break;

                case "1_year":
                    for (int i = 0; i < 12; i++)
                    {
                        DateTime thang = homNay.AddMonths(-i);
                        int doanhThuThang = dsDonHang
                            .Where(x => x.NgayDat.Year == thang.Year && x.NgayDat.Month == thang.Month)?
                            .Sum(x => x.TongTienSanPham) ?? 0;

                        doanhThu[thang] = doanhThuThang;
                    }
                    break;
                default:
                    break;
            }
            Dictionary<string, int> result = doanhThu.ToDictionary(
                kvp => loaiThoiGian == "30_days" ? kvp.Key.ToString("dd/MM/yyyy") : kvp.Key.ToString("MM/yyyy", new System.Globalization.CultureInfo("vi-VN")),
                kvp => kvp.Value
            );

            return JsonConvert.SerializeObject(result);
        }
        /// <summary>
        /// Lấy danh sách các sản phẩm bán chạy nhất trong tuần.
        /// </summary>
        /// <param name="take">Số lượng sản phẩm cần lấy</param>
        /// <returns>Chuỗi JSON chứa danh sách các sản phẩm bán chạy nhất</returns>
        public static string getSanPhamBanChayNhatTuan(int take)
        {
            DateTime ngayBatDauTuan = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var sanPhamBanChay = getDonHangDaGiao()
                .Where(dh => dh.NgayDat >= ngayBatDauTuan)
                .SelectMany(dh => dh.ChiTietDonHangs)
                .GroupBy(ct => ct.MaSanPham)
                .Select(g => new
                {
                    MaSanPham = g.Key,
                    TongSoLuong = g.Sum(ct => ct.SoLuong)
                })
                .OrderByDescending(x => x.TongSoLuong)
                .Take(take)
                .ToList();

            Dictionary<int, string> sanPhamDict = FastFood_SanPham.getSanPham()
                .ToDictionary(sp => sp.MaSanPham, sp => sp.TenSanPham);

            List<FastFood_DonHang_SanPhamBanChay> ketQua = sanPhamBanChay.Select(sp => new FastFood_DonHang_SanPhamBanChay
            {
                MaSanPham = sp.MaSanPham,
                TenSanPham = sanPhamDict.ContainsKey(sp.MaSanPham) ? sanPhamDict[sp.MaSanPham] : "Không có tên sản phẩm",
                SoLuongDaBan = sp.TongSoLuong
            }).ToList();

            return JsonConvert.SerializeObject(ketQua);
        }

        /// <summary>
        /// Lấy danh sách đơn hàng trong tuần.
        /// </summary>
        /// <returns>Chuỗi JSON chứa thống kê đơn hàng theo ngày trong tuần</returns>
        public static string getDonHangTrongTuan()
        {
            DateTime today = DateTime.Today;
            var orderStats = getDonHang().AsEnumerable()
                .Where(dh => dh.TrangThaiDon != 0 && dh.NgayDat >= today.AddDays(-7))
                .GroupBy(d => d.NgayDat.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("dd-MM-yyyy"),
                    OrderCount = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToList();

            return JsonConvert.SerializeObject(orderStats);
        }

        /// <summary>
        /// Đếm tổng số đơn hàng trong hệ thống.
        /// </summary>
        /// <returns>Tổng số đơn hàng</returns>
        public static int demTongDonHang()
        {
            using (FastFoodEntities e = new FastFoodEntities())
                return e.DonHangs.Count();
        }

        /// <summary>
        /// Đếm tổng số khách hàng đã đặt hàng.
        /// </summary>
        /// <returns>Tổng số khách hàng đã đặt hàng</returns>
        public static int demKhachDaDatHang()
        {
            using (FastFoodEntities e = new FastFoodEntities())
                return e.KhachHangs.Count(x => x.DonHangs.Any());
        }

        /// <summary>
        /// Tính tổng doanh thu từ các đơn hàng.
        /// </summary>
        /// <returns>Tổng doanh thu từ các đơn hàng</returns>
        public static int tongDoanhThuDonHang()
        {
            using (FastFoodEntities e = new FastFoodEntities())
                return e.DonHangs.Sum(x => x.TongTienSanPham);
        }

        /// <summary>
        /// Tính trung bình doanh thu của một đơn hàng.
        /// </summary>
        /// <returns>Trung bình doanh thu của một đơn hàng</returns>
        public static double trungBinhDonHang()
        {
            using (FastFoodEntities e = new FastFoodEntities())
                return e.DonHangs.Average(x => x.TongTienSanPham);
        }
        /// <summary>
        /// Lấy danh sách đơn hàng trong hệ thống.
        /// </summary>
        /// <returns>Danh sách các đơn hàng dưới dạng đối tượng JSON</returns>
        public static IEnumerable<FastFood_DonHang_DanhSachDonHang> getDanhSachDonHang()
        {
            IEnumerable<FastFood_DonHang_DanhSachDonHang> ketQua = getDonHang().Select(dh => new FastFood_DonHang_DanhSachDonHang
            {
                MaDonHang = dh.MaDonHang,
                NgayDat = dh.NgayDat,
                NguoiBan = dh.NhanVien != null ? dh.NhanVien.HoDem + " " + dh.NhanVien.TenNhanVien : null,
                TongTienSanPham = dh.TongTienSanPham,
                TrangThaiDonHang = dh.TrangThaiDonHang,
                MaTrangThaiThanhToan = dh.ThanhToans.Select(x => x.TrangThaiThanhToan).FirstOrDefault(),
                TrangThaiThanhToan = dh.ThanhToans.Select(x => x.TrangThaiThanhToan).FirstOrDefault() == false ? "Chưa thanh toán" : "Đã thanh toán",
                TongSanPham = dh.ChiTietDonHangs.Count(),
                NguoiMua = dh.KhachHang.HoDem + " " + dh.KhachHang.TenKhachHang
            }) ?? Enumerable.Empty<FastFood_DonHang_DanhSachDonHang>();
            return ketQua;
        }
    }
    public class FastFood_DonHang_SanPhamBanChay
    {
        public int MaSanPham { get; set; } = 0;
        public string TenSanPham { get; set; } = string.Empty;
        public int SoLuongDaBan { get; set; } = 0;
    }

    public class FastFood_DonHang_DanhSachDonHang
    {
        public int MaDonHang { get; set; } = 0;
        public DateTime? NgayDat { get; set; } = null;
        public string NguoiMua { get; set; } = string.Empty;
        public string NguoiBan { get; set; } = string.Empty;
        public int TongTienSanPham { get; set; } = 0;
        public int? TongThanhToan { get; set; } = 0;
        public TrangThaiDonHang TrangThaiDonHang { get; set; } = new TrangThaiDonHang();
        public bool? MaTrangThaiThanhToan { get; set; } = false;
        public string TrangThaiThanhToan { get; set; } = string.Empty;
        public int TongSanPham { get; set; } = 0;
        public FastFood_DonHang_DanhSachDonHang() { }

    }

    public class FastFood_DonHang_ChiTietDonHang : FastFood_DonHang_DanhSachDonHang
    {
        public IEnumerable<ChiTietDonHang> Chitietdonhangs { get; set; } = Enumerable.Empty<ChiTietDonHang>();
        public KhachHang Khachhang { get; set; } = new KhachHang();
        public int? PhiVanChuyen { get; set; } = 0;
        public string NguoiGiao { get; set; } = string.Empty;
        public DateTime? ThoiGianGiaoHangDuKien { get; set; } = null;
        public DateTime? ThoiGianGiaoHangThucTe { get; set; } = null;
        public string PhuongThucVanChuyen { get; set; } = string.Empty;
        public int MaThanhToan { get; set; } = 0;
        public int MaKhuyenMai { get; set; } = 0;
        public long? MaGiaoDich { get; set; } = null;
        public DateTime? NgayThanhToan { get; set; } = null;

        public FastFood_DonHang_ChiTietDonHang() : base() { }
        public FastFood_DonHang_ChiTietDonHang(FastFood_DonHang_ChiTietDonHang a)
        {
            Chitietdonhangs = a.Chitietdonhangs;
            Khachhang = a.Khachhang;
            PhiVanChuyen = a.PhiVanChuyen;
            NguoiGiao = a.NguoiGiao;
            ThoiGianGiaoHangDuKien = a.ThoiGianGiaoHangDuKien;
            ThoiGianGiaoHangThucTe = a.ThoiGianGiaoHangThucTe;
            PhuongThucVanChuyen = a.PhuongThucVanChuyen;
            MaThanhToan = a.MaThanhToan;
            MaKhuyenMai = a.MaKhuyenMai;
            MaGiaoDich = a.MaGiaoDich;
            NgayThanhToan = a.NgayThanhToan;
        }
    }

    public class FastFood_DonHang_ThemThongTinVanChuyen
    {
        public int MaDonHang { get; set; }
        [Display(Name = "Đơn vị vận chuyển")]
        [DataType(DataType.Text)]
        public string DonViVanChuyen { get; set; } = string.Empty;
        [Display(Name = "Mã vận đơn")]
        [DataType(DataType.Text)]
        public string MaVanDon { get; set; } = string.Empty;
        [Display(Name = "Số ngày dự kiến")]
        [DataType(DataType.Text)]
        public int SoNgayDuKien { get; set; } = 0;
        public FastFood_DonHang_ThemThongTinVanChuyen() { }
        public FastFood_DonHang_ThemThongTinVanChuyen(FastFood_DonHang_ThemThongTinVanChuyen a)
        {
            MaDonHang = a.MaDonHang;
            DonViVanChuyen = a.DonViVanChuyen;
            MaVanDon = a.MaVanDon;
            SoNgayDuKien = a.SoNgayDuKien;
        }
    }
}
