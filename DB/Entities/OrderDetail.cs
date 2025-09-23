using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[PrimaryKey("OrderId", "ProductId")]
[Table("order_details")]
public partial class OrderDetail
{
    [Key]
    [Column("order_id")]
    public int OrderId { get; set; }

    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("total_price")]
    public int TotalPrice { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("OrderDetails")]
    public virtual Product Product { get; set; } = null!;
}
