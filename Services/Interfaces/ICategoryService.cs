using FastFood.DB.Entities;
using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace FastFood.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IPagedList<Category>> GetCategoriesPagedList(int page, int size);
        Task<(bool, string)> NewCategory(NewCategoryViewModel newCategoryViewModel);
        Task<(bool, string)> DeleteCategory(int categoryId);
        Task<(bool, string)> DeleteCategories(int[] categoryIds);
        Task<(bool, string)> EditCategory(EditCategoryViewModel editCategoryViewModel);
        Task<SelectList> GetCustomCategoriesSelectList();
    }
}
