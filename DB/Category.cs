using System;
using System.Collections.Generic;

namespace FastFood.DB;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public string? BackgroundImage { get; set; }

    public string? ThumbnailImage { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
