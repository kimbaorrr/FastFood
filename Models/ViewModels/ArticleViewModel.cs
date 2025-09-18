using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.ViewModels
{
    public abstract class BaseArticleViewModel
    {
        [Display(Name = "Tiêu đề")]
        [DataType(DataType.Text)]
        public string Title { get; set; } = string.Empty;
        [Display(Name = "Mô tả ngắn")]
        [DataType(DataType.Text)]
        public string Summary { get; set; } = string.Empty;
        [Display(Name = "Nội dung")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; } = string.Empty;
    }
    public class NewArticleViewModel : BaseArticleViewModel
    {
        public int AuthorId { get; set; } = -1;

        [Display(Name = "Ảnh đại diện")]
        [DataType(DataType.Text)]
        public FormFile? CoverImage { get; set; }
    }

    public class EditArticleViewModel : BaseArticleViewModel
    {
        public int ArticleId { get; set; } = -1;

    }
}