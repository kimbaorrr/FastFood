using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("payments")]
public partial class Payment
{
    [Key]
    [Column("payment_id")]
    public int PaymentId { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("payment_status")]
    public bool PaymentStatus { get; set; }

    [Column("created_at", TypeName = "timestamp(3) without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("transaction_id")]
    public long? TransactionId { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Payments")]
    public virtual Order Order { get; set; } = null!;
}
