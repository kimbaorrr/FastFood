using FastFood.DB;
using FastFood.Models;
using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FastFood.Areas.Controllers
{
    [Route("gio-hang")]
    public class CartController : SessionController
    {

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.Title = "Giỏ hàng";
            return View();
        }

        [HttpGet("thong-tin")]
        public string GetItem()
        {
            FastFood_GioHang? gioHang = GetGioHang();

            if (gioHang == null)
                return JsonMessage(message: "Giỏ hàng trống.");

            return JsonConvert.SerializeObject(gioHang!.SanPhamDaChon.Values, Formatting.Indented);
        }

        [HttpPost("thanh-toan")]
        [ValidateAntiForgeryToken]
        public string GetSummaryCheckout([FromForm] string couponCode)
        {
            FastFood_GioHang? gioHang = GetGioHang();

            if (gioHang == null)
                return JsonMessage(message: "Giỏ hàng trống.");

            Makhuyenmai? maKhuyenMai = FastFood_SanPham.getMaKhuyenMai()
                .FirstOrDefault(x => x.Code.Equals(couponCode));
            int soTienGiamKM = 0;
            int idKM = 0;

            if (maKhuyenMai != null)
            {
                if (IsValidCoupon(maKhuyenMai))
                {
                    soTienGiamKM = maKhuyenMai.Sotienduocgiam;
                    idKM = maKhuyenMai.Id;
                }
                else
                {
                    string message = maKhuyenMai.Ngayketthuc.HasValue && DateTime.Now > maKhuyenMai.Ngayketthuc.Value
                        ? "Mã khuyến mãi đã hết hạn sử dụng!"
                        : "Mã khuyến mãi đã hết lượt sử dụng!";
                    return JsonMessage(message: message);
                }
            }

            FastFood_ThanhToan_TomTatThanhToan checkOut = new FastFood_ThanhToan_TomTatThanhToan
            {
                MaKhuyenMai = soTienGiamKM,
                TongTienSanPham = gioHang!.TongTien(),
                IDMaKhuyenMai = idKM
            };
            SetTomTatThanhToan(checkOut);
            return JsonMessage(true, data: checkOut);

        }

        [HttpPost("them-vao-gio")]
        [ValidateAntiForgeryToken]
        public string AddItem([FromForm] int productId, [FromForm] int quantity = 1)
        {
            FastFood_GioHang? gioHang = GetGioHang();

            if (gioHang == null)
                SetGioHang(new FastFood_GioHang());

            gioHang!.ThemVaoGio(productId, quantity);
            SetGioHang(gioHang);
            return JsonMessage(true, message: "Đã thêm vào giỏ");
        }

        [HttpPost("xoa-khoi-gio")]
        [ValidateAntiForgeryToken]
        public string RemoveItem([FromForm] int productId)
        {
            FastFood_GioHang? gioHang = GetGioHang();

            if (gioHang == null)
                return JsonMessage(message: "Giỏ hàng trống.");

            gioHang!.XoaKhoiGio(productId);
            SetGioHang(gioHang);
            return JsonMessage(true, message: "Đã xóa khỏi giỏ");
        }

        [HttpPost("giam-so-luong")]
        public string DecreaseQuantity([FromForm] int productId)
        {
            FastFood_GioHang? gioHang = GetGioHang();
            if (gioHang == null)
                return JsonMessage(message: "Giỏ hàng trống.");

            gioHang!.GiamSoLuong(productId);
            SetGioHang(gioHang);
            return JsonMessage(true, message: "Giảm số lượng thành công");

        }
        [HttpPost("kiem-tra-gio-hang-rong")]
        [ValidateAntiForgeryToken]
        public string IsEmpty()
        {
            FastFood_GioHang? gioHang = GetGioHang();
            return gioHang!.GioHangRong() ? JsonMessage(true, message: "Giỏ hàng hiện đang rỗng !") : JsonMessage();
        }

        private static bool IsValidCoupon(Makhuyenmai maKhuyenMai)
        {
            DateTime now = DateTime.Now;
            bool isExpired = maKhuyenMai.Ngayketthuc.HasValue && now > maKhuyenMai.Ngayketthuc.Value;
            bool isUsable = maKhuyenMai.Luotsudung > 0;
            return !isExpired && isUsable;
        }
    }
}
