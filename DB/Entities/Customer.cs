using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("customers")]
[Index("Email", Name = "uq__khachhan__a9d10534018c4d92", IsUnique = true)]
public partial class Customer
{
    [Key]
    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("last_name")]
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Column("first_name")]
    [StringLength(15)]
    public string FirstName { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("phone")]
    [StringLength(12)]
    public string? Phone { get; set; }

    [Column("address")]
    [StringLength(100)]
    public string? Address { get; set; }

    [Column("thumbnail_image")]
    public string? ThumbnailImage { get; set; }

    [Column("created_at", TypeName = "timestamp(3) without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("bod")]
    public DateOnly? Bod { get; set; }

    [InverseProperty("Customer")]
    public virtual CustomerAccount? CustomerAccount { get; set; }

    [InverseProperty("BuyerNavigation")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Customer")]
    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
}
