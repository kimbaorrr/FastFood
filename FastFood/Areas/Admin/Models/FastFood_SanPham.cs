using FastFood.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastFood.Areas.Admin.Models
{
    public class FastFood_SanPham
    {
        private static FastFoodEntities context => new FastFoodEntities();
        private static IQueryable<SanPham> sanPhams => context.SanPhams;

        public static IQueryable<SanPham> getSanPham()
        {
            return sanPhams;
        }

        public static IEnumerable<SanPham> getSanPhamDaDuyet()
        {
            return getSanPham().Where(x => x.DaDuyet) ?? Enumerable.Empty<SanPham>();
        }

        public static IEnumerable<SanPham> getSanPhamChuaDuyet()
        {
            return getSanPham().Where(x => !x.DaDuyet) ?? Enumerable.Empty<SanPham>();
        }

        public static double getXepHangSaoTrungBinh(int maSanPham)
        {
            return getSanPham().FirstOrDefault(x => x.MaSanPham == maSanPham).DanhGiaSanPhams?.Average(d => d.XepHangSao) ?? 3;
        }

        public static int getTongLuotDanhGia(int maSanPham)
        {
            return getSanPham().FirstOrDefault(x => x.MaSanPham == maSanPham)?.DanhGiaSanPhams.Count() ?? 0;

        }
    }

    public class FastFood_SanPham_ThemSanPham
    {


        [Display(Name = "Tên sản phẩm")]
        [DataType(DataType.Text)]
        public string TenSanPham { get; set; } = string.Empty;

        [Display(Name = "Danh mục")]
        [DataType(DataType.Text)]
        public int MaDanhMuc { get; set; } = 0;

        [Display(Name = "Giá gốc")]
        [DataType(DataType.Currency)]
        public int GiaGoc { get; set; } = 0;

        [Display(Name = "Khuyến mãi (%)")]
        [Range(0, 100)]
        public int KhuyenMai { get; set; } = 0;

        [Display(Name = "Giá sau khuyến mãi")]
        [DataType(DataType.Currency)]
        public int GiaSauKhuyenMai { get; set; } = 0;

        [Display(Name = "Mô tả ngắn")]
        [DataType(DataType.MultilineText)]
        public string MoTaNgan { get; set; } = string.Empty;
        [Display(Name = "Mô tả dài")]
        [DataType(DataType.MultilineText)]
        public string MoTaDai { get; set; } = string.Empty;
        [Display(Name = "Ngày tạo")]
        [DataType(DataType.DateTime)]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        [Display(Name = "Ngày cập nhật")]
        [DataType(DataType.DateTime)]
        public DateTime? NgayCapNhat { get; set; } = null;
        [Display(Name = "Người tạo")]
        [DataType(DataType.MultilineText)]
        public string NguoiTao { get; set; } = string.Empty;
        public int MaNguoiTao { get; set; } = 0;
        public SelectList DanhMuc { get; set; } = null;
        public FastFood_SanPham_ThemSanPham()
        {

        }

        public FastFood_SanPham_ThemSanPham(FastFood_SanPham_ThemSanPham a)
        {

            TenSanPham = a.TenSanPham;
            MaDanhMuc = a.MaDanhMuc;
            GiaGoc = a.GiaGoc;
            KhuyenMai = a.KhuyenMai;
            GiaSauKhuyenMai = a.GiaSauKhuyenMai;
            MoTaNgan = a.MoTaNgan;
            MoTaDai = a.MoTaDai;
            NgayTao = a.NgayTao;
            NgayCapNhat = a.NgayCapNhat;
            NguoiTao = a.NguoiTao;
            MaNguoiTao = a.MaNguoiTao;
        }
    }

    public class FastFood_SanPham_ThemSanPham_Post
    {
        public FastFood_SanPham_ThemSanPham SanPham { get; set; } = new FastFood_SanPham_ThemSanPham();
        public IEnumerable<FastFood_SanPham_NguyenLieuDaChon> DanhSachNguyenLieu { get; set; } = Enumerable.Empty<FastFood_SanPham_NguyenLieuDaChon>();
        public List<HttpPostedFileBase> HinhAnh { get; set; } = Enumerable.Empty<HttpPostedFileBase>().ToList();
    }

    public class FastFood_SanPham_ChiTietSanPham : FastFood_SanPham_ThemSanPham
    {
        [DataType(DataType.Text)]
        [Display(Name = "Mã sản phẩm")]
        public int MaSanPham { get; set; } = 0;
        [Display(Name = "Trạng thái duyệt")]
        [DataType(DataType.Text)]
        public string TrangThaiDuyet { get; set; } = string.Empty;
        [Display(Name = "Ngày duyệt")]
        [DataType(DataType.DateTime)]
        public DateTime? NgayDuyet { get; set; } = null;
        [Display(Name = "Người duyệt")]
        [DataType(DataType.Text)]
        public string NguoiDuyet { get; set; } = string.Empty;
        public FastFood_SanPham_ChiTietSanPham()
        {

        }
        public FastFood_SanPham_ChiTietSanPham(FastFood_SanPham_ChiTietSanPham a)
        {
            MaSanPham = a.MaSanPham;
            TrangThaiDuyet = a.TrangThaiDuyet;
            NguoiDuyet = a.NguoiDuyet;
            NgayDuyet = a.NgayDuyet;
        }
    }

    public class FastFood_SanPham_ChiTietSanPham_Post
    {
        public FastFood_SanPham_ChiTietSanPham SanPham { get; set; } = new FastFood_SanPham_ChiTietSanPham();
        public IEnumerable<FastFood_SanPham_NguyenLieuDaChon> DanhSachNguyenLieu { get; set; } = Enumerable.Empty<FastFood_SanPham_NguyenLieuDaChon>();
        public List<HttpPostedFileBase> HinhAnh { get; set; } = Enumerable.Empty<HttpPostedFileBase>().ToList();
    }

    public class FastFood_SanPham_NguyenLieuDaChon
    {
        public string MaNguyenLieu { get; set; } = string.Empty;
        public int SoLuong { get; set; } = 1;
        public string DonViTinh { get; set; } = "cái";
    }


}
