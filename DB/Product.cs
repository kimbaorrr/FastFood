using System;
using System.Collections.Generic;

namespace FastFood.DB;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int? CategoryId { get; set; }

    public int OriginalPrice { get; set; }

    public int? Discount { get; set; }

    public string? Image { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public bool IsApprove { get; set; }

    public int? ApproverId { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public string? Summary { get; set; }

    public string? Content { get; set; }

    public int? FinalPrice { get; set; }

    public virtual Employee? Approver { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
}
