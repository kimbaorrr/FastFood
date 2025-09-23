using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("feedbacks")]
public partial class Feedback
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("customer_name")]
    [StringLength(100)]
    public string? CustomerName { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("phone")]
    [StringLength(10)]
    public string? Phone { get; set; }

    [Column("content")]
    [StringLength(255)]
    public string? Content { get; set; }
}
