using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("orders_status")]
[Index("StatusName", Name = "uq__trangtha__9489ef66db060174", IsUnique = true)]
public partial class OrdersStatus
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("status_name")]
    [StringLength(50)]
    public string? StatusName { get; set; }

    [Column("description")]
    [StringLength(50)]
    public string? Description { get; set; }

    [Column("color")]
    [StringLength(50)]
    public string? Color { get; set; }

    [Column("progress")]
    public int? Progress { get; set; }

    [InverseProperty("OrderStatusNavigation")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
