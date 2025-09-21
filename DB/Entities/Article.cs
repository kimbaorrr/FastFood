using System;
using System.Collections.Generic;

namespace FastFood.DB.Entities;

public partial class Article
{
    public int ArticleId { get; set; }

    public int AuthorId { get; set; }

    public string Title { get; set; } = null!;

    public string? Summary { get; set; }

    public string? Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Tags { get; set; }

    public bool IsApproved { get; set; }

    public DateTime? ApproveAt { get; set; }

    public int? ApproverId { get; set; }

    public string? CoverImage { get; set; }

    public virtual Employee? Approver { get; set; }

    public virtual Employee Author { get; set; } = null!;
}
