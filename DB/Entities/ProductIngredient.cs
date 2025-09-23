using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DB.Entities;

[PrimaryKey("ProductId", "IngredientId")]
[Table("product_ingredients")]
public partial class ProductIngredient
{
    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Key]
    [Column("ingredient_id")]
    public int IngredientId { get; set; }

    [Column("quantity_needed")]
    public int QuantityNeeded { get; set; }

    [Column("unit")]
    [StringLength(10)]
    public string Unit { get; set; } = null!;

    [ForeignKey("IngredientId")]
    [InverseProperty("ProductIngredients")]
    public virtual Ingredient Ingredient { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("ProductIngredients")]
    public virtual Product Product { get; set; } = null!;
}
