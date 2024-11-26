using FastFood.DB;
using FastFood.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using VNPAY_CS_ASPX;
namespace FastFood.Controllers
{
    [RoutePrefix("thanh-toan")]
    public class CheckoutController : SessionController
    {
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            int maKhachHang = Convert.ToInt32(Session["KH_MaKhachHang"] as string);
            KhachHang kh = FastFood_KhachHang.getKhachHang().FirstOrDefault(x => x.MaKhachHang == maKhachHang);
            if (kh == null)
                return HttpNotFound();
            FastFood_ThanhToan_TomTatThanhToan checkOut = Session["TomTatThanhToan"] as FastFood_ThanhToan_TomTatThanhToan;
            FastFood_ThanhToan_ThemDot tt = new FastFood_ThanhToan_ThemDot()
            {
                HoDem = kh.HoDem,
                TenKhachHang = kh.TenKhachHang,
                DiaChiGiaoHang = kh.DiaChi,
                Email = kh.Email,
                SoDienThoai = kh.SoDienThoai,
                TongTienSanPham = checkOut.TongTienSanPham,
                PhiVanChuyen = checkOut.PhiVanChuyen,
                MaKhuyenMai = checkOut.MaKhuyenMai,
                IDMaKhuyenMai = checkOut.IDMaKhuyenMai,
                MaNguoiMua = kh.MaKhachHang
            };
            ViewBag.Title = "Thanh toán";
            return View(tt);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("")]
        public ActionResult Index(FastFood_ThanhToan_ThemDot a)
        {
            FastFood_GioHang gioHang = Session["GioHang"] as FastFood_GioHang;
            string paymentUrl = string.Empty;
            using (FastFoodEntities e = new FastFoodEntities())
            {
                using (DbContextTransaction transaction = e.Database.BeginTransaction())
                {
                    try
                    {
                        int soTienDuocGiam = e.MaKhuyenMais.Where(x => x.id == a.IDMaKhuyenMai).Select(x => x.SoTienDuocGiam).FirstOrDefault();
                        int tongThanhToan = a.TongTienSanPham + a.PhiVanChuyen - soTienDuocGiam;
                        KhachHang kh = e.KhachHangs.FirstOrDefault(x => x.MaKhachHang == a.MaNguoiMua);
                        if (kh == null)
                            return HttpNotFound();
                        kh.DiaChi = a.DiaChiGiaoHang;
                        DonHang dh = new DonHang()
                        {
                            NguoiMua = a.MaNguoiMua,
                            NgayDat = DateTime.Now,
                            GhiChu = a.GhiChuDonHang,
                            PhiVanChuyen = a.PhiVanChuyen,
                            TongTienSanPham = a.TongTienSanPham,
                            MaKhuyenMai = a.IDMaKhuyenMai != 0 ? a.IDMaKhuyenMai : (int?)null
                        };
                        e.DonHangs.Add(dh);
                        e.SaveChanges();
                        foreach (FastFood_GioHang_DonHangCuaToi item in gioHang.SanPhamDaChon.Values)
                        {
                            ChiTietDonHang ct = new ChiTietDonHang()
                            {
                                MaDonHang = dh.MaDonHang,
                                MaSanPham = item.MaSanPham,
                                SoLuong = item.SoLuong
                            };
                            e.ChiTietDonHangs.Add(ct);
                        }
                        ThanhToan tt = new ThanhToan()
                        {
                            MaDonHang = dh.MaDonHang,
                            TrangThaiThanhToan = false,
                            NgayThanhToan = DateTime.Now
                        };
                        e.ThanhToans.Add(tt);
                        e.SaveChanges();

                        paymentUrl = SendToVNPay(dh.MaDonHang, tongThanhToan, dh.NgayDat);
                        transaction.Commit();
                        return Redirect(paymentUrl);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return View();
                    }
                }
            }
        }

        [HttpGet]
        [Route("ket-qua")]
        public ActionResult PaymentResult()
        {
            ViewBag.Title = "Kết quả thanh toán";

            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret key
                System.Collections.Specialized.NameValueCollection vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }

                int vnp_TxnRef = Convert.ToInt32(vnpay.GetResponseData("vnp_TxnRef"));
                int vnp_Amount = Convert.ToInt32(vnpay.GetResponseData("vnp_Amount")) / 100;
                long vnp_TransactionNo = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    using (FastFoodEntities e = new FastFoodEntities())
                    {
                        FastFood_ThanhToan_KetQua kq = new FastFood_ThanhToan_KetQua()
                        {
                            MaDonHang = vnp_TxnRef,
                            TongThanhToan = vnp_Amount,
                            MaGiaoDich = vnp_TransactionNo,
                            TrangThaiGiaoDich = responseCodeMessages[vnp_ResponseCode],
                            TrangThaiThanhToan = transactionStatusMessages[vnp_TransactionStatus]
                        };
                        ThanhToan tt = e.ThanhToans.FirstOrDefault(x => x.MaDonHang == vnp_TxnRef);
                        if (tt == null)
                            return HttpNotFound();
                        tt.MaGiaoDich = vnp_TransactionNo;
                        if (vnp_Amount == tt.DonHang.TongThanhToan && !tt.TrangThaiThanhToan && vnp_ResponseCode.Equals("00") && vnp_TransactionStatus.Equals("00"))
                        {
                            tt.TrangThaiThanhToan = true;
                            e.SaveChanges();
                        }
                        return View(kq);
                    }
                }
            }
            return HttpNotFound();
        }

        private string SendToVNPay(int maDonHang, int tongThanhToan, DateTime ngayTao)
        {
            VnPayLibrary vnpay = new VnPayLibrary();

            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"];
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"];
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];

            vnpay.AddRequestData("vnp_Amount", (tongThanhToan * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", ngayTao.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_TxnRef", maDonHang.ToString());
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {maDonHang}");
            vnpay.AddRequestData("vnp_ReturnUrl", $"{Request.Url.Scheme}://{Request.Url.Host}:{Request.Url.Port}{Url.Action("PaymentResult", "Checkout")}");
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderType", "other");

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return paymentUrl;
        }

        private readonly Dictionary<string, string> responseCodeMessages = new Dictionary<string, string>
            {
                { "00", "Giao dịch thành công" },
                { "07", "Trừ tiền thành công. Giao dịch bị nghi ngờ (liên quan tới lừa đảo, giao dịch bất thường)." },
                { "09", "Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng chưa đăng ký dịch vụ InternetBanking tại ngân hàng." },
                { "10", "Giao dịch không thành công do: Khách hàng xác thực thông tin thẻ/tài khoản không đúng quá 3 lần" },
                { "11", "Giao dịch không thành công do: Đã hết hạn chờ thanh toán. Xin quý khách vui lòng thực hiện lại giao dịch." },
                { "12", "Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng bị khóa." },
                { "13", "Giao dịch không thành công do Quý khách nhập sai mật khẩu xác thực giao dịch (OTP). Xin quý khách vui lòng thực hiện lại giao dịch." },
                { "24", "Giao dịch không thành công do: Khách hàng hủy giao dịch" },
                { "51", "Giao dịch không thành công do: Tài khoản của quý khách không đủ số dư để thực hiện giao dịch." },
                { "65", "Giao dịch không thành công do: Tài khoản của Quý khách đã vượt quá hạn mức giao dịch trong ngày." },
                { "75", "Ngân hàng thanh toán đang bảo trì." },
                { "79", "Giao dịch không thành công do: KH nhập sai mật khẩu thanh toán quá số lần quy định. Xin quý khách vui lòng thực hiện lại giao dịch" },
                { "99", "Các lỗi khác (lỗi còn lại, không có trong danh sách mã lỗi đã liệt kê)" }
            };

        private readonly Dictionary<string, string> transactionStatusMessages = new Dictionary<string, string>
            {
                { "00", "Giao dịch thành công" },
                { "01", "Giao dịch chưa hoàn tất" },
                { "02", "Giao dịch bị lỗi" },
                { "04", "Giao dịch đảo (Khách hàng đã bị trừ tiền tại Ngân hàng nhưng GD chưa thành công ở VNPAY)" },
                { "05", "VNPAY đang xử lý giao dịch này (GD hoàn tiền)" },
                { "06", "VNPAY đã gửi yêu cầu hoàn tiền sang Ngân hàng (GD hoàn tiền)" },
                { "07", "Giao dịch bị nghi ngờ gian lận" },
                { "09", "GD Hoàn trả bị từ chối" }
            };


    }
}