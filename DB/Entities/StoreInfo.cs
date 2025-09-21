using System;
using System.Collections.Generic;

namespace FastFood.DB.Entities;

public partial class StoreInfo
{
    public string StoreName { get; set; } = null!;

    public string? Slogan { get; set; }

    public string? Address { get; set; }

    public string? Hotline { get; set; }

    public string? Email { get; set; }

    public string? FacebookUrl { get; set; }

    public string? InstagramUrl { get; set; }

    public string? YoutubeUrl { get; set; }

    public string? XUrl { get; set; }

    public string? Logo { get; set; }
}
