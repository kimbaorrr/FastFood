using System;
using System.Collections.Generic;

namespace FastFood.DB;

public partial class CustomerAccount
{
    public int CustomerId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
