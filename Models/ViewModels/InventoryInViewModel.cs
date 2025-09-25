using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.ViewModels
{
    /// <summary>
    /// ViewModel đại diện cho thông tin nhập kho nguyên liệu.
    /// </summary>
    public class InventoryInViewModel
    {
    }

    /// <summary>
    /// ViewModel dùng để thêm mới thông tin nhập kho nguyên liệu.
    /// </summary>
    public class AddInventoryInViewModel
    {
        /// <summary>
        /// Mã nguyên liệu đã có.
        /// </summary>
        [Display(Name = "Nguồn nguyên liệu đã có")]
        public int IngredientId { get; set; } = -1;

        /// <summary>
        /// Tên nguyên liệu.
        /// </summary>
        [Display(Name = "Tên nguyên liệu")]
        [DataType(DataType.Text)]
        public string IngredientName { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng nhập kho.
        /// </summary>
        [Display(Name = "Số lượng nhập")]
        [DataType(DataType.Text)]
        public int Quantity { get; set; } = 0;

        /// <summary>
        /// Đơn vị tính của nguyên liệu.
        /// </summary>
        [Display(Name = "Đơn vị tính")]
        [DataType(DataType.Text)]
        public string Unit { get; set; } = "cái";

        /// <summary>
        /// Mức đặt hàng lại.
        /// </summary>
        [Display(Name = "Mức đặt hàng lại")]
        [DataType(DataType.Text)]
        public int ReorderLevel { get; set; } = 0;

        /// <summary>
        /// Mô tả nguyên liệu.
        /// </summary>
        [Display(Name = "Mô tả")]
        [DataType(DataType.Text)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Ngày nhập kho.
        /// </summary>
        [Display(Name = "Ngày nhập")]
        [DataType(DataType.Text)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Người nhập kho.
        /// </summary>
        [Display(Name = "Người nhập")]
        [DataType(DataType.Text)]
        public string CreatedBy { get; set; } = string.Empty;
    }
}
