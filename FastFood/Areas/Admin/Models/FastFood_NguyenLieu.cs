using FastFood.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FastFood.Areas.Admin.Models
{
    /// <summary>
    /// Lớp quản lý các nguyên liệu trong hệ thống thức ăn nhanh.
    /// </summary>
    public class FastFood_NguyenLieu
    {
        /// <summary>
        /// Lấy ra đối tượng context từ cơ sở dữ liệu.
        /// </summary>
        private static FastFoodEntities context => new FastFoodEntities();

        /// <summary>
        /// Truy vấn nguyên liệu từ cơ sở dữ liệu.
        /// </summary>
        private static IQueryable<NguyenLieu> nguyenLieux => context.NguyenLieux;

        /// <summary>
        /// Truy vấn nguyên liệu của sản phẩm từ cơ sở dữ liệu.
        /// </summary>
        private static IQueryable<SanPham_NguyenLieu> sanPhamNguyenLieus => context.SanPham_NguyenLieu;

        /// <summary>
        /// Truy vấn nhập kho từ cơ sở dữ liệu.
        /// </summary>
        private static IQueryable<NhapKho> nhapKhoes => context.NhapKhoes;

        /// <summary>
        /// Lấy danh sách nguyên liệu.
        /// </summary>
        /// <returns>Danh sách nguyên liệu.</returns>
        public static IQueryable<NguyenLieu> getNguyenLieu()
        {
            return nguyenLieux;
        }

        /// <summary>
        /// Lấy danh sách nguyên liệu của sản phẩm.
        /// </summary>
        /// <returns>Danh sách nguyên liệu của sản phẩm.</returns>
        public static IQueryable<SanPham_NguyenLieu> getSanPhamNguyenLieu()
        {
            return sanPhamNguyenLieus;
        }

        /// <summary>
        /// Lấy danh sách nhập kho.
        /// </summary>
        /// <returns>Danh sách nhập kho.</returns>
        public static IQueryable<NhapKho> getNhapKho()
        {
            return nhapKhoes;
        }

        /// <summary>
        /// Lấy tất cả nguyên liệu với các thông tin cơ bản (Mã, tên, mô tả).
        /// </summary>
        /// <returns>Danh sách nguyên liệu cơ bản.</returns>
        public static IEnumerable<FastFood_NguyenLieu_Get> getAll()
        {
            return getNguyenLieu().Select(x => new FastFood_NguyenLieu_Get { MaNguyenLieu = x.MaNguyenLieu, TenNguyenLieu = x.TenNguyenLieu, MoTa = x.MoTa }) ?? Enumerable.Empty<FastFood_NguyenLieu_Get>();
        }

        /// <summary>
        /// Đếm số lượng nguyên liệu sắp hết dựa trên các sản phẩm cần nguyên liệu.
        /// </summary>
        /// <returns>Số lượng nguyên liệu sắp hết.</returns>
        public static int demNguyenLieuSapHet()
        {
            IEnumerable<SanPham_NguyenLieu> dsSPNL = getSanPhamNguyenLieu().AsEnumerable();
            int nguyenLieuSapHet = dsSPNL
                .GroupBy(x => x.MaNguyenLieu)
                .Select(group => new
                {
                    MaNguyenLieu = group.Key,
                    TongSoLuongCan = group.Sum(x => x.SoLuongCan)
                })
                .Join(
                    getNguyenLieu(),
                    spnl => spnl.MaNguyenLieu,
                    nl => nl.MaNguyenLieu,
                    (spnl, nl) => new
                    {
                        MaNguyenLieu = spnl.MaNguyenLieu,
                        TongSoLuongCan = spnl.TongSoLuongCan,
                        MucDatHangLai = nl.MucDatHangLai
                    }
                )
                .Count(x => x.TongSoLuongCan >= x.MucDatHangLai);

            return nguyenLieuSapHet;
        }

        /// <summary>
        /// Lấy danh sách nguyên liệu theo mã sản phẩm.
        /// </summary>
        /// <param name="productID">Mã sản phẩm.</param>
        /// <returns>Danh sách nguyên liệu của sản phẩm.</returns>
        public static IEnumerable<FastFood_NguyenLieu_GetByProductID> getByProductID(int productID)
        {
            return getSanPhamNguyenLieu().Where(x => x.MaSanPham == productID).Select(x => new FastFood_NguyenLieu_GetByProductID
            {
                MaNguyenLieu = x.MaNguyenLieu,
                TenNguyenLieu = x.NguyenLieu.TenNguyenLieu,
                MoTa = x.NguyenLieu.MoTa,
                SoLuongCan = x.SoLuongCan,
                DonViTinh = x.DonViTinh
            }) ?? Enumerable.Empty<FastFood_NguyenLieu_GetByProductID>();
        }
    }


    public class FastFood_NguyenLieu_Get
    {
        public int MaNguyenLieu { get; set; } = 0;
        public string TenNguyenLieu { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
    }

    public class FastFood_NguyenLieu_GetByProductID
    {
        public int MaNguyenLieu { get; set; } = 0;
        public string TenNguyenLieu { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public decimal SoLuongCan { get; set; } = 0;
        public string DonViTinh { get; set; } = string.Empty;
    }
    public class FastFood_NguyenLieu_ThemPhieuNhap
    {
        [Display(Name = "Nguồn nguyên liệu đã có")]
        public int MaNguyenLieu { get; set; } = 0;
        [Display(Name = "Tên nguyên liệu")]
        [DataType(DataType.Text)]
        public string TenNguyenLieu { get; set; } = string.Empty;
        [Display(Name = "Số lượng nhập")]
        [DataType(DataType.Text)]
        public int SoLuongNhap { get; set; } = 0;
        [Display(Name = "Đơn vị tính")]
        [DataType(DataType.Text)]
        public string DonViTinh { get; set; } = "cái";
        [Display(Name = "Mức đặt hàng lại")]
        [DataType(DataType.Text)]
        public int MucDatHangLai { get; set; } = 0;
        [Display(Name = "Mô tả")]
        [DataType(DataType.Text)]
        public string MoTa { get; set; } = string.Empty;
        [Display(Name = "Ngày nhập")]
        [DataType(DataType.Text)]
        public DateTime NgayNhap { get; set; } = DateTime.Now;
        [Display(Name = "Người nhập")]
        [DataType(DataType.Text)]
        public string NguoiNhap { get; set; } = string.Empty;
        public int MaNguoiNhap { get; set; } = 0;
        public FastFood_NguyenLieu_ThemPhieuNhap() { }
        public FastFood_NguyenLieu_ThemPhieuNhap(FastFood_NguyenLieu_ThemPhieuNhap a)
        {
            MaNguyenLieu = a.MaNguyenLieu;
            TenNguyenLieu = a.TenNguyenLieu;
            SoLuongNhap = a.SoLuongNhap;
            DonViTinh = a.DonViTinh;
            MucDatHangLai = a.MucDatHangLai;
            MoTa = a.MoTa;
            NgayNhap = a.NgayNhap;
            NguoiNhap = a.NguoiNhap;
            MaNguoiNhap = a.MaNguoiNhap;
        }
    }

    public class IngredientSubmission
    {
        public Dictionary<string, string> FormData { get; set; } = new Dictionary<string, string>();
        public IEnumerable<SelectedIngredient> SelectedIngredients { get; set; } = Enumerable.Empty<SelectedIngredient>();
    }

    public class SelectedIngredient
    {
        public string MaNguyenLieu { get; set; } = string.Empty;
        public int SoLuong { get; set; } = 0;
        public string DonViTinh { get; set; } = "cái";
    }

}