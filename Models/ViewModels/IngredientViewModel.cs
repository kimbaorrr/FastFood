using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.ViewModels
{
    public abstract class BaseIngredientViewModel
    {
        public int IngredientId { get; set; } = -1;
        public string IngredientName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }

    public class OriginalIngredientViewModel : BaseIngredientViewModel
    {

    }

    public class CustomIngredientViewModel : BaseIngredientViewModel
    {
        public int QuantityNeeded { get; set; } = 0;
        public string Unit { get; set; } = string.Empty;
    }
    

    public class IngredientSubmission
    {
        public Dictionary<string, string> FormData { get; set; } = new Dictionary<string, string>();
        public IEnumerable<SelectedIngredient> SelectedIngredients { get; set; } = Enumerable.Empty<SelectedIngredient>();
    }

    public class SelectedIngredient
    {
        public string IngredientId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public string Unit { get; set; } = "cái";
    }
}
