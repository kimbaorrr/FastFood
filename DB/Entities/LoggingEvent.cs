using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("logging_events")]
public partial class LoggingEvent
{
    [Key]
    [Column("event_id")]
    public int EventId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("user_type")]
    public bool? UserType { get; set; }

    [Column("user_name")]
    [StringLength(50)]
    public string? UserName { get; set; }

    [Column("ip_address")]
    [StringLength(50)]
    public string? IpAddress { get; set; }

    [Column("device")]
    [StringLength(100)]
    public string? Device { get; set; }

    [Column("browser_agent")]
    [StringLength(100)]
    public string? BrowserAgent { get; set; }

    [Column("accessed_time", TypeName = "timestamp(3) without time zone")]
    public DateTime? AccessedTime { get; set; }

    [Column("event_detail")]
    [StringLength(255)]
    public string? EventDetail { get; set; }
}
