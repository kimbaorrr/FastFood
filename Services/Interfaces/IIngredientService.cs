using FastFood.Models.ViewModels;

namespace FastFood.Services.Interfaces
{
    /// <summary>
    /// Dịch vụ quản lý nguyên liệu.
    /// </summary>
    public interface IIngredientService
    {
        /// <summary>
        /// Lấy danh sách nguyên liệu gốc.
        /// </summary>
        /// <returns>Danh sách các nguyên liệu gốc dưới dạng ViewModel.</returns>
        Task<List<OriginalIngredientViewModel>> GetOriginalIngredientViewModels();

        /// <summary>
        /// Lấy danh sách nguyên liệu tuỳ chỉnh theo sản phẩm.
        /// </summary>
        /// <param name="productID">ID của sản phẩm.</param>
        /// <returns>Danh sách các nguyên liệu tuỳ chỉnh dưới dạng ViewModel.</returns>
        Task<List<CustomIngredientViewModel>> GetCustomIngredientViewModels(int productID);
    }
}