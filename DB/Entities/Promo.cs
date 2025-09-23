using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("promos")]
public partial class Promo
{
    [Key]
    [Column("promo_id")]
    public int PromoId { get; set; }

    [Column("promo_code")]
    [StringLength(20)]
    public string PromoCode { get; set; } = null!;

    [Column("start_time", TypeName = "timestamp(3) without time zone")]
    public DateTime StartTime { get; set; }

    [Column("end_time", TypeName = "timestamp(3) without time zone")]
    public DateTime? EndTime { get; set; }

    [Column("usage")]
    public int Usage { get; set; }

    [Column("discount_amount")]
    public int DiscountAmount { get; set; }

    [InverseProperty("Promo")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
