using FastFood.DB.Entities;
using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace FastFood.Services.Interfaces
{
    /// <summary>
    /// Interface cho các dịch vụ quản lý danh mục sản phẩm.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Lấy danh sách danh mục sản phẩm phân trang.
        /// </summary>
        Task<IPagedList<Category>> GetCategoriesPagedList(int page, int size);

        /// <summary>
        /// Tạo mới một danh mục sản phẩm.
        /// </summary>
        Task<(bool, string)> NewCategory(NewCategoryViewModel newCategoryViewModel);

        /// <summary>
        /// Xóa một danh mục sản phẩm theo mã.
        /// </summary>
        Task<(bool, string)> DeleteCategory(int categoryId);

        /// <summary>
        /// Xóa nhiều danh mục sản phẩm theo danh sách mã.
        /// </summary>
        Task<(bool, string)> DeleteCategories(int[] categoryIds);

        /// <summary>
        /// Chỉnh sửa thông tin danh mục sản phẩm.
        /// </summary>
        Task<(bool, string)> EditCategory(EditCategoryViewModel editCategoryViewModel);

        /// <summary>
        /// Lấy danh sách danh mục sản phẩm cho SelectList.
        /// </summary>
        Task<SelectList> GetCustomCategoriesSelectList();

        /// <summary>
        /// Lấy danh sách view model danh mục sản phẩm tùy chỉnh.
        /// </summary>
        Task<List<CustomCategoryViewModel>> GetCustomCategoryViewModel();
        /// <summary>
        /// Lấy danh mục theo số lượng sản phẩm, sắp xếp giảm dần.
        /// </summary>
        /// <returns></returns>
        Task<List<Category>> GetCategoriesOrderByProductCount(int take);
    }
}
