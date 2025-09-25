using FastFood.DB.Entities;
using FastFood.Models.ViewModels;
using X.PagedList;

namespace FastFood.Services
{
    /// <summary>
    /// Interface cho các dịch vụ quản lý bài viết.
    /// </summary>
    public interface IArticleService
    {
        /// <summary>
        /// Tạo mới một bài viết.
        /// </summary>
        /// <param name="newArticleViewModel">Model chứa thông tin bài viết mới.</param>
        /// <returns>Kết quả thực hiện và thông báo.</returns>
        Task<(bool, string)> NewArticle(NewArticleViewModel newArticleViewModel, int employeeId);

        /// <summary>
        /// Lấy danh sách bài viết phân trang.
        /// </summary>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="size">Số lượng bài viết mỗi trang.</param>
        /// <returns>Danh sách bài viết phân trang.</returns>
        Task<IPagedList<Article>> GetArticlesPagedList(int page, int size);

        /// <summary>
        /// Xóa một bài viết theo mã.
        /// </summary>
        /// <param name="articleId">Mã bài viết cần xóa.</param>
        /// <returns>Kết quả thực hiện và thông báo.</returns>
        Task<(bool, string)> DeleteArticle(int articleId);

        /// <summary>
        /// Xóa nhiều bài viết theo danh sách mã.
        /// </summary>
        /// <param name="articleIds">Danh sách mã bài viết cần xóa.</param>
        /// <returns>Kết quả thực hiện và thông báo.</returns>
        Task<(bool, string)> DeleteArticles(int[] articleIds);

        /// <summary>
        /// Duyệt bài viết.
        /// </summary>
        /// <param name="articleId">Mã bài viết cần duyệt.</param>
        /// <param name="approverId">Mã người duyệt.</param>
        /// <param name="isApproved">Trạng thái duyệt.</param>
        /// <returns>Kết quả thực hiện và thông báo.</returns>
        Task<(bool, string)> ApproveArticle(int articleId, int? approverId, bool isApproved);

        /// <summary>
        /// Duyệt nhiều bài viết.
        /// </summary>
        /// <param name="articleIds">Danh sách mã bài viết cần duyệt.</param>
        /// <param name="approverId">Mã người duyệt.</param>
        /// <param name="isApproved">Trạng thái duyệt.</param>
        /// <returns>Kết quả thực hiện và thông báo.</returns>
        Task<(bool, string)> ApproveArticles(int[] articleIds, int? approverId, bool isApproved);

        /// <summary>
        /// Lấy danh sách bài viết chưa được duyệt.
        /// </summary>
        /// <returns>Danh sách bài viết chưa duyệt.</returns>
        Task<List<Article>> GetArticlesNotApprove();

        /// <summary>
        /// Chỉnh sửa thông tin bài viết.
        /// </summary>
        /// <param name="editArticleViewModel">Model chứa thông tin bài viết cần chỉnh sửa.</param>
        /// <returns>Kết quả thực hiện và thông báo.</returns>
        Task<(bool, string)> EditArticle(EditArticleViewModel editArticleViewModel);

        /// <summary>
        /// Lấy danh sách bài viết theo trạng thái duyệt, có phân trang.
        /// </summary>
        /// <param name="isApproved">Trạng thái duyệt.</param>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="size">Số lượng bài viết mỗi trang.</param>
        /// <returns>Danh sách bài viết phân trang theo trạng thái duyệt.</returns>
        Task<IPagedList<Article>> GetArticlesByApproveStatusPagedList(bool isApproved, int page, int size);

        /// <summary>
        /// Lấy thông tin chi tiết bài viết và các bài viết liên quan.
        /// </summary>
        /// <param name="articleId">Mã bài viết cần lấy thông tin.</param>
        /// <returns>Model bài viết tùy chỉnh.</returns>
        Task<CustomArticleViewModel> GetCustomArticleViewModel(int articleId);
    }
}