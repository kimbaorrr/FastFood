using System.ComponentModel.DataAnnotations;
using FastFood.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Models.ViewModels
{
    /// <summary>
    /// ViewModel cơ sở cho danh mục sản phẩm.
    /// </summary>
    public abstract class BaseCategoryViewModel
    {
        /// <summary>
        /// Tên danh mục.
        /// </summary>
        [Display(Name = "Tên danh mục")]
        [DataType(DataType.Text)]
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả danh mục.
        /// </summary>
        [Display(Name = "Mô tả")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel cho việc tạo mới danh mục sản phẩm.
    /// </summary>
    public class NewCategoryViewModel : BaseCategoryViewModel
    {
        /// <summary>
        /// Ảnh đại diện của danh mục.
        /// </summary>
        [Display(Name = "Ảnh đại diện")]
        [DataType(DataType.Text)]
        public FormFile? ThumbnailImage { get; set; }

        /// <summary>
        /// Ảnh nền của danh mục.
        /// </summary>
        [Display(Name = "Ảnh nền")]
        [DataType(DataType.Text)]
        public FormFile? BackgroundImage { get; set; }

        /// <summary>
        /// Mã nhân viên tạo danh mục.
        /// </summary>
        public int? CreatedBy { get; set; } = -1;
    }

    /// <summary>
    /// ViewModel cho việc chỉnh sửa danh mục sản phẩm.
    /// </summary>
    public class EditCategoryViewModel : BaseCategoryViewModel
    {
        /// <summary>
        /// Mã danh mục.
        /// </summary>
        public int? CategoryId { get; set; } = -1;
    }

    /// <summary>
    /// ViewModel tuỳ chỉnh cho danh mục, bao gồm thông tin danh mục và danh sách sản phẩm.
    /// </summary>
    public class CustomCategoryViewModel
    {
        /// <summary>
        /// Thông tin danh mục.
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Danh sách sản phẩm thuộc danh mục.
        /// </summary>
        public List<Product> Products { get; set; }
    }
}