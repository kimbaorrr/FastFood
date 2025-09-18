using System.ComponentModel.DataAnnotations;
using FastFood.DB;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Models.ViewModels
{
    public abstract class BaseCategoryViewModel
    {
        [Display(Name = "Tên danh mục")]
        [DataType(DataType.Text)]
        public string CategoryName { get; set; } = string.Empty;
        [Display(Name = "Mô tả")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;
    }

    public class NewCategoryViewModel : BaseCategoryViewModel
    {

        [Display(Name = "Ảnh đại diện")]
        [DataType(DataType.Text)]
        public IFormFile? ThumbnailImage { get; set; }
        [Display(Name = "Ảnh nền")]
        [DataType(DataType.Text)]
        public IFormFile? BackgroundImage { get; set; }

    }
    public class EditCategoryViewModel : BaseCategoryViewModel
    {
        public int CategoryId { get; set; } = -1;

    }
}