using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("permissions")]
[Index("Description", Name = "uq__quyenhan__24b0ca9edb8a3f03", IsUnique = true)]
public partial class Permission
{
    [Key]
    [Column("permission_id")]
    public int PermissionId { get; set; }

    [Column("description")]
    [StringLength(50)]
    public string Description { get; set; } = null!;

    [Column("created_at", TypeName = "timestamp(3) without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("color")]
    [StringLength(50)]
    public string? Color { get; set; }
}
