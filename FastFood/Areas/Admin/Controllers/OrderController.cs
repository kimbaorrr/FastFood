using FastFood.Areas.Admin.Models;
using FastFood.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{
    public class OrderController : SessionController
    {
        /// <summary>
        /// Hiển thị danh sách các đơn hàng với phân trang.
        /// </summary>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="size">Số lượng đơn hàng trên mỗi trang.</param>
        /// <returns>Trả về View hiển thị danh sách đơn hàng.</returns>
        [HttpGet]
        public ActionResult List(int page = 1, int size = 10)
        {
            X.PagedList.IPagedList<FastFood_DonHang_DanhSachDonHang> donHangs = FastFood_DonHang.getDanhSachDonHang().OrderBy(m => m.MaDonHang).ToPagedList(page, size);
            SortedList<string, int> donHangCards = new SortedList<string, int>();
            donHangCards["DaHuy"] = FastFood_DonHang.getDonHang().Count(x => x.TrangThaiDon == 8);
            donHangCards["DaGiao"] = FastFood_DonHang.getDonHang().Count(x => x.TrangThaiDon == 7);
            donHangCards["DangGiao"] = FastFood_DonHang.getDonHang().Count(x => x.TrangThaiDon == 6);
            donHangCards["ChoGiao"] = FastFood_DonHang.getDonHang().Count(x => x.TrangThaiDon == 5);
            donHangCards["DangThucHien"] = FastFood_DonHang.getDonHang().Count(x => x.TrangThaiDon == 4);
            donHangCards["DaXacNhan"] = FastFood_DonHang.getDonHang().Count(x => x.TrangThaiDon == 3);
            donHangCards["ChoXacNhan"] = FastFood_DonHang.getDonHang().Count(x => x.TrangThaiDon == 2);
            donHangCards["DaThanhToan"] = FastFood_DonHang.getDonHang().Count(x => x.TrangThaiDon == 1);
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
        [HttpGet]
        public ActionResult OrderStatus(int id, int page = 1, int size = 10)
        {
            X.PagedList.IPagedList<FastFood_DonHang_DanhSachDonHang> dsDonHang = FastFood_DonHang.getDanhSachDonHang().Where(x => x.TrangThaiDonHang.MaTrangThai == id).OrderBy(m => m.MaDonHang).ToPagedList(page, size);
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
        [HttpGet]
        public ActionResult Detail(int id)
        {
            if (!FastFood_NhanVienDangNhap.CheckPermission(Session["MaNhanVien"] as string, 2))
                return HttpNotFound();
            DonHang dh = FastFood_DonHang.getDonHang().FirstOrDefault(x => x.MaDonHang == id);
            if (dh == null)
                return HttpNotFound();
            string nguoiGiao = (dh.NhanVien1?.HoDem ?? "") + " " + (dh.NhanVien1?.TenNhanVien ?? "");
            FastFood_DonHang_ChiTietDonHang chiTiet = new FastFood_DonHang_ChiTietDonHang()
            {
                MaDonHang = dh.MaDonHang,
                NgayDat = dh.NgayDat,
                NguoiMua = dh.KhachHang.HoDem + " " + dh.KhachHang.TenKhachHang,
                NguoiGiao = nguoiGiao,
                TongTienSanPham = dh.TongTienSanPham,
                TongThanhToan = dh.TongThanhToan,
                MaTrangThaiThanhToan = dh.ThanhToans.Select(x => x.TrangThaiThanhToan).FirstOrDefault(),
                TrangThaiThanhToan = dh.ThanhToans.Select(x => x.TrangThaiThanhToan).FirstOrDefault() == false ? "Chưa thanh toán" : "Đã thanh toán",
                TongSanPham = dh.ChiTietDonHangs.Count(),
                PhiVanChuyen = dh.PhiVanChuyen,
                PhuongThucVanChuyen = dh.PhuongThucVanChuyen,
                ThoiGianGiaoHangDuKien = dh.ThoiGianGiaoHangDuKien,
                ThoiGianGiaoHangThucTe = dh.ThoiGianGiaoHangThucTe,
                Chitietdonhangs = dh.ChiTietDonHangs,
                Khachhang = dh.KhachHang,
                TrangThaiDonHang = dh.TrangThaiDonHang,
                MaKhuyenMai = dh.MaKhuyenMai1?.SoTienDuocGiam ?? 0,
                MaGiaoDich = dh.ThanhToans.Where(x => x.TrangThaiThanhToan).Select(x => x.MaGiaoDich).FirstOrDefault(),
                NgayThanhToan = dh.ThanhToans.Where(x => x.TrangThaiThanhToan).Select(x => x.NgayThanhToan).FirstOrDefault()
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOrderStatus(int id, int orderStatus, int staffId)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                DonHang dh = e.DonHangs.FirstOrDefault(x => x.MaDonHang == id);
                bool isStaff = e.NhanViens.Any(x => x.MaNhanVien == staffId);
                if (dh == null && !isStaff)
                    return HttpNotFound();
                e.Entry(dh).State = EntityState.Modified;
                dh.TrangThaiDon = orderStatus;
                if (orderStatus == 4)
                    dh.NguoiBan = staffId;
                if (orderStatus == 7)
                    dh.ThoiGianGiaoHangThucTe = DateTime.Now;
                e.SaveChanges();
                return JsonMessage(true, $"Cập nhật trạng thái cho đơn hàng {FastFood_DonHang.getTenTrangThai(orderStatus)} thành công!");
            }
        }
        /// <summary>
        /// Cập nhật thông tin vận chuyển cho đơn hàng.
        /// </summary>
        /// <param name="a">Thông tin vận chuyển mới.</param>
        /// <returns>Trả về thông báo JSON về kết quả cập nhật.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateShippingInfo(FastFood_DonHang_ThemThongTinVanChuyen a)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                DonHang dh = e.DonHangs.FirstOrDefault(x => x.MaDonHang == a.MaDonHang);
                if (dh == null)
                    return HttpNotFound();
                dh.DonViVanChuyen = a.DonViVanChuyen;
                dh.MaVanDon = a.MaVanDon;
                dh.ThoiGianGiaoHangDuKien = DateTime.Today.AddDays(a.SoNgayDuKien);
                dh.TrangThaiDon = 6;
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
        [HttpPost]
        public ActionResult CheckShippingInfo(int id)
        {
            DonHang dh = FastFood_DonHang.getDonHang().FirstOrDefault(x => x.MaDonHang == id);
            if (dh == null)
                return HttpNotFound();
            if (!string.IsNullOrEmpty(dh.DonViVanChuyen) && dh.PhiVanChuyen != 0 && !string.IsNullOrEmpty(dh.MaVanDon))
            {
                return JsonMessage(true, "");
            }
            return JsonMessage(true, "");
        }
    }
}