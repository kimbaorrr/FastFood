using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("store_info")]
public partial class StoreInfo
{
    [Key]
    [Column("store_name")]
    [StringLength(50)]
    public string StoreName { get; set; } = null!;

    [Column("slogan")]
    [StringLength(100)]
    public string? Slogan { get; set; }

    [Column("address")]
    [StringLength(100)]
    public string? Address { get; set; }

    [Column("hotline")]
    [StringLength(12)]
    public string? Hotline { get; set; }

    [Column("email")]
    [StringLength(50)]
    public string? Email { get; set; }

    [Column("facebook_url")]
    [StringLength(100)]
    public string? FacebookUrl { get; set; }

    [Column("instagram_url")]
    [StringLength(100)]
    public string? InstagramUrl { get; set; }

    [Column("youtube_url")]
    [StringLength(100)]
    public string? YoutubeUrl { get; set; }

    [Column("x_url")]
    [StringLength(100)]
    public string? XUrl { get; set; }

    [Column("logo")]
    public string? Logo { get; set; }
}
