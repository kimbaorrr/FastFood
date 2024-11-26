using FastFood.DB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FastFood.Models
{
    /// <summary>
    /// Lớp FastFood_DanhMuc quản lý các danh mục trong hệ thống cửa hàng thức ăn nhanh.
    /// </summary>
    public static class FastFood_DanhMuc
    {
        /// <summary>
        /// Khởi tạo một đối tượng mới của FastFoodEntities để truy xuất dữ liệu.
        /// </summary>
        private static FastFoodEntities context => new FastFoodEntities();

        /// <summary>
        /// Tập hợp các danh mục trong cơ sở dữ liệu.
        /// </summary>
        private static IQueryable<DanhMuc> danhMucs => context.DanhMucs;

        /// <summary>
        /// Lấy tất cả các danh mục trong cơ sở dữ liệu.
        /// </summary>
        /// <returns>Một tập hợp các danh mục.</returns>
        public static IQueryable<DanhMuc> getDanhMuc()
        {
            return danhMucs;
        }

        /// <summary>
        /// Đếm số lượng sản phẩm trong một danh mục cụ thể.
        /// </summary>
        /// <param name="maDM">Mã của danh mục cần đếm số lượng sản phẩm.</param>
        /// <returns>Số lượng sản phẩm trong danh mục có mã đã chỉ định.</returns>
        public static int demSanPham(int maDM)
        {
            using (FastFoodEntities e = new FastFoodEntities())
                return e.SanPhams?.Count(x => x.MaDanhMuc == maDM) ?? 0;
        }

        /// <summary>
        /// Lấy các danh mục có nhiều sản phẩm nhất, sắp xếp giảm dần theo số lượng sản phẩm.
        /// </summary>
        /// <param name="take">Số lượng danh mục cần lấy.</param>
        /// <returns>Một tập hợp các danh mục có nhiều sản phẩm nhất.</returns>
        public static IEnumerable<DanhMuc> getDanhMucNhieuSanPham(int take)
        {
            return getDanhMuc()
                .OrderByDescending(x => x.SanPhams.Count)
                .Take(take);
        }
    }

    public class FastFood_DanhMuc_TaoMoi
    {
        [Display(Name = "Tên danh mục")]
        [DataType(DataType.Text)]
        public string TenDanhMuc { get; set; } = string.Empty;
        [Display(Name = "Mô tả")]
        [DataType(DataType.MultilineText)]
        public string MoTa { get; set; } = string.Empty;
        [Display(Name = "Ảnh đại diện")]
        [DataType(DataType.Text)]
        public HttpPostedFileBase AnhDaiDien { get; set; } = null;
        [Display(Name = "Ảnh nền")]
        [DataType(DataType.Text)]
        public HttpPostedFileBase AnhNen { get; set; } = null;
        [Display(Name = "Mã người tạo")]
        [DataType(DataType.Text)]
        public int MaNguoiTao { get; set; } = 0;
        public FastFood_DanhMuc_TaoMoi() { }
        public FastFood_DanhMuc_TaoMoi(FastFood_DanhMuc_TaoMoi a)
        {
            TenDanhMuc = a.TenDanhMuc;
            MoTa = a.MoTa;
            AnhNen = a.AnhNen;
            AnhDaiDien = a.AnhDaiDien;
            MaNguoiTao = a.MaNguoiTao;
        }

    }
}