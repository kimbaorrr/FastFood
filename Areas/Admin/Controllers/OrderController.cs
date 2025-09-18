using FastFood.DB;
using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/don-hang")]
    public class OrderController : SessionController
    {
        /// <summary>
        /// Hiển thị danh sách các đơn hàng với phân trang.
        /// </summary>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="size">Số lượng đơn hàng trên mỗi trang.</param>
        /// <returns>Trả về View hiển thị danh sách đơn hàng.</returns>
        [HttpGet("danh-sach-don-hang")]
        public IActionResult List([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            IPagedList<FastFood_DonHang_DanhSachDonHang> donHangs = FastFood_DonHang.getDanhSachDonHang().OrderBy(m => m.MaDonHang).ToPagedList(page, size);
            SortedList<string, int> donHangCards = new SortedList<string, int>();
            string[] trangThaiDon = new string[]
            {
                    "DaThanhToan",
                    "ChoXacNhan",
                    "DaXacNhan",
                    "DangThucHien",
                    "ChoGiao",
                    "DangGiao",
                    "DaGiao",
                    "DaHuy"
            };
            byte i = 0;
            foreach (string trangThai in trangThaiDon)
            {
                donHangCards[trangThai] = FastFood_DonHang.getDonHang().Count(x => x.Trangthaidon == i + 1);
                i++;
            }
            ViewBag.DonHang = donHangs;
            ViewBag.DonHangCards = donHangCards;
            ViewBag.CurrentPage = donHangs.PageNumber;
            ViewBag.TotalPages = donHangs.PageCount;
            ViewBag.Title = "Tất cả đơn hàng";
            return View();
        }

        /// <summary>
        /// Hiển thị danh sách đơn hàng theo trạng thái của đơn hàng.
        /// </summary>
        /// <param name="id">ID trạng thái đơn hàng.</param>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="size">Số lượng đơn hàng trên mỗi trang.</param>
        /// <returns>Trả về View hiển thị đơn hàng theo trạng thái.</returns>
        [HttpGet("danh-sach-theo-trang-thai-don/{id}")]
        public IActionResult OrderStatus(int id, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            IPagedList<FastFood_DonHang_DanhSachDonHang> dsDonHang = FastFood_DonHang.getDanhSachDonHang().Where(x => x.TrangThaiDonHang.Matrangthai == id).OrderBy(m => m.MaDonHang).ToPagedList(page, size);
            ViewBag.DonHang = dsDonHang;
            ViewBag.Title = $"Đơn hàng {FastFood_DonHang.getTenTrangThai(id).ToLower()}";
            ViewBag.MaTrangThai = id;
            return View();

        }
        /// <summary>
        /// Hiển thị chi tiết thông tin đơn hàng theo ID.
        /// </summary>
        /// <param name="id">ID của đơn hàng.</param>
        /// <returns>Trả về View chi tiết của đơn hàng.</returns>
        [HttpGet("chi-tiet-don-hang/{id}")]
        public IActionResult Detail(int id)
        {
            if (!FastFood_NhanVienDangNhap.CheckPermission(this.MaNhanVien, 2))
                return BadRequest();


            Donhang? dh = FastFood_DonHang.getDonHang()
                .Include(x => x.Thanhtoans)
                .Include(x => x.Chitietdonhangs)
                .FirstOrDefault(x => x.Madonhang == id);
            if (dh == null)
                return BadRequest();

            FastFood_DonHang_ChiTietDonHang chiTiet = new FastFood_DonHang_ChiTietDonHang()
            {
                MaDonHang = dh.Madonhang,
                NgayDat = dh.Ngaydat,
                NguoiMua = dh.NguoimuaNavigation.Hodem + " " + dh.NguoimuaNavigation.Tenkhachhang,
                NguoiGiao = dh.Nguoigiao ?? string.Empty,
                TongTienSanPham = dh.Tongtiensanpham,
                TongThanhToan = dh.Tongthanhtoan,
                MaTrangThaiThanhToan = dh.Thanhtoans.Select(x => x.Trangthaithanhtoan).FirstOrDefault(),
                TrangThaiThanhToan = dh.Thanhtoans.Select(x => x.Trangthaithanhtoan).FirstOrDefault() ? "Đã thanh toán" : "Chưa thanh toán",
                TongSanPham = dh.Chitietdonhangs.Count,
                PhiVanChuyen = dh.Phivanchuyen,
                PhuongThucVanChuyen = dh.Phuongthucvanchuyen ?? string.Empty,
                ThoiGianGiaoHangDuKien = dh.Thoigiangiaohangdukien,
                ThoiGianGiaoHangThucTe = dh.Thoigiangiaohangthucte,
                Chitietdonhangs = dh.Chitietdonhangs,
                Khachhang = dh.NguoimuaNavigation,
                TrangThaiDonHang = dh.TrangthaidonNavigation,
                MaKhuyenMai = dh.MakhuyenmaiNavigation?.Sotienduocgiam ?? 0,
                MaGiaoDich = dh.Thanhtoans.Where(x => x.Trangthaithanhtoan).Select(x => x.Magiaodich).FirstOrDefault(),
                NgayThanhToan = dh.Thanhtoans.Where(x => x.Trangthaithanhtoan).Select(x => x.Ngaythanhtoan).FirstOrDefault()
            };
            ViewBag.ChiTiet = chiTiet;
            return View();


        }
        /// <summary>
        /// Cập nhật trạng thái đơn hàng.
        /// </summary>
        /// <param name="id">ID của đơn hàng.</param>
        /// <param name="orderStatus">Trạng thái mới của đơn hàng.</param>
        /// <param name="staffId">ID của nhân viên thực hiện.</param>
        /// <returns>Trả về thông báo JSON về kết quả cập nhật.</returns>
        [HttpPost("cap-nhat-trang-thai-don")]
        [ValidateAntiForgeryToken]
        public string UpdateOrderStatus([FromForm] int id, [FromForm] int orderStatus)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                Donhang? dh = e.Donhangs.FirstOrDefault(x => x.Madonhang == id);

                if (dh == null)
                    return JsonMessage(message: "ID không hợp lệ !");

                e.Entry(dh).State = EntityState.Modified;

                dh.Trangthaidon = orderStatus;

                switch (orderStatus)
                {
                    case 4:
                        dh.Nguoiban = Convert.ToInt32(this.MaNhanVien);
                        break;
                    case 7:
                        dh.Thoigiangiaohangthucte = DateTime.Now;
                        break;
                }
                e.SaveChanges();

                return JsonMessage(true, $"Cập nhật trạng thái cho đơn hàng {FastFood_DonHang.getTenTrangThai(orderStatus)} thành công!");
            }
        }
        /// <summary>
        /// Cập nhật thông tin vận chuyển cho đơn hàng.
        /// </summary>
        /// <param name="a">Thông tin vận chuyển mới.</param>
        /// <returns>Trả về thông báo JSON về kết quả cập nhật.</returns>
        [HttpPost("cap-nhat-thong-tin-van-chuyen")]
        [ValidateAntiForgeryToken]
        public string UpdateShippingInfo([FromForm] FastFood_DonHang_ThemThongTinVanChuyen a)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                Donhang? dh = e.Donhangs.FirstOrDefault(x => x.Madonhang == a.MaDonHang);
                if (dh == null)
                    return JsonMessage(message: "ID đơn hàng không hợp lệ !");

                dh.Donvivanchuyen = a.DonViVanChuyen;
                dh.Mavandon = a.MaVanDon;
                dh.Thoigiangiaohangdukien = DateOnly.FromDateTime(DateTime.Now).AddDays(a.SoNgayDuKien);
                dh.Trangthaidon = 6;

                e.Entry(dh).State = EntityState.Modified;
                e.SaveChanges();

                return JsonMessage(true, "Cập nhật thông tin vận chuyển thành công!");
            }

        }
        /// <summary>
        /// Kiểm tra thông tin vận chuyển của đơn hàng.
        /// </summary>
        /// <param name="id">ID của đơn hàng.</param>
        /// <returns>Trả về thông tin vận chuyển của đơn hàng dưới dạng JSON.</returns>
        [HttpPost("thong-tin-van-chuyen")]
        public string CheckShippingInfo([FromForm] int id)
        {
            Donhang? dh = FastFood_DonHang.getDonHang().FirstOrDefault(x => x.Madonhang == id);
            if (dh == null)
                return JsonMessage(message: "ID đơn hàng không hợp lệ !");

            if (!string.IsNullOrEmpty(dh.Donvivanchuyen) && dh.Phivanchuyen != 0 && !string.IsNullOrEmpty(dh.Mavandon))
                return JsonMessage(true);

            return JsonMessage();

        }
    }
}