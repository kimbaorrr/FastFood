using System;
using System.Collections.Generic;

namespace FastFood.DB;

public partial class Feedback
{
    public int Id { get; set; }

    public string? CustomerName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Content { get; set; }
}
