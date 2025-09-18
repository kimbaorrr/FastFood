using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FastFood.Models.ViewModels
{
    public abstract class BaseProductViewModel
    {
        [Display(Name = "Tên sản phẩm")]
        [DataType(DataType.Text)]
        public string ProductName { get; set; } = string.Empty;

        [Display(Name = "Danh mục")]
        [DataType(DataType.Text)]
        public int CategoryId { get; set; } = -1;

        [Display(Name = "Giá gốc")]
        [DataType(DataType.Currency)]
        public int OriginalPrice { get; set; } = 0;

        [Display(Name = "Khuyến mãi (%)")]
        [Range(0, 100)]
        public int Discount { get; set; } = 0;

        [Display(Name = "Giá sau khuyến mãi")]
        [DataType(DataType.Currency)]
        public int FinalPrice { get; set; } = 0;

        [Display(Name = "Mô tả ngắn")]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; } = string.Empty;

        [Display(Name = "Mô tả dài")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Ngày cập nhật")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Người tạo")]
        [DataType(DataType.MultilineText)]
        public string CreatedBy { get; set; } = string.Empty;

        public SelectList Categories { get; set; } = null!;
    }

    public abstract class BaseReviewViewModel
    {
        [DataType(DataType.Text)]
        public int CustomerId { get; set; } = -1;

        [DataType(DataType.MultilineText)]
        [Display(Name = "Nội dung đánh giá")]
        public string Content { get; set; } = string.Empty;

        [DataType(DataType.Text)]
        [Range(1, 5)]
        public int StarRating { get; set; } = 3;
    }

    public class NewProductViewModel : BaseProductViewModel
    {
       
    }

    public class ProductDetailViewModel : BaseProductViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; } = -1;

        [Display(Name = "Trạng thái duyệt")]
        [DataType(DataType.Text)]
        public string ApprovedStatusText { get; set; } = string.Empty;

        [Display(Name = "Ngày duyệt")]
        [DataType(DataType.DateTime)]
        public DateTime? ApprovedAt { get; set; }

        [Display(Name = "Người duyệt")]
        [DataType(DataType.Text)]
        public string ApprovedBy { get; set; } = string.Empty;
    }

    public class CustomProductReviewViewModel : BaseReviewViewModel
    {
        
    }

    public class NewProductPostViewModel
    {
        public NewProductViewModel NewProduct { get; set; } = null!;
        public List<IngredientsSelectedViewModel> Ingredients { get; set; } = new();
        public List<IFormFile>? ProductImages { get; set; }
    }

    public class ProductDetailPostViewModel
    {
        public ProductDetailViewModel ProductDetail { get; set; } = null!;
        public List<IngredientsSelectedViewModel> Ingredients { get; set; } = new();
        public List<IFormFile>? ProductImages { get; set; }
    }

    public class IngredientsSelectedViewModel
    {
        public string IngredientId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public string Unit { get; set; } = "cái";
    }
}
