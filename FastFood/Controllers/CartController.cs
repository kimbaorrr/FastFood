using FastFood.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace FastFood.Controllers
{
    [RoutePrefix("gio-hang")]
    public class CartController : SessionController
    {
        [Route("")]
        public ActionResult Index()
        {
            ViewBag.Title = "Giỏ hàng";
            return View();
        }

        [HttpGet]
        [Route("thong-tin")]
        public ActionResult GetItem()
        {
            if (!(Session["GioHang"] is FastFood_GioHang gioHang))
                return Json(new { success = false, message = "Giỏ hàng trống." });

            return Json(gioHang.SanPhamDaChon.Values, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("thanh-toan")]
        public ActionResult GetSummaryCheckout(string couponCode)
        {
            if (!(Session["GioHang"] is FastFood_GioHang gioHang))
                return Json(new { success = false, message = "Giỏ hàng trống." });

            DB.MaKhuyenMai maKhuyenMai = FastFood_SanPham.getMaKhuyenMai()
                .FirstOrDefault(x => x.Code.Equals(couponCode, StringComparison.OrdinalIgnoreCase));
            int soTienGiamKM = 0, idKM = 0;

            if (maKhuyenMai != null)
            {
                if (IsValidCoupon(maKhuyenMai))
                {
                    soTienGiamKM = maKhuyenMai.SoTienDuocGiam;
                    idKM = maKhuyenMai.id;
                }
                else
                {
                    string message = maKhuyenMai.NgayKetThuc.HasValue && DateTime.Now > maKhuyenMai.NgayKetThuc.Value
                        ? "Mã khuyến mãi đã hết hạn sử dụng!"
                        : "Mã khuyến mãi đã hết lượt sử dụng!";
                    return JsonMessage(false, message);
                }
            }

            FastFood_ThanhToan_TomTatThanhToan checkOut = new FastFood_ThanhToan_TomTatThanhToan
            {
                MaKhuyenMai = soTienGiamKM,
                TongTienSanPham = gioHang.TongTien(),
                IDMaKhuyenMai = idKM
            };
            Session["TomTatThanhToan"] = checkOut;
            return Json(new { data = checkOut, success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("them-vao-gio")]
        public ActionResult AddItem(int productId, int quantity)
        {
            if (!(Session["GioHang"] is FastFood_GioHang gioHang))
            {
                gioHang = new FastFood_GioHang();
                Session["GioHang"] = gioHang;
            }

            gioHang.ThemVaoGio(productId, quantity);
            return JsonMessage(true, "Đã thêm vào giỏ");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("xoa-khoi-gio")]
        public ActionResult RemoveItem(int productId)
        {
            if (Session["GioHang"] is FastFood_GioHang gioHang)
            {
                gioHang.XoaKhoiGio(productId);
                return JsonMessage(true, "Đã xóa khỏi giỏ");
            }
            return JsonMessage(false, "Giỏ hàng trống.");
        }

        [HttpPost]
        [Route("giam-so-luong")]
        public ActionResult DecreaseQuantity(int productId)
        {
            if (Session["GioHang"] is FastFood_GioHang gioHang)
            {
                gioHang.GiamSoLuong(productId);
                return JsonMessage(true, "Giảm số lượng thành công");
            }
            return JsonMessage(false, "Giỏ hàng trống.");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("kiem-tra-gio-hang-rong")]
        public ActionResult IsEmpty()
        {
            FastFood_GioHang gioHang = Session["GioHang"] as FastFood_GioHang;
            return gioHang.GioHangRong() ? JsonMessage(false, "Giỏ hàng hiện đang rỗng !") : JsonMessage(true, "");
        }

        private static bool IsValidCoupon(DB.MaKhuyenMai maKhuyenMai)
        {
            DateTime now = DateTime.Now;
            bool isExpired = maKhuyenMai.NgayKetThuc.HasValue && now > maKhuyenMai.NgayKetThuc.Value;
            bool isUsable = maKhuyenMai.LuotSuDung > 0;
            return !isExpired && isUsable;
        }
    }
}
