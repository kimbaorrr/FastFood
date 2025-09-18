using System.Collections.Specialized;
using System.Web;
using FastFood.DB;
using FastFood.Models;
using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using VNPAY_CS_ASPX;
namespace FastFood.Areas.Controllers
{
    [Route("thanh-toan")]
    public class CheckoutController : SessionController
    {
        private readonly IConfiguration _configuration;
        private string MaKhachHang => Session.GetString("CustomerId") ?? string.Empty;
        private string VNP_HashSecret => _configuration.GetSection("VNPAY").GetValue<string>("vnp_HashSecret") ?? string.Empty;
        private string VNP_TmnCode => _configuration.GetSection("VNPAY").GetValue<string>("vnp_TmnCode") ?? string.Empty;
        private string VNP_Url => _configuration.GetSection("VNPAY").GetValue<string>("vnp_Url") ?? string.Empty;
        private string ClientIPAddress => Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

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

        public CheckoutController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            FastFood_ThanhToan_TomTatThanhToan? tt = GetTomTatThanhToan();
            Khachhang? kh = FastFood_KhachHang.getKhachHang().FirstOrDefault(x => x.Makhachhang.ToString().Equals(MaKhachHang));

            if (kh == null || tt == null)
                return BadRequest();

            FastFood_ThanhToan_ThemDot td = new FastFood_ThanhToan_ThemDot()
            {
                HoDem = kh.Hodem,
                TenKhachHang = kh.Tenkhachhang,
                DiaChiGiaoHang = kh.Diachi ?? string.Empty,
                Email = kh.Email ?? string.Empty,
                SoDienThoai = kh.Sodienthoai ?? string.Empty,
                TongTienSanPham = tt.TongTienSanPham,
                PhiVanChuyen = tt.PhiVanChuyen,
                MaKhuyenMai = tt.MaKhuyenMai,
                IDMaKhuyenMai = tt.IDMaKhuyenMai,
            };
            ViewBag.Title = "Thanh toán";
            return View(td);
        }
        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public IActionResult Index(FastFood_ThanhToan_ThemDot a)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                using (IDbContextTransaction transaction = e.Database.BeginTransaction())
                {
                    try
                    {
                        int soTienDuocGiam = 0;
                        string paymentUrl = string.Empty;
                        FastFood_GioHang? gioHang = GetGioHang();
                        Makhuyenmai? maKM = e.Makhuyenmais.Where(x => x.Id == a.IDMaKhuyenMai).FirstOrDefault();
                        if (maKM != null)
                            soTienDuocGiam = maKM.Sotienduocgiam;

                        int tongThanhToan = a.TongTienSanPham + a.PhiVanChuyen - soTienDuocGiam;
                        Khachhang? kh = e.Khachhangs.FirstOrDefault(x => x.Makhachhang.ToString().Equals(MaKhachHang));
                        if (kh == null || gioHang == null)
                            return BadRequest();

                        kh.Diachi = a.DiaChiGiaoHang;
                        Donhang dh = new Donhang()
                        {
                            Nguoimua = Convert.ToInt32(MaKhachHang),
                            Ngaydat = DateTime.Now,
                            Ghichu = a.GhiChuDonHang,
                            Phivanchuyen = a.PhiVanChuyen,
                            Tongtiensanpham = a.TongTienSanPham,
                            Makhuyenmai = a.IDMaKhuyenMai != 0 ? a.IDMaKhuyenMai : null,
                            Tongthanhtoan = tongThanhToan
                        };
                        e.Donhangs.Add(dh);
                        e.SaveChanges();
                        foreach (FastFood_GioHang_DonHangCuaToi item in gioHang.SanPhamDaChon.Values)
                        {
                            Chitietdonhang ct = new Chitietdonhang()
                            {
                                Madonhang = dh.Madonhang,
                                Masanpham = item.MaSanPham,
                                Soluong = item.SoLuong
                            };
                            e.Chitietdonhangs.Add(ct);
                        }
                        Thanhtoan tt = new Thanhtoan()
                        {
                            Madonhang = dh.Madonhang,
                            Trangthaithanhtoan = false,
                            Ngaythanhtoan = DateTime.Now
                        };
                        e.Thanhtoans.Add(tt);
                        e.SaveChanges();

                        paymentUrl = SendToVNPay(dh.Madonhang, tongThanhToan, dh.Ngaydat);

                        if (!string.IsNullOrEmpty(paymentUrl))
                        {
                            transaction.Commit();
                            return Redirect(paymentUrl);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return View();
                    }
                }
            }
            return View();

        }

        [HttpGet("ket-qua")]
        public IActionResult PaymentResult()
        {
            ViewBag.Title = "Kết quả thanh toán";

            if (Request.QueryString.HasValue)
            {
                NameValueCollection vnpayData = HttpUtility.ParseQueryString(Request.QueryString.Value);
                VnPayLibrary vnpay = new VnPayLibrary();
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]!);
                    }
                }

                int vnp_TxnRef = Convert.ToInt32(vnpay.GetResponseData("vnp_TxnRef"));
                int vnp_Amount = Convert.ToInt32(vnpay.GetResponseData("vnp_Amount")) / 100;
                long vnp_TransactionNo = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                string vnp_SecureHash = Request.QueryString.Value.Split("=").LastOrDefault() ?? string.Empty;

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, VNP_HashSecret);

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

                        Thanhtoan? tt = e.Thanhtoans.FirstOrDefault(x => x.Madonhang == vnp_TxnRef);
                        if (tt == null)
                            return BadRequest();

                        tt.Magiaodich = vnp_TransactionNo;
                        if (vnp_Amount == tt.MadonhangNavigation.Tongthanhtoan && !tt.Trangthaithanhtoan && vnp_ResponseCode.Equals("00") && vnp_TransactionStatus.Equals("00"))
                        {
                            tt.Trangthaithanhtoan = true;
                            e.SaveChanges();
                        }
                        return View(kq);
                    }
                }
            }
            return BadRequest();
        }

        private string SendToVNPay(int maDonHang, int tongThanhToan, DateTime ngayTao)
        {
            string scheme = Request.Scheme;
            string host = Request.Host.ToString();
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Amount", (tongThanhToan * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", ngayTao.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_IpAddr", ClientIPAddress);
            vnpay.AddRequestData("vnp_TxnRef", maDonHang.ToString());
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {maDonHang}");
            vnpay.AddRequestData("vnp_ReturnUrl", Url.Action("PaymentResult", "Checkout", new { }, scheme, host)!);
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", VNP_TmnCode);
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderType", "other");

            string paymentUrl = vnpay.CreateRequestUrl(VNP_Url, VNP_HashSecret);
            return paymentUrl;
        }
    }
}