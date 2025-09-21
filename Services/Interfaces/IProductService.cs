using FastFood.DB.Entities;
using FastFood.Models.ViewModels;
using X.PagedList;

namespace FastFood.Services.Interfaces
{
    public interface IProductService
    {
        Task<IPagedList<Product>> GetProductsByApproveStatusPagedList(bool isApproved, int page, int size);

        Task<(bool, string)> AddProduct(NewProductPostViewModel newProductPostViewModel);

        Task<(bool, string, ProductDetailViewModel, string)> GetProductDetailViewModel(int productId, int? employeeId);

        Task<(bool, string)> EditProductDetail(ProductDetailPostViewModel productDetailPostViewModel);
        Task<(bool, string)> DeleteProducts(int[] productIds);
        Task<(bool, string)> DeleteProduct(int productId);
        Task<(bool, string)> ApproveProduct(int? approverId, int productId, bool isApproved);
        Task<(bool, string)> ApproveProducts(int? approverId, int[] productIds, bool isApproved);
    }
}
