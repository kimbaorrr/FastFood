using System.ComponentModel.DataAnnotations;
using FastFood.DB.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FastFood.Models.ViewModels
{
    /// <summary>
    /// Lớp cơ sở cho ViewModel sản phẩm.
    /// </summary>
    public abstract class BaseProductViewModel
    {
        /// <summary>
        /// Tên sản phẩm.
        /// </summary>
        [Display(Name = "Tên sản phẩm")]
        [DataType(DataType.Text)]
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Mã danh mục sản phẩm.
        /// </summary>
        [Display(Name = "Danh mục")]
        [DataType(DataType.Text)]
        public int CategoryId { get; set; } = -1;

        /// <summary>
        /// Giá gốc của sản phẩm.
        /// </summary>
        [Display(Name = "Giá gốc")]
        [DataType(DataType.Currency)]
        public int OriginalPrice { get; set; } = 0;

        /// <summary>
        /// Phần trăm khuyến mãi.
        /// </summary>
        [Display(Name = "Khuyến mãi (%)")]
        [Range(0, 100)]
        public int Discount { get; set; } = 0;

        /// <summary>
        /// Giá sau khuyến mãi.
        /// </summary>
        [Display(Name = "Giá sau khuyến mãi")]
        [DataType(DataType.Currency)]
        public int FinalPrice { get; set; } = 0;

        /// <summary>
        /// Mô tả ngắn về sản phẩm.
        /// </summary>
        [Display(Name = "Mô tả ngắn")]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả chi tiết về sản phẩm.
        /// </summary>
        [Display(Name = "Mô tả dài")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Ngày tạo sản phẩm.
        /// </summary>
        [Display(Name = "Ngày tạo")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Ngày cập nhật sản phẩm.
        /// </summary>
        [Display(Name = "Ngày cập nhật")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Người tạo sản phẩm.
        /// </summary>
        [Display(Name = "Người tạo")]
        [DataType(DataType.MultilineText)]
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Danh sách danh mục sản phẩm.
        /// </summary>
        public SelectList Categories { get; set; } = null!;
    }

    /// <summary>
    /// Lớp cơ sở cho ViewModel đánh giá sản phẩm.
    /// </summary>
    public abstract class BaseReviewViewModel
    {
        /// <summary>
        /// Mã khách hàng.
        /// </summary>
        [DataType(DataType.Text)]
        public int CustomerId { get; set; } = -1;

        /// <summary>
        /// Nội dung đánh giá.
        /// </summary>
        [DataType(DataType.MultilineText)]
        [Display(Name = "Nội dung đánh giá")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Số sao đánh giá (1-5).
        /// </summary>
        [DataType(DataType.Text)]
        [Range(1, 5)]
        public int StarRating { get; set; } = 3;
    }

    /// <summary>
    /// ViewModel thêm mới sản phẩm.
    /// </summary>
    public class NewProductViewModel : BaseProductViewModel
    {
        /// <summary>
        /// Mã nhân viên tạo sản phẩm.
        /// </summary>
        public int? EmployeeId { get; set; } = -1;
    }

    /// <summary>
    /// ViewModel chi tiết sản phẩm.
    /// </summary>
    public class ProductDetailViewModel : BaseProductViewModel
    {
        /// <summary>
        /// Mã sản phẩm.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; } = -1;

        /// <summary>
        /// Trạng thái duyệt sản phẩm.
        /// </summary>
        [Display(Name = "Trạng thái duyệt")]
        [DataType(DataType.Text)]
        public string ApprovedStatusText { get; set; } = string.Empty;

        /// <summary>
        /// Ngày duyệt sản phẩm.
        /// </summary>
        [Display(Name = "Ngày duyệt")]
        [DataType(DataType.DateTime)]
        public DateTime? ApprovedAt { get; set; }

        /// <summary>
        /// Người duyệt sản phẩm.
        /// </summary>
        [Display(Name = "Người duyệt")]
        [DataType(DataType.Text)]
        public string ApprovedBy { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel đánh giá sản phẩm tuỳ chỉnh.
    /// </summary>
    public class CustomProductReviewViewModel : BaseReviewViewModel
    {
        /// <summary>
        /// Mã sản phẩm.
        /// </summary>
        public int ProductId { get; set; } = -1;
    }

    /// <summary>
    /// ViewModel đăng sản phẩm mới.
    /// </summary>
    public class NewProductPostViewModel
    {
        /// <summary>
        /// Thông tin sản phẩm mới.
        /// </summary>
        public NewProductViewModel NewProduct { get; set; } = null!;

        /// <summary>
        /// Danh sách nguyên liệu cho sản phẩm.
        /// </summary>
        public List<IngredientsSelectedViewModel> Ingredients { get; set; } = new();

        /// <summary>
        /// Danh sách ảnh sản phẩm.
        /// </summary>
        public List<FormFile>? ProductImages { get; set; }
    }

    /// <summary>
    /// ViewModel đăng chi tiết sản phẩm.
    /// </summary>
    public class ProductDetailPostViewModel
    {
        /// <summary>
        /// Thông tin chi tiết sản phẩm.
        /// </summary>
        public ProductDetailViewModel ProductDetail { get; set; } = null!;

        /// <summary>
        /// Danh sách nguyên liệu cho sản phẩm.
        /// </summary>
        public List<IngredientsSelectedViewModel> Ingredients { get; set; } = new();

        /// <summary>
        /// Danh sách ảnh sản phẩm.
        /// </summary>
        public List<FormFile>? ProductImages { get; set; }
    }

    /// <summary>
    /// ViewModel nguyên liệu đã chọn cho sản phẩm.
    /// </summary>
    public class IngredientsSelectedViewModel
    {
        /// <summary>
        /// Mã nguyên liệu.
        /// </summary>
        public string IngredientId { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng nguyên liệu.
        /// </summary>
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Đơn vị tính nguyên liệu.
        /// </summary>
        public string Unit { get; set; } = "cái";
    }

    /// <summary>
    /// ViewModel chi tiết tuỳ chỉnh cho sản phẩm.
    /// </summary>
    public class CustomProductDetailViewModel
    {
        /// <summary>
        /// Thông tin sản phẩm.
        /// </summary>
        public Product Product { get; set; } = new();

        /// <summary>
        /// Danh sách đường dẫn ảnh sản phẩm.
        /// </summary>
        public string[] Image { get; set; } = new string[] { };

        /// <summary>
        /// Danh sách đánh giá sản phẩm.
        /// </summary>
        public List<ProductReview> ProductReviews { get; set; } = new();

        /// <summary>
        /// Đánh giá nổi bật nhất của sản phẩm.
        /// </summary>
        public ProductReview TopProductReview { get; set; } = new();

        /// <summary>
        /// Đánh giá sản phẩm của người dùng.
        /// </summary>
        public ProductReview ProductReviewModel { get; set; } = new();

        /// <summary>
        /// Danh sách sản phẩm ngẫu nhiên.
        /// </summary>
        public List<Product> RandomProducts { get; set; } = new();
    }
}
