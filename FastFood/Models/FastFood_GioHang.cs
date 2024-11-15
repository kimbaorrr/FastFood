using FastFood.DB;
using System;
using System.Collections.Generic;
using System.Linq;
namespace FastFood.Models
{
    /// <summary>
    /// Lớp đại diện cho giỏ hàng của khách hàng.
    /// </summary>
    public class FastFood_GioHang
    {
        /// <summary>
        /// Danh sách sản phẩm đã chọn trong giỏ hàng.
        /// </summary>
        public Dictionary<int, FastFood_GioHang_DonHangCuaToi> SanPhamDaChon { get; set; }

        /// <summary>
        /// Khởi tạo một đối tượng giỏ hàng mới.
        /// </summary>
        public FastFood_GioHang()
        {
            SanPhamDaChon = new Dictionary<int, FastFood_GioHang_DonHangCuaToi>();
        }

        /// <summary>
        /// Sao chép đối tượng giỏ hàng.
        /// </summary>
        /// <param name="a">Đối tượng giỏ hàng để sao chép.</param>
        public FastFood_GioHang(FastFood_GioHang a)
        {
            SanPhamDaChon = a.SanPhamDaChon;
        }

        /// <summary>
        /// Kiểm tra xem giỏ hàng có rỗng hay không.
        /// </summary>
        /// <returns>Trả về true nếu giỏ hàng rỗng, ngược lại là false.</returns>
        public bool GioHangRong() => !SanPhamDaChon.Any();

        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng.
        /// </summary>
        /// <param name="maSanPham">Mã sản phẩm.</param>
        /// <param name="soLuong">Số lượng sản phẩm cần thêm.</param>
        public void ThemVaoGio(int maSanPham, int soLuong)
        {
            if (SanPhamDaChon.TryGetValue(maSanPham, out FastFood_GioHang_DonHangCuaToi sanPham))
            {
                sanPham.SoLuong += soLuong;
            }
            else
            {
                SanPham sp = FastFood_SanPham.getSanPham().FirstOrDefault(a => a.MaSanPham == maSanPham);
                if (sp == null) return;

                SanPhamDaChon[maSanPham] = new FastFood_GioHang_DonHangCuaToi
                {
                    MaSanPham = maSanPham,
                    SoLuong = soLuong,
                    TenSanPham = sp.TenSanPham,
                    GiaBan = Convert.ToInt32(sp.GiaSauKhuyenMai),
                    HinhAnh = FastFood_Tools.SplitAnh(sp.HinhAnh).FirstOrDefault()
                };
            }
        }

        /// <summary>
        /// Xóa sản phẩm khỏi giỏ hàng.
        /// </summary>
        /// <param name="maSanPham">Mã sản phẩm cần xóa.</param>
        public void XoaKhoiGio(int maSanPham)
        {
            SanPhamDaChon.Remove(maSanPham);
        }

        /// <summary>
        /// Giảm số lượng của một sản phẩm trong giỏ hàng.
        /// </summary>
        /// <param name="maSanPham">Mã sản phẩm cần giảm số lượng.</param>
        public void GiamSoLuong(int maSanPham)
        {
            if (!SanPhamDaChon.TryGetValue(maSanPham, out FastFood_GioHang_DonHangCuaToi sanPham)) return;

            sanPham.SoLuong--;
            if (sanPham.SoLuong <= 0)
            {
                SanPhamDaChon.Remove(maSanPham);
            }
        }

        /// <summary>
        /// Tính tổng tiền của tất cả sản phẩm trong giỏ hàng.
        /// </summary>
        /// <returns>Tổng tiền của giỏ hàng.</returns>
        public int TongTien() => SanPhamDaChon.Values?.Sum(x => x.ThanhTien) ?? 0;
    }

    /// <summary>
    /// Lớp đại diện cho sản phẩm trong giỏ hàng của khách hàng.
    /// </summary>
    public class FastFood_GioHang_DonHangCuaToi
    {
        /// <summary>
        /// Mã sản phẩm.
        /// </summary>
        public int MaSanPham { get; set; }

        /// <summary>
        /// Tên sản phẩm.
        /// </summary>
        public string TenSanPham { get; set; }

        /// <summary>
        /// Số lượng sản phẩm.
        /// </summary>
        public int SoLuong { get; set; }

        /// <summary>
        /// Giá bán của sản phẩm.
        /// </summary>
        public int GiaBan { get; set; }

        /// <summary>
        /// Tổng tiền của sản phẩm dựa trên số lượng và giá bán.
        /// </summary>
        public int ThanhTien => GiaBan * SoLuong;

        /// <summary>
        /// Hình ảnh của sản phẩm.
        /// </summary>
        public string HinhAnh { get; set; }
    }
}
