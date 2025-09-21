using FastFood.DB.Entities;

namespace FastFood.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts();
        Task<Product> GetProductById(int productId);
        Task<Product> GetProductByName(string productName);
        Task<List<Product>> GetProductsByApproveStatus(bool isApproved);
        Task<List<Product>> GetProductsByCategory(bool isApproved, int categoryId, int take);
        Task<List<Product>> GetRandomProducts(bool isApproved, int excludedProductId, int take);
        Task<List<Product>> GetProductsByOrderDiscount();
        Task<List<Product>> GetProductsByTopSale(int take);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(Product product);
        Task<bool> IsProductHasOrder(int productId);
    }
}
