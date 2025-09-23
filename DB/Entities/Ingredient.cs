using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[Table("ingredients")]
[Index("IngredientName", Name = "uq__nguyenli__0b228abdb3fb41d7", IsUnique = true)]
public partial class Ingredient
{
    [Key]
    [Column("ingredient_id")]
    public int IngredientId { get; set; }

    [Column("ingredient_name")]
    [StringLength(50)]
    public string IngredientName { get; set; } = null!;

    [Column("inventory")]
    public int Inventory { get; set; }

    [Column("unit")]
    [StringLength(10)]
    public string Unit { get; set; } = null!;

    [Column("limit_reorder")]
    public int LimitReorder { get; set; }

    [Column("description")]
    [StringLength(100)]
    public string? Description { get; set; }

    [InverseProperty("Ingredient")]
    public virtual ICollection<InventoryIn> InventoryIns { get; set; } = new List<InventoryIn>();

    [InverseProperty("Ingredient")]
    public virtual ICollection<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();
}
