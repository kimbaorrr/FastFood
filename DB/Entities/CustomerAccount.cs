using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("customer_accounts")]
[Index("UserName", Name = "uq__khachhan__55f68fc01bca21a4", IsUnique = true)]
public partial class CustomerAccount
{
    [Key]
    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("user_name")]
    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [Column("password")]
    public string Password { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp(3) without time zone")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("CustomerAccount")]
    public virtual Customer Customer { get; set; } = null!;
}
