using System;
using System.Collections.Generic;

namespace FastFood.DB.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public int Buyer { get; set; }

    public int? Seller { get; set; }

    public DateTime OrderDate { get; set; }

    public string? Note { get; set; }

    public int OrderStatus { get; set; }

    public DateOnly? EstimatedDeliveryTime { get; set; }

    public string? ShippingMethod { get; set; }

    public int ShippingFee { get; set; }

    public int TotalPrice { get; set; }

    public string? ShippingUnit { get; set; }

    public string? ShippingId { get; set; }

    public DateTime? ActualDeliveryTime { get; set; }

    public int? PromoId { get; set; }

    public int? TotalPay { get; set; }

    public string? ShipperName { get; set; }

    public virtual Customer BuyerNavigation { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual OrdersStatus OrderStatusNavigation { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Promo? PromoCodeNavigation { get; set; }

    public virtual Employee? SellerNavigation { get; set; }
}
