using System.ComponentModel.DataAnnotations;
using FastFood.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace FastFood.Models.ViewModels
{
    /// <summary>
    /// ViewModel đại diện cho đơn hàng.
    /// </summary>
    public class OrderViewModel
    {

    }

    /// <summary>
    /// ViewModel cho sản phẩm bán chạy nhất.
    /// </summary>
    public class TopSellingProductViewModel
    {
        /// <summary>
        /// Mã sản phẩm.
        /// </summary>
        public int ProductId { get; set; } = 0;

        /// <summary>
        /// Tên sản phẩm.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng sản phẩm đã bán.
        /// </summary>
        public int QuantitySoldProduct { get; set; } = 0;
    }

    /// <summary>
    /// ViewModel cho đơn hàng tuỳ chỉnh.
    /// </summary>
    public class CustomOrderViewModel
    {
        /// <summary>
        /// Mã đơn hàng.
        /// </summary>
        public int OrderId { get; set; } = -1;

        /// <summary>
        /// Ngày đặt hàng.
        /// </summary>
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// Người mua.
        /// </summary>
        public string Buyer { get; set; } = string.Empty;

        /// <summary>
        /// Người bán.
        /// </summary>
        public string Seller { get; set; } = string.Empty;

        /// <summary>
        /// Tổng giá trị đơn hàng.
        /// </summary>
        public int TotalPrice { get; set; } = 0;

        /// <summary>
        /// Tổng số tiền thanh toán.
        /// </summary>
        public int? TotalPay { get; set; } = 0;

        /// <summary>
        /// Trạng thái đơn hàng.
        /// </summary>
        public OrdersStatus OrderStatuses { get; set; } = new OrdersStatus();

        /// <summary>
        /// Đã hoàn thành thanh toán hay chưa.
        /// </summary>
        public bool? IsPaymentCompleted { get; set; }

        /// <summary>
        /// Văn bản trạng thái thanh toán.
        /// </summary>
        public string PaymentStatusText { get; set; } = string.Empty;

        /// <summary>
        /// Tổng số sản phẩm trong đơn hàng.
        /// </summary>
        public int TotalProduct { get; set; } = 0;

    }

    /// <summary>
    /// ViewModel chi tiết cho đơn hàng tuỳ chỉnh.
    /// </summary>
    public class CustomOrderDetailViewModel : CustomOrderViewModel
    {
        /// <summary>
        /// Danh sách chi tiết đơn hàng.
        /// </summary>
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        /// <summary>
        /// Thông tin khách hàng.
        /// </summary>
        public Customer Customer { get; set; } = new Customer();

        /// <summary>
        /// Phí vận chuyển.
        /// </summary>
        public int ShippingFee { get; set; } = 0;

        /// <summary>
        /// Tên người giao hàng.
        /// </summary>
        public string ShipperName { get; set; } = string.Empty;

        /// <summary>
        /// Thời gian dự kiến giao hàng.
        /// </summary>
        public DateOnly? EstimatedDeliveryTime { get; set; }

        /// <summary>
        /// Thời gian giao hàng thực tế.
        /// </summary>
        public DateTime? ActualDeliveryTime { get; set; }

        /// <summary>
        /// Phương thức vận chuyển.
        /// </summary>
        public string ShippingMethod { get; set; } = string.Empty;

        /// <summary>
        /// Mã thanh toán.
        /// </summary>
        public int PaymentId { get; set; } = -1;

        /// <summary>
        /// Số tiền giảm giá.
        /// </summary>
        public int DiscountAmount { get; set; } = 0;

        /// <summary>
        /// Mã giao dịch thanh toán.
        /// </summary>
        public long? TransactionId { get; set; }

        /// <summary>
        /// Ngày thanh toán.
        /// </summary>
        public DateTime? PaymentDate { get; set; }
    }

    /// <summary>
    /// ViewModel cho thông tin vận chuyển mới.
    /// </summary>
    public class NewShippingInfo
    {
        /// <summary>
        /// Mã đơn hàng.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Đơn vị vận chuyển.
        /// </summary>
        [Display(Name = "Đơn vị vận chuyển")]
        [DataType(DataType.Text)]
        public string ShippingUnit { get; set; } = string.Empty;

        /// <summary>
        /// Mã vận đơn.
        /// </summary>
        [Display(Name = "Mã vận đơn")]
        [DataType(DataType.Text)]
        public string ShippingId { get; set; } = string.Empty;

        /// <summary>
        /// Số ngày dự kiến giao hàng.
        /// </summary>
        [Display(Name = "Số ngày dự kiến")]
        [DataType(DataType.Text)]
        public int EstimateDay { get; set; } = 0;
    }

}
