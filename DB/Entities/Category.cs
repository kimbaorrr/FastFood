using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("categories")]
[Index("CategoryName", Name = "uq__danhmuc__650cae4e957f6e3a", IsUnique = true)]
public partial class Category
{
    [Key]
    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("category_name")]
    [StringLength(100)]
    public string CategoryName { get; set; } = null!;

    [Column("description")]
    [StringLength(255)]
    public string? Description { get; set; }

    [Column("created_at", TypeName = "timestamp(3) without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("created_by")]
    public int? CreatedBy { get; set; }

    [Column("background_image")]
    public string? BackgroundImage { get; set; }

    [Column("thumbnail_image")]
    public string? ThumbnailImage { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("Categories")]
    public virtual Employee? CreatedByNavigation { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
