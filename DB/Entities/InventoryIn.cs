using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("inventory_in")]
public partial class InventoryIn
{
    [Key]
    [Column("inventory_id")]
    public int InventoryId { get; set; }

    [Column("ingredient_id")]
    public int IngredientId { get; set; }

    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("created_at", TypeName = "timestamp(3) without time zone")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InventoryIns")]
    public virtual Employee CreatedByNavigation { get; set; } = null!;

    [ForeignKey("IngredientId")]
    [InverseProperty("InventoryIns")]
    public virtual Ingredient Ingredient { get; set; } = null!;
}
