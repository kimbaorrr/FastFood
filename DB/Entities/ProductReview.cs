using System;
using System.Collections.Generic;

namespace FastFood.DB.Entities;

public partial class ProductReview
{
    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public int? StarRating { get; set; }

    public string? ReviewContent { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
