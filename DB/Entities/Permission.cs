using System;
using System.Collections.Generic;

namespace FastFood.DB.Entities;

public partial class Permission
{
    public int PermissionId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string? Color { get; set; }
}
