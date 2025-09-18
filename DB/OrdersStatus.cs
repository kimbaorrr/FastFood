using System;
using System.Collections.Generic;

namespace FastFood.DB;

public partial class OrdersStatus
{
    public int Id { get; set; }

    public string? StatusName { get; set; }

    public string? Description { get; set; }

    public string? Color { get; set; }

    public int? Progress { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
