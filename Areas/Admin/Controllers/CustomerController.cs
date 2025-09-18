using FastFood.DB;
using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/quan-ly-khach-hang")]
    public class CustomerController : SessionController
    {
        /// <summary>
        /// Hiển thị danh sách tất cả khách hàng.
        /// </summary>
        /// <param name="page">Số trang hiện tại.</param>
        /// <param name="size">Số lượng khách hàng hiển thị trên mỗi trang.</param>
        /// <returns>Trang danh sách khách hàng.</returns>
        [HttpGet("danh-sach-khach-hang")]
        public IActionResult List([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            ViewBag.Title = "Tất cả khách hàng";
            IPagedList<Khachhang> khachHang = FastFood_KhachHang.getKhachHang().OrderBy(m => m.Makhachhang).ToPagedList(page, size);
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
        [HttpGet("khach-hang-tiem-nang")]
        public IActionResult Potential([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            ViewBag.Title = "Khách hàng tiềm năng";
            IPagedList<FastFood_KhachHang_DanhSachKhachHang> khachHang = FastFood_KhachHang.getKhachHangTiemNang().OrderByDescending(m => m.TongHoaDon).ToPagedList(page, size);
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
        [HttpGet("chi-tiet-khach-hang")]
        public IActionResult Detail([FromQuery] int id, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            Khachhang? kh = FastFood_KhachHang.getKhachHang().Include(x => x.Donhangs).FirstOrDefault(x => x.Makhachhang == id);

            if (kh == null)
                return BadRequest();

            DateTime? thoiGianTruyCap = FastFood_KhachHang.getLichSuTruyCap()
                .OrderByDescending(x => x.Thoigiantruycap)
                .Select(x => x.Thoigiantruycap)
                .FirstOrDefault();

            int hoatDongGanDay = thoiGianTruyCap.HasValue ? DateTime.SpecifyKind(DateTime.Now, thoiGianTruyCap.Value.Kind).Hour : 0;
            hoatDongGanDay = hoatDongGanDay < 12 ? 0 : hoatDongGanDay;
            IPagedList<Donhang> donHangs = kh.Donhangs.OrderBy(x => x.Madonhang).ToPagedList(page, size);
            FastFood_KhachHang_ChiTiet chiTiet = new FastFood_KhachHang_ChiTiet
            {
                MaKhachHang = id,
                HoTenKhachHang = $"{kh.Hodem} {kh.Tenkhachhang}",
                NgaySinh = kh.Ngaysinh.HasValue ? kh.Ngaysinh.Value.ToString("dd/MM/yyyy") : string.Empty,
                NgayTao = kh.Ngaytao,
                DiaChi = kh.Diachi ?? string.Empty,
                Email = kh.Email ?? string.Empty,
                SoDienThoai = kh.Sodienthoai ?? string.Empty,
                AnhDD = kh.Anhdd ?? string.Empty,
                TongChiTieu = donHangs.Sum(x => x.Tongtiensanpham),
                TongHoaDon = donHangs.Count(),
                DonHangs = donHangs,
                ChiTieuLonNhat = donHangs.OrderByDescending(x => x.Tongtiensanpham).Select(x => x.Tongtiensanpham).FirstOrDefault(),
                HoatDongGanDay = hoatDongGanDay
            };

            ViewBag.ChiTietKhachHang = chiTiet;
            ViewBag.ReturnURL = GetAbsoluteUri();
            ViewBag.PagedList = donHangs;
            return View();
        }
    }
}