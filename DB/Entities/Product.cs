using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("products")]
public partial class Product
{
    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("product_name")]
    [StringLength(100)]
    public string ProductName { get; set; } = null!;

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [Column("original_price")]
    public int OriginalPrice { get; set; }

    [Column("discount")]
    public int? Discount { get; set; }

    [Column("image")]
    public string? Image { get; set; }

    [Column("created_at", TypeName = "timestamp(3) without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp(3) without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("created_by")]
    public int? CreatedBy { get; set; }

    [Column("is_approve")]
    public bool IsApprove { get; set; }

    [Column("approver_id")]
    public int? ApproverId { get; set; }

    [Column("approved_at", TypeName = "timestamp(3) without time zone")]
    public DateTime? ApprovedAt { get; set; }

    [Column("summary")]
    [StringLength(100)]
    public string? Summary { get; set; }

    [Column("content")]
    [StringLength(255)]
    public string? Content { get; set; }

    [Column("final_price")]
    public int? FinalPrice { get; set; }

    [ForeignKey("ApproverId")]
    [InverseProperty("ProductApprovers")]
    public virtual Employee? Approver { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category? Category { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProductCreatedByNavigations")]
    public virtual Employee? CreatedByNavigation { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
}
