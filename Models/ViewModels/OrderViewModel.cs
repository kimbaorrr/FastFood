using System.ComponentModel.DataAnnotations;
using FastFood.DB;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace FastFood.Models.ViewModels
{
    public class OrderViewModel
    {


    }

    public class TopSellingProductViewModel
    {
        public int MaSanPham { get; set; } = 0;
        public string TenSanPham { get; set; } = string.Empty;
        public int SoLuongDaBan { get; set; } = 0;
    }

    public class CustomOrderViewModel
    {
        public int OrderId { get; set; } = -1;
        public DateTime? OrderDate { get; set; }
        public string Buyer { get; set; } = string.Empty;
        public string Seller { get; set; } = string.Empty;
        public int TotalPrice { get; set; } = 0;
        public int? TotalPay { get; set; } = 0;
        public OrdersStatus OrderStatuses { get; set; } = new OrdersStatus();
        public bool? IsPaymentCompleted { get; set; }
        public string PaymentStatusText { get; set; } = string.Empty;
        public int TotalProduct { get; set; } = 0;

    }

    public class CustomOrderDetailViewModel : CustomOrderViewModel
    {
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public Customer Customer { get; set; } = new Customer();
        public int ShippingFee { get; set; } = 0;
        public string ShipperName { get; set; } = string.Empty;
        public DateOnly? EstimatedDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public string ShippingMethod { get; set; } = string.Empty;
        public int PaymentId { get; set; } = -1;
        public int PromoId { get; set; } = -1;
        public long? TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
    }

    public class NewShippingInfo
    {
        public int OrderId { get; set; }
        [Display(Name = "Đơn vị vận chuyển")]
        [DataType(DataType.Text)]
        public string ShippingUnit { get; set; } = string.Empty;
        [Display(Name = "Mã vận đơn")]
        [DataType(DataType.Text)]
        public string shippingId { get; set; } = string.Empty;
        [Display(Name = "Số ngày dự kiến")]
        [DataType(DataType.Text)]
        public int EstimateDay { get; set; } = 0;
    }

}
