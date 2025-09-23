using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    /// <summary>
    /// Lớp cơ sở cho ViewModel thanh toán.
    /// </summary>
    public abstract class BasePaymentViewModel
    {
        /// <summary>
        /// Tổng giá trị sản phẩm.
        /// </summary>
        public int TotalProductPrice { get; set; } = 0;

        /// <summary>
        /// Phí vận chuyển.
        /// </summary>
        public int ShippingFee { get; set; } = 20000;

        /// <summary>
        /// Số tiền khuyến mãi.
        /// </summary>
        public int PromoAmount { get; set; } = 0;

        /// <summary>
        /// Mã khuyến mãi.
        /// </summary>
        public string PromoCode { get; set; } = string.Empty;

        /// <summary>
        /// Id khuyến mãi.
        /// </summary>
        public int? PromoId { get; set; }

        /// <summary>
        /// Tổng số tiền thanh toán.
        /// </summary>
        public int TotalPay { get; set; } = 0;
    }

    /// <summary>
    /// ViewModel tóm tắt thanh toán.
    /// </summary>
    public class PaymentSummaryViewModel : BasePaymentViewModel
    {

    }

    /// <summary>
    /// ViewModel thông tin khách hàng.
    /// </summary>
    public class CustomerInfoViewModel
    {
        /// <summary>
        /// Họ đệm của khách hàng.
        /// </summary>
        [Display(Name = "Họ đệm")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Tên khách hàng.
        /// </summary>
        [Display(Name = "Tên khách hàng")]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Họ tên đầy đủ của khách hàng.
        /// </summary>
        public string FullName => $"{this.LastName} {this.FirstName}";

        /// <summary>
        /// Địa chỉ giao hàng.
        /// </summary>
        [Display(Name = "Địa chỉ giao hàng")]
        [DataType(DataType.Text)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Thành phố.
        /// </summary>
        [Display(Name = "Thành phố")]
        [DataType(DataType.Text)]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Mã bưu điện.
        /// </summary>
        [Display(Name = "Mã bưu điện")]
        [DataType(DataType.Text)]
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// Email khách hàng.
        /// </summary>
        [Display(Name = "Email")]
        [DataType(DataType.Text)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Số điện thoại khách hàng.
        /// </summary>
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.Text)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Ghi chú đơn hàng.
        /// </summary>
        [Display(Name = "Ghi chú đơn hàng")]
        [DataType(DataType.MultilineText)]
        public string OrderNote { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel thêm mới thanh toán.
    /// </summary>
    public class AddPaymentViewModel : BasePaymentViewModel
    {
        /// <summary>
        /// Thông tin khách hàng.
        /// </summary>
        public CustomerInfoViewModel Customer { get; set; } = new();
    }

    /// <summary>
    /// ViewModel kết quả thanh toán.
    /// </summary>
    public class PaymentResultViewModel
    {
        /// <summary>
        /// Mã đơn hàng.
        /// </summary>
        [Display(Name = "Mã đơn hàng")]
        [DataType(DataType.Text)]
        public int OrderId { get; set; } = 0;

        /// <summary>
        /// Tổng số tiền thanh toán.
        /// </summary>
        [Display(Name = "Tổng thanh toán")]
        [DataType(DataType.Text)]
        public int TotalPay { get; set; } = 0;

        /// <summary>
        /// Mã giao dịch.
        /// </summary>
        [Display(Name = "Mã giao dịch")]
        [DataType(DataType.Text)]
        public long TransactionId { get; set; } = 0;

        /// <summary>
        /// Trạng thái thanh toán.
        /// </summary>
        [Display(Name = "Trạng thái thanh toán")]
        [DataType(DataType.Text)]
        public string PaymentStatus { get; set; } = string.Empty;

        /// <summary>
        /// Trạng thái giao dịch.
        /// </summary>
        [Display(Name = "Trạng thái giao dịch")]
        [DataType(DataType.Text)]
        public string TransactionStatus { get; set; } = string.Empty;

        /// <summary>
        /// Phương thức thanh toán.
        /// </summary>
        [Display(Name = "Phương thức thanh toán")]
        [DataType(DataType.Text)]
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
