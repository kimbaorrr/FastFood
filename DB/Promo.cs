using System;
using System.Collections.Generic;

namespace FastFood.DB;

public partial class Promo
{
    public int PromoId { get; set; }

    public string PromoCode { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int Usage { get; set; }

    public int DiscountAmount { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
