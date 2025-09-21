using System;
using System.Collections.Generic;

namespace FastFood.DB.Entities;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? ThumbnailImage { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Article> ArticleApprovers { get; set; } = new List<Article>();

    public virtual ICollection<Article> ArticleAuthors { get; set; } = new List<Article>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual EmployeeAccount? EmployeeAccount { get; set; }

    public virtual ICollection<InventoryIn> InventoryIns { get; set; } = new List<InventoryIn>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> ProductApprovers { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductCreatedByNavigations { get; set; } = new List<Product>();
}
