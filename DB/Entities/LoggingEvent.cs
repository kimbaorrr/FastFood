using System;
using System.Collections.Generic;

namespace FastFood.DB.Entities;

public partial class LoggingEvent
{
    public int EventId { get; set; }

    public int UserId { get; set; }

    public bool? UserType { get; set; }

    public string? UserName { get; set; }

    public string? IpAddress { get; set; }

    public string? Device { get; set; }

    public string? BrowserAgent { get; set; }

    public DateTime? AccessedTime { get; set; }

    public string? EventDetail { get; set; }
}
