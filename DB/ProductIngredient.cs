using System;
using System.Collections.Generic;

namespace FastFood.DB;

public partial class ProductIngredient
{
    public int ProductId { get; set; }

    public int IngredientId { get; set; }

    public int QuantityNeeded { get; set; }

    public string Unit { get; set; } = null!;

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
