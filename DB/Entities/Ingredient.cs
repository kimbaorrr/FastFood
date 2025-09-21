using System;
using System.Collections.Generic;

namespace FastFood.DB.Entities;

public partial class Ingredient
{
    public int IngredientId { get; set; }

    public string IngredientName { get; set; } = null!;

    public int Inventory { get; set; }

    public string Unit { get; set; } = null!;

    public int LimitReorder { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<InventoryIn> InventoryIns { get; set; } = new List<InventoryIn>();

    public virtual ICollection<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();
}
