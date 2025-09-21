using System;
using System.Collections.Generic;

namespace FastFood.DB.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public bool PaymentStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long? TransactionId { get; set; }

    public virtual Order Order { get; set; } = null!;
}
