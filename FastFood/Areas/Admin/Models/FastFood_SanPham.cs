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
            return getSanPham().Where(x => x.DaDuyet);
        }

        public static IEnumerable<SanPham> getSanPhamChuaDuyet()
        {
            return getSanPham().Where(x => !x.DaDuyet);
        }

        public static double getXepHangSaoTrungBinh(int maSanPham)
        {
            return getSanPham().FirstOrDefault(x => x.MaSanPham == maSanPham).DanhGiaSanPhams?.Average(d => d.XepHangSao) ?? 3;
        }

        public static int getTongLuotDanhGia(int maSanPham)
        {
            return getSanPham().FirstOrDefault(x => x.MaSanPham == maSanPham).DanhGiaSanPhams.Count();

        }
    }

    public class FastFood_SanPham_ThemSanPham
    {


        [Display(Name = "Tên sản phẩm")]
        [DataType(DataType.Text)]
        public string TenSanPham { get; set; }

        [Display(Name = "Danh mục")]
        [DataType(DataType.Text)]
        public int MaDanhMuc { get; set; }

        [Display(Name = "Giá gốc")]
        [DataType(DataType.Currency)]
        public int GiaGoc { get; set; }

        [Display(Name = "Khuyến mãi (%)")]
        [Range(0, 100)]
        public int KhuyenMai { get; set; }

        [Display(Name = "Giá sau khuyến mãi")]
        [DataType(DataType.Currency)]
        public int GiaSauKhuyenMai { get; set; }

        [Display(Name = "Mô tả ngắn")]
        [DataType(DataType.MultilineText)]
        public string MoTaNgan { get; set; }
        [Display(Name = "Mô tả dài")]
        [DataType(DataType.MultilineText)]
        public string MoTaDai { get; set; }
        [Display(Name = "Ngày tạo")]
        [DataType(DataType.DateTime)]
        public DateTime NgayTao { get; set; }

        [Display(Name = "Ngày cập nhật")]
        [DataType(DataType.DateTime)]
        public DateTime NgayCapNhat { get; set; }
        [Display(Name = "Người tạo")]
        [DataType(DataType.MultilineText)]
        public string NguoiTao { get; set; }
        public int MaNguoiTao { get; set; }
        public SelectList DanhMuc { get; set; }
        public FastFood_SanPham_ThemSanPham()
        {
            TenSanPham = string.Empty;
            MaDanhMuc = -1;
            GiaGoc = 0;
            KhuyenMai = 0;
            GiaSauKhuyenMai = 0;
            NgayTao = DateTime.Now;
            NgayCapNhat = DateTime.Now;
            NguoiTao = string.Empty;
            MaNguoiTao = -1;
            MoTaNgan = string.Empty;
            MoTaDai = string.Empty;
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
        public FastFood_SanPham_ThemSanPham SanPham { get; set; }
        public IEnumerable<FastFood_SanPham_NguyenLieuDaChon> DanhSachNguyenLieu { get; set; }
        public List<HttpPostedFileBase> HinhAnh { get; set; }
    }

    public class FastFood_SanPham_ChiTietSanPham : FastFood_SanPham_ThemSanPham
    {
        [DataType(DataType.Text)]
        [Display(Name = "Mã sản phẩm")]
        public int MaSanPham { get; set; }
        [Display(Name = "Trạng thái duyệt")]
        [DataType(DataType.Text)]
        public string TrangThaiDuyet { get; set; }
        [Display(Name = "Ngày duyệt")]
        [DataType(DataType.DateTime)]
        public DateTime? NgayDuyet { get; set; }
        [Display(Name = "Người duyệt")]
        [DataType(DataType.Text)]
        public string NguoiDuyet { get; set; }
        public FastFood_SanPham_ChiTietSanPham()
        {
            MaSanPham = 0;
            TrangThaiDuyet = string.Empty;
            NguoiDuyet = string.Empty;
            NgayDuyet = null;
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
        public FastFood_SanPham_ChiTietSanPham SanPham { get; set; }
        public IEnumerable<FastFood_SanPham_NguyenLieuDaChon> DanhSachNguyenLieu { get; set; }
        public List<HttpPostedFileBase> HinhAnh { get; set; }
    }

    public class FastFood_SanPham_NguyenLieuDaChon
    {
        public string MaNguyenLieu { get; set; }
        public int SoLuong { get; set; }
        public string DonViTinh { get; set; }
    }


}
