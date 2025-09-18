using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{

    public abstract class BasePaymentSummary
    {
        public int TotalProductPrice { get; set; } = 0;
        public int ShippingFee { get; set; } = 20000;
        public int PromoAmount { get; set; } = 0;
        public int PromoId { get; set; } = 0;

        public int TotalPay => TotalProductPrice + ShippingFee - PromoAmount;

        protected BasePaymentSummary() { }

        protected BasePaymentSummary(BasePaymentSummary a)
        {
            TotalProductPrice = a.TotalProductPrice;
            ShippingFee = a.ShippingFee;
            PromoAmount = a.PromoAmount;
            PromoId = a.PromoId;
        }
    }

    public class BaseCustomerInfo
    {
        [Display(Name = "Họ đệm")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Tên khách hàng")]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Địa chỉ giao hàng")]
        [DataType(DataType.Text)]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Thành phố")]
        [DataType(DataType.Text)]
        public string City { get; set; } = string.Empty;

        [Display(Name = "Mã bưu điện")]
        [DataType(DataType.Text)]
        public string PostalCode { get; set; } = string.Empty;

        [Display(Name = "Email")]
        [DataType(DataType.Text)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.Text)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Ghi chú đơn hàng")]
        [DataType(DataType.MultilineText)]
        public string OrderNote { get; set; } = string.Empty;
    }

    public class AddOrderPayment : BasePaymentSummary
    {
        public BaseCustomerInfo Customer { get; set; } = new BaseCustomerInfo();

        public AddOrderPayment() { }

        public AddOrderPayment(AddOrderPayment a) : base()
        {
            TotalProductPrice = a.TotalProductPrice;
            ShippingFee = a.ShippingFee;
            PromoAmount = a.PromoAmount;
            PromoId = a.PromoId;

            Customer = new BaseCustomerInfo()
            {
                FirstName = a.Customer.FirstName,
                LastName = a.Customer.LastName,
                Address = a.Customer.Address,
                City = a.Customer.City,
                PostalCode = a.Customer.PostalCode,
                Email = a.Customer.Email,
                PhoneNumber = a.Customer.PhoneNumber,
                OrderNote = a.Customer.OrderNote
            };
        }
    }

    public class PaymentResult
    {
        [Display(Name = "Mã đơn hàng")]
        [DataType(DataType.Text)]
        public int OrderId { get; set; } = 0;

        [Display(Name = "Tổng thanh toán")]
        [DataType(DataType.Text)]
        public int TotalPayment { get; set; } = 0;

        [Display(Name = "Mã giao dịch")]
        [DataType(DataType.Text)]
        public long TransactionId { get; set; } = 0;

        [Display(Name = "Trạng thái thanh toán")]
        [DataType(DataType.Text)]
        public string PaymentStatus { get; set; } = string.Empty;

        [Display(Name = "Trạng thái giao dịch")]
        [DataType(DataType.Text)]
        public string TransactionStatus { get; set; } = string.Empty;
    }
}
