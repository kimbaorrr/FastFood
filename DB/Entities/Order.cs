using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("orders")]
[Index("PromoId", Name = "fki_fk_promo_id_promo_id")]
public partial class Order
{
    [Key]
    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("buyer")]
    public int Buyer { get; set; }

    [Column("seller")]
    public int? Seller { get; set; }

    [Column("order_date", TypeName = "timestamp(3) without time zone")]
    public DateTime OrderDate { get; set; }

    [Column("note")]
    [StringLength(255)]
    public string? Note { get; set; }

    [Column("order_status")]
    public int OrderStatus { get; set; }

    [Column("estimated_delivery_time")]
    public DateOnly? EstimatedDeliveryTime { get; set; }

    [Column("shipping_method")]
    [StringLength(100)]
    public string? ShippingMethod { get; set; }

    [Column("shipping_fee")]
    public int ShippingFee { get; set; }

    [Column("total_price")]
    public int TotalPrice { get; set; }

    [Column("shipping_unit")]
    [StringLength(100)]
    public string? ShippingUnit { get; set; }

    [Column("shipping_id")]
    [StringLength(50)]
    public string? ShippingId { get; set; }

    [Column("actual_delivery_time", TypeName = "timestamp(3) without time zone")]
    public DateTime? ActualDeliveryTime { get; set; }

    [Column("promo_id")]
    public int? PromoId { get; set; }

    [Column("total_pay")]
    public int? TotalPay { get; set; }

    [Column("shipper_name")]
    [StringLength(100)]
    public string? ShipperName { get; set; }

    [ForeignKey("Buyer")]
    [InverseProperty("Orders")]
    public virtual Customer BuyerNavigation { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("OrderStatus")]
    [InverseProperty("Orders")]
    public virtual OrdersStatus OrderStatusNavigation { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("PromoId")]
    [InverseProperty("Orders")]
    public virtual Promo? Promo { get; set; }

    [ForeignKey("Seller")]
    [InverseProperty("Orders")]
    public virtual Employee? SellerNavigation { get; set; }
}
