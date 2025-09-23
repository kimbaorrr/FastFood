using FastFood.DB.Entities;
using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.ViewModels
{
    /// <summary>
    /// ViewModel cơ sở cho bài viết.
    /// </summary>
    public abstract class BaseArticleViewModel
    {
        /// <summary>
        /// Tiêu đề bài viết.
        /// </summary>
        [Display(Name = "Tiêu đề")]
        [DataType(DataType.Text)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả ngắn của bài viết.
        /// </summary>
        [Display(Name = "Mô tả ngắn")]
        [DataType(DataType.Text)]
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// Nội dung bài viết.
        /// </summary>
        [Display(Name = "Nội dung")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel cho việc tạo mới bài viết.
    /// </summary>
    public class NewArticleViewModel : BaseArticleViewModel
    {
        /// <summary>
        /// Mã tác giả bài viết.
        /// </summary>
        public int AuthorId { get; set; } = -1;

        /// <summary>
        /// Ảnh đại diện của bài viết.
        /// </summary>
        [Display(Name = "Ảnh đại diện")]
        [DataType(DataType.Text)]
        public FormFile? CoverImage { get; set; }
    }

    /// <summary>
    /// ViewModel cho việc chỉnh sửa bài viết.
    /// </summary>
    public class EditArticleViewModel : BaseArticleViewModel
    {
        /// <summary>
        /// Mã bài viết.
        /// </summary>
        public int ArticleId { get; set; } = -1;
    }

    /// <summary>
    /// ViewModel tuỳ chỉnh cho bài viết, bao gồm bài viết và danh sách bài viết gần đây.
    /// </summary>
    public class CustomArticleViewModel
    {
        /// <summary>
        /// Thông tin bài viết.
        /// </summary>
        public Article Article { get; set; } = new();

        /// <summary>
        /// Danh sách các bài viết gần đây.
        /// </summary>
        public List<Article> RecentArticles { get; set; } = new();
    }
}