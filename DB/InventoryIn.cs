using System;
using System.Collections.Generic;

namespace FastFood.DB;

public partial class InventoryIn
{
    public int InventoryId { get; set; }

    public int IngredientId { get; set; }

    public int CreatedBy { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual Ingredient Ingredient { get; set; } = null!;
}
