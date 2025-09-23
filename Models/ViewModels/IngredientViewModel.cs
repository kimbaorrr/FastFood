using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.ViewModels
{
    /// <summary>
    /// Lớp cơ sở cho ViewModel nguyên liệu.
    /// </summary>
    public abstract class BaseIngredientViewModel
    {
        /// <summary>
        /// Mã nguyên liệu.
        /// </summary>
        public int IngredientId { get; set; } = -1;

        /// <summary>
        /// Tên nguyên liệu.
        /// </summary>
        public string IngredientName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả nguyên liệu.
        /// </summary>
        public string? Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel cho nguyên liệu gốc.
    /// </summary>
    public class OriginalIngredientViewModel : BaseIngredientViewModel
    {

    }

    /// <summary>
    /// ViewModel cho nguyên liệu tuỳ chỉnh.
    /// </summary>
    public class CustomIngredientViewModel : BaseIngredientViewModel
    {
        /// <summary>
        /// Số lượng cần thiết của nguyên liệu.
        /// </summary>
        public int QuantityNeeded { get; set; } = 0;

        /// <summary>
        /// Đơn vị tính của nguyên liệu.
        /// </summary>
        public string Unit { get; set; } = string.Empty;
    }
    
    /// <summary>
    /// Lớp đại diện cho dữ liệu gửi lên liên quan đến nguyên liệu.
    /// </summary>
    public class IngredientSubmission
    {
        /// <summary>
        /// Dữ liệu biểu mẫu gửi lên.
        /// </summary>
        public Dictionary<string, string> FormData { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Danh sách nguyên liệu được chọn.
        /// </summary>
        public IEnumerable<SelectedIngredient> SelectedIngredients { get; set; } = Enumerable.Empty<SelectedIngredient>();
    }

    /// <summary>
    /// Lớp đại diện cho nguyên liệu đã chọn.
    /// </summary>
    public class SelectedIngredient
    {
        /// <summary>
        /// Mã nguyên liệu.
        /// </summary>
        public string IngredientId { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng nguyên liệu.
        /// </summary>
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Đơn vị tính của nguyên liệu.
        /// </summary>
        public string Unit { get; set; } = "cái";
    }
}
