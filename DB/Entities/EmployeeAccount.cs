using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("employee_accounts")]
[Index("UserName", Name = "uq__nhanvien__55f68fc0955a1e2e", IsUnique = true)]
public partial class EmployeeAccount
{
    [Key]
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    [Column("user_name")]
    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [Column("password")]
    public string Password { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp(3) without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("temporary_password")]
    public bool TemporaryPassword { get; set; }

    [Column("permission")]
    public string? Permission { get; set; }

    [Column("role")]
    public bool Role { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("EmployeeAccount")]
    public virtual Employee Employee { get; set; } = null!;
}
