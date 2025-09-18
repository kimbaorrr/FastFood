using System;
using System.Collections.Generic;

namespace FastFood.DB;

public partial class EmployeeAccount
{
    public int EmployeeId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public bool TemporaryPassword { get; set; }

    public string? Permission { get; set; }

    public bool Role { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
