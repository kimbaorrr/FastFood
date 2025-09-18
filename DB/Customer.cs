using System;
using System.Collections.Generic;

namespace FastFood.DB;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? ThumbnailImage { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateOnly? Bod { get; set; }

    public virtual CustomerAccount? CustomerAccount { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
}
