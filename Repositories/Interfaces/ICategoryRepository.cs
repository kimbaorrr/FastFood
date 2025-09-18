using FastFood.DB;

namespace FastFood.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories();
        Task<int> CountProductInCategory(int categoryId);
        Task<List<Category>> TakeOrderCategoryByProductQuantity(int take);
        Task<string> GetCategoryName(int categoryId);

    }
}
