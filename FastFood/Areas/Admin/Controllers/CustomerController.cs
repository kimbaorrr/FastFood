using FastFood.Areas.Admin.Models;
using FastFood.DB;
using System;
using System.Linq;
using System.Web.Mvc;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{
    public class CustomerController : SessionController
    {
        /// <summary>
        /// Hiển thị danh sách tất cả khách hàng.
        /// </summary>
        /// <param name="page">Số trang hiện tại.</param>
        /// <param name="size">Số lượng khách hàng hiển thị trên mỗi trang.</param>
        /// <returns>Trang danh sách khách hàng.</returns>
        [HttpGet]
        public ActionResult List(int page = 1, int size = 10)
        {
            ViewBag.Title = "Tất cả khách hàng";
            X.PagedList.IPagedList<KhachHang> khachHang = FastFood_KhachHang.getKhachHang().OrderBy(m => m.MaKhachHang).ToPagedList(page, size);
            ViewBag.KhachHang = khachHang;
            ViewBag.CurrentPage = khachHang.PageNumber;
            ViewBag.TotalPages = khachHang.PageCount;
            return View();
        }
        /// <summary>
        /// Hiển thị danh sách các khách hàng tiềm năng dựa trên tổng giá trị hóa đơn.
        /// </summary>
        /// <param name="page">Số trang hiện tại.</param>
        /// <param name="size">Số lượng khách hàng hiển thị trên mỗi trang.</param>
        /// <returns>Trang danh sách khách hàng tiềm năng.</returns>
        [HttpGet]
        public ActionResult Potential(int page = 1, int size = 10)
        {
            ViewBag.Title = "Khách hàng tiềm năng";
            X.PagedList.IPagedList<FastFood_KhachHang_DanhSachKhachHang> khachHang = FastFood_KhachHang.getKhachHangTiemNang().OrderByDescending(m => m.TongHoaDon).ToPagedList(page, size);
            ViewBag.KhachHang = khachHang;
            ViewBag.CurrentPage = khachHang.PageNumber;
            ViewBag.TotalPages = khachHang.PageCount;
            return View();
        }
        /// <summary>
        /// Hiển thị chi tiết của một khách hàng cụ thể.
        /// </summary>
        /// <param name="id">ID của khách hàng.</param>
        /// <param name="return_url">URL trả về sau khi xem chi tiết.</param>
        /// <param name="page">Số trang hiện tại của danh sách đơn hàng.</param>
        /// <param name="size">Số lượng đơn hàng hiển thị trên mỗi trang.</param>
        /// <returns>Trang chi tiết khách hàng.</returns>
        [HttpGet]
        public ActionResult Detail(int id, string return_url, int page = 1, int size = 10)
        {
            KhachHang kh = FastFood_KhachHang.getKhachHang().FirstOrDefault(x => x.MaKhachHang == id);
            if (kh == null)
                return HttpNotFound();

            DateTime? thoiGianTruyCap = FastFood_KhachHang.getLichSuTruyCap()
                .OrderByDescending(x => x.ThoiGianTruyCap)
                .Select(x => x.ThoiGianTruyCap)
                .FirstOrDefault();

            int hoatDongGanDay = thoiGianTruyCap.HasValue ? (DateTime.Now - thoiGianTruyCap.Value).Hours : 0;
            hoatDongGanDay = hoatDongGanDay < 12 ? 0 : hoatDongGanDay;

            X.PagedList.IPagedList<DB.DonHang> donHangs = kh.DonHangs.OrderBy(x => x.MaDonHang).ToPagedList(page, size);

            FastFood_KhachHang_ChiTiet chiTiet = new FastFood_KhachHang_ChiTiet
            {
                MaKhachHang = id,
                HoTenKhachHang = $"{kh.HoDem} {kh.TenKhachHang}",
                NgaySinh = kh.NgaySinh?.ToString("dd/MM/yyyy"),
                NgayTao = kh.NgayTao,
                DiaChi = kh.DiaChi,
                Email = kh.Email,
                SoDienThoai = kh.SoDienThoai,
                AnhDD = kh.AnhDD,
                TongChiTieu = kh.DonHangs.Sum(x => x.TongTienSanPham),
                TongHoaDon = kh.DonHangs.Count(),
                DonHangs = donHangs,
                ChiTieuLonNhat = kh.DonHangs.Max(x => x.TongTienSanPham),
                HoatDongGanDay = hoatDongGanDay
            };

            ViewBag.ChiTietKhachHang = chiTiet;
            ViewBag.ReturnURL = return_url;
            ViewBag.PagedList = donHangs;
            return View();
        }
    }
}