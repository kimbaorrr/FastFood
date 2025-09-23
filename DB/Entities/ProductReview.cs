using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[PrimaryKey("CustomerId", "ProductId")]
[Table("product_reviews")]
public partial class ProductReview
{
    [Key]
    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("star_rating")]
    public int? StarRating { get; set; }

    [Column("review_content")]
    [StringLength(255)]
    public string? ReviewContent { get; set; }

    [Column("created_at", TypeName = "timestamp(3) without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp(3) without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("ProductReviews")]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("ProductReviews")]
    public virtual Product Product { get; set; } = null!;
}
