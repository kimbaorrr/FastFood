using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("articles")]
[Index("Title", Name = "uq__baiviet__371689aaa357a80b", IsUnique = true)]
public partial class Article
{
    [Key]
    [Column("article_id")]
    public int ArticleId { get; set; }

    [Column("author_id")]
    public int AuthorId { get; set; }

    [Column("title")]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    [Column("summary")]
    [StringLength(255)]
    public string? Summary { get; set; }

    [Column("content")]
    public string? Content { get; set; }

    [Column("created_at", TypeName = "timestamp(3) without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp(3) without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("tags")]
    [StringLength(255)]
    public string? Tags { get; set; }

    [Column("is_approved")]
    public bool IsApproved { get; set; }

    [Column("approve_at", TypeName = "timestamp(3) without time zone")]
    public DateTime? ApproveAt { get; set; }

    [Column("approver_id")]
    public int? ApproverId { get; set; }

    [Column("cover_image")]
    public string? CoverImage { get; set; }

    [ForeignKey("ApproverId")]
    [InverseProperty("ArticleApprovers")]
    public virtual Employee? Approver { get; set; }

    [ForeignKey("AuthorId")]
    [InverseProperty("ArticleAuthors")]
    public virtual Employee Author { get; set; } = null!;
}
