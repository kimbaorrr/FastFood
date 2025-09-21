using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories();
        Task<int> CountProductInCategory(int categoryId);
        Task<List<Category>> TakeOrderCategoryByProductQuantity(int take);
        Task<string> GetCategoryName(int categoryId);
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task<Category> GetCategoryById(int? categoryId);
        Task DeleteCategory(Category category);

    }
}
