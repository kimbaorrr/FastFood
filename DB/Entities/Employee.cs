using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("employees")]
[Index("Email", Name = "uq_email", IsUnique = true)]
public partial class Employee
{
    [Key]
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Column("last_name")]
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Column("first_name")]
    [StringLength(15)]
    public string FirstName { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("phone")]
    [StringLength(20)]
    public string? Phone { get; set; }

    [Column("address")]
    [StringLength(100)]
    public string? Address { get; set; }

    [Column("thumbnail_image")]
    public string? ThumbnailImage { get; set; }

    [Column("created_at", TypeName = "timestamp(3) without time zone")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("Approver")]
    public virtual ICollection<Article> ArticleApprovers { get; set; } = new List<Article>();

    [InverseProperty("Author")]
    public virtual ICollection<Article> ArticleAuthors { get; set; } = new List<Article>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    [InverseProperty("Employee")]
    public virtual EmployeeAccount? EmployeeAccount { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<InventoryIn> InventoryIns { get; set; } = new List<InventoryIn>();

    [InverseProperty("SellerNavigation")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Approver")]
    public virtual ICollection<Product> ProductApprovers { get; set; } = new List<Product>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Product> ProductCreatedByNavigations { get; set; } = new List<Product>();
}
