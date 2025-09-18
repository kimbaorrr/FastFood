using FastFood.DB;
using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    public class HomeController : SessionController
    {
        /// <summary>
        /// Hiển thị trang chủ với danh sách đơn hàng theo phân trang.
        /// </summary>
        /// <param name="page">Số trang hiện tại, mặc định là 1</param>
        /// <param name="size">Số lượng đơn hàng trên mỗi trang, mặc định là 10</param>
        /// <returns>Trang chủ với danh sách đơn hàng</returns>
        [HttpGet("")]
        public IActionResult Index([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            ViewBag.Title = "Trang chủ";
            IPagedList<Donhang> donHang = FastFood_DonHang.getDonHang().OrderByDescending(m => m.Madonhang).ToPagedList(page, size);
            ViewBag.DonHang = donHang;
            ViewBag.CurrentPage = donHang.PageNumber;
            ViewBag.TotalPages = donHang.PageCount;
            return View();

        }
        /// <summary>
        /// Lấy doanh thu theo thời gian (theo loại: ngày, tháng, năm).
        /// </summary>
        /// <param name="type">Loại thời gian (ngày, tháng, năm)</param>
        /// <returns>Doanh thu theo loại thời gian</returns>
        [HttpGet("doanh-thu-theo-thoi-gian")]
        public string DoanhThuTheoThoiGian([FromQuery] string type)
        {
            return FastFood_DonHang.getDoanhThu(type);

        }
        /// <summary>
        /// Lấy thông tin các sản phẩm bán chạy nhất trong tuần.
        /// </summary>
        /// <param name="take">Số sản phẩm muốn lấy</param>
        /// <returns>Danh sách các sản phẩm bán chạy nhất trong tuần</returns>
        [HttpGet("san-pham-ban-chay")]
        public string TopSanPhamBanChay([FromQuery] int take)
        {
            return FastFood_DonHang.getSanPhamBanChayNhatTuan(take);
        }
        /// <summary>
        /// Lấy thông tin đơn hàng trong tuần.
        /// </summary>
        /// <returns>Danh sách đơn hàng trong tuần</returns>
        [HttpGet("don-hang-trong-tuan")]
        public string DonHangTrongTuan()
        {
            return FastFood_DonHang.getDonHangTrongTuan();
        }




    }
}