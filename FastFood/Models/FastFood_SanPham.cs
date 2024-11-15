using FastFood.DB;
using System.Collections.Generic;
using System.Linq;

namespace FastFood.Models
{
    public class FastFood_SanPham
    {
        private static FastFoodEntities context = new FastFoodEntities();

        private static IQueryable<SanPham> sanPhams => context.SanPhams;
        private static IQueryable<MaKhuyenMai> maKhuyenMais => context.MaKhuyenMais;
        private static IQueryable<DanhMuc> danhMucs => context.DanhMucs;
        private static IQueryable<DanhGiaSanPham> danhGiaSanPhams => context.DanhGiaSanPhams;

        public static IQueryable<SanPham> getSanPham()
        {
            return sanPhams;
        }

        public static IEnumerable<SanPham> getSanPhamDaDuyet()
        {
            return getSanPham().Where(x => x.DaDuyet) ?? Enumerable.Empty<SanPham>();
        }

        public static IEnumerable<SanPham> getSanPhamBanChay(int take)
        {
            return getSanPhamDaDuyet()
                .OrderByDescending(x => x.ChiTietDonHangs.Count())
                .Take(take) ?? Enumerable.Empty<SanPham>().Take(take);
        }

        public static IEnumerable<SanPham> getSanPhamKhuyenMai(int take)
        {
            return getSanPhamDaDuyet()
                .OrderByDescending(x => x.KhuyenMai)
                .Take(take) ?? Enumerable.Empty<SanPham>().Take(take);
        }

        public static IEnumerable<SanPham> getSanPhamGiamGiaSoc(int take)
        {
            return getSanPhamDaDuyet()
                .OrderByDescending(x => x.KhuyenMai)
                .ThenBy(x => x.GiaSauKhuyenMai)
                .Take(take) ?? Enumerable.Empty<SanPham>().Take(take);
        }

        public static IQueryable<MaKhuyenMai> getMaKhuyenMai()
        {
            return maKhuyenMais;
        }

        public static IQueryable<DanhMuc> getDanhMuc()
        {
            return danhMucs;
        }

        public static string getTenDanhMuc(int maDM)
        {
            return getDanhMuc()
                .Where(x => x.MaDanhMuc == maDM)
                .Select(x => x.TenDanhMuc)
                .FirstOrDefault() ?? string.Empty;
        }

        public static IEnumerable<SanPham> getSanPhamTheoDanhMuc(int? maDM, int take)
        {
            return getSanPhamDaDuyet()
                .Where(x => x.MaDanhMuc == maDM)
                .Take(take) ?? Enumerable.Empty<SanPham>().Take(take);
        }

        public static IQueryable<DanhGiaSanPham> getDanhGiaSanPham()
        {
            return danhGiaSanPhams;
        }

        public static IEnumerable<DanhGiaSanPham> getKhachHangDanhGia()
        {
            return getDanhGiaSanPham()
                .Where(x => !string.IsNullOrEmpty(x.DanhGia))
                .GroupBy(x => x.MaKhachHang)
                .Select(g => g.OrderByDescending(x => x.XepHangSao).FirstOrDefault())
                .OrderByDescending(x => x.XepHangSao) ?? Enumerable.Empty<DanhGiaSanPham>();
        }
        public static IEnumerable<DanhGiaSanPham> GetDanhGiaTheoMaSP(int maSP)
        {
            return getDanhGiaSanPham().Where(x => x.MaSanPham == maSP) ?? Enumerable.Empty<DanhGiaSanPham>();
        }
    }
    public class FastFood_SanPham_DanhGiaSanPham
    {
        public int MaKhachHang { get; set; } = 0;
        public string TenKhachHang { get; set; } = string.Empty;
        public string NoiDung { get; set; } = string.Empty;
        public int XepHangSao { get; set; } = 3;
        public FastFood_SanPham_DanhGiaSanPham() { }
        public FastFood_SanPham_DanhGiaSanPham(FastFood_SanPham_DanhGiaSanPham a)
        {
            MaKhachHang = a.MaKhachHang;
            TenKhachHang = a.TenKhachHang;
            NoiDung = a.NoiDung;
            XepHangSao = a.XepHangSao;
        }

    }
}
