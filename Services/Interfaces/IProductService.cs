using FastFood.DB.Entities;
using FastFood.Models.ViewModels;
using X.PagedList;

namespace FastFood.Services.Interfaces
{
    /// <summary>
    /// Dịch vụ quản lý sản phẩm.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Lấy danh sách sản phẩm theo trạng thái duyệt và phân trang.
        /// </summary>
        /// <param name="isApproved">Trạng thái duyệt sản phẩm.</param>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="size">Số lượng sản phẩm trên mỗi trang.</param>
        /// <returns>Danh sách sản phẩm phân trang.</returns>
        Task<IPagedList<Product>> GetProductsByApproveStatusPagedList(bool isApproved, int page, int size);

        /// <summary>
        /// Thêm sản phẩm mới.
        /// </summary>
        /// <param name="newProductPostViewModel">Thông tin sản phẩm mới.</param>
        /// <returns>Tuple gồm kết quả thêm và thông báo.</returns>
        Task<(bool, string)> AddProduct(NewProductPostViewModel newProductPostViewModel);

        /// <summary>
        /// Lấy chi tiết sản phẩm.
        /// </summary>
        /// <param name="productId">ID sản phẩm.</param>
        /// <param name="employeeId">ID nhân viên (nếu có).</param>
        /// <returns>Tuple gồm kết quả, thông báo, chi tiết sản phẩm và trạng thái duyệt.</returns>
        Task<(bool, string, ProductDetailViewModel, string)> GetProductDetailViewModel(int productId, int? employeeId);

        /// <summary>
        /// Chỉnh sửa chi tiết sản phẩm.
        /// </summary>
        /// <param name="productDetailPostViewModel">Thông tin chi tiết sản phẩm cần chỉnh sửa.</param>
        /// <returns>Tuple gồm kết quả chỉnh sửa và thông báo.</returns>
        Task<(bool, string)> EditProductDetail(ProductDetailPostViewModel productDetailPostViewModel);

        /// <summary>
        /// Xóa nhiều sản phẩm.
        /// </summary>
        /// <param name="productIds">Danh sách ID sản phẩm cần xóa.</param>
        /// <returns>Tuple gồm kết quả xóa và thông báo.</returns>
        Task<(bool, string)> DeleteProducts(int[] productIds);

        /// <summary>
        /// Xóa một sản phẩm.
        /// </summary>
        /// <param name="productId">ID sản phẩm cần xóa.</param>
        /// <returns>Tuple gồm kết quả xóa và thông báo.</returns>
        Task<(bool, string)> DeleteProduct(int productId);

        /// <summary>
        /// Duyệt một sản phẩm.
        /// </summary>
        /// <param name="approverId">ID người duyệt (nếu có).</param>
        /// <param name="productId">ID sản phẩm cần duyệt.</param>
        /// <param name="isApproved">Trạng thái duyệt.</param>
        /// <returns>Tuple gồm kết quả duyệt và thông báo.</returns>
        Task<(bool, string)> ApproveProduct(int? approverId, int productId, bool isApproved);

        /// <summary>
        /// Duyệt nhiều sản phẩm.
        /// </summary>
        /// <param name="approverId">ID người duyệt (nếu có).</param>
        /// <param name="productIds">Danh sách ID sản phẩm cần duyệt.</param>
        /// <param name="isApproved">Trạng thái duyệt.</param>
        /// <returns>Tuple gồm kết quả duyệt và thông báo.</returns>
        Task<(bool, string)> ApproveProducts(int? approverId, int[] productIds, bool isApproved);

        /// <summary>
        /// Lấy chi tiết sản phẩm tuỳ chỉnh.
        /// </summary>
        /// <param name="productId">ID sản phẩm.</param>
        /// <returns>Chi tiết sản phẩm tuỳ chỉnh.</returns>
        Task<CustomProductDetailViewModel> GetCustomProductDetailViewModel(int productId);
    }
}
