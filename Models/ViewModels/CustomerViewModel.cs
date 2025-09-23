using FastFood.DB.Entities;
using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.ViewModels
{
    /// <summary>
    /// ViewModel cơ sở cho khách hàng.
    /// </summary>
    public abstract class BaseCustomerViewModel
    {
        /// <summary>
        /// Mã khách hàng.
        /// </summary>
        public int CustomerId { get; set; } = -1;

        /// <summary>
        /// Họ và tên khách hàng.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Ngày tạo tài khoản khách hàng.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Ngày sinh của khách hàng.
        /// </summary>
        public DateOnly? Bod { get; set; }

        /// <summary>
        /// Ảnh đại diện của khách hàng.
        /// </summary>
        public string Avatar { get; set; } = string.Empty;

        /// <summary>
        /// Email của khách hàng.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Số điện thoại của khách hàng.
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Địa chỉ của khách hàng.
        /// </summary>
        public string Address { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel cho khách hàng tiềm năng.
    /// </summary>
    public class PotentialCustomersViewModel : BaseCustomerViewModel
    {
        /// <summary>
        /// Số lần mua hàng.
        /// </summary>
        public int NumOfPurchase { get; set; } = 0;

        /// <summary>
        /// Tổng giá trị hóa đơn.
        /// </summary>
        public int? TotalInvoice { get; set; } = 0;

        /// <summary>
        /// Giá trị đơn hàng lớn nhất.
        /// </summary>
        public int? BiggestOrder { get; set; }

        /// <summary>
        /// Thời gian mua hàng gần nhất.
        /// </summary>
        public DateTime? RecentPurchase { get; set; }

        /// <summary>
        /// Thời gian truy cập gần nhất.
        /// </summary>
        public DateTime? LastAccessedTime { get; set; }
    }

    /// <summary>
    /// ViewModel chi tiết khách hàng.
    /// </summary>
    public class CustomerDetailViewModel : PotentialCustomersViewModel
    {
        /// <summary>
        /// Số lượng hoạt động gần đây.
        /// </summary>
        public int RecentActivities { get; set; } = 0;

        /// <summary>
        /// Danh sách các đơn hàng của khách hàng.
        /// </summary>
        public List<Order> Orders { get; set; } = new List<Order>();
    }

    /// <summary>
    /// ViewModel gửi phản hồi của khách hàng.
    /// </summary>
    public class CustomerSendFeedbackViewModel
    {
        /// <summary>
        /// Tên khách hàng.
        /// </summary>
        [Display(Name = "Tên khách hàng")]
        [DataType(DataType.Text)]
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Email của khách hàng.
        /// </summary>
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Số điện thoại của khách hàng.
        /// </summary>
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.Text)]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Nội dung phản hồi.
        /// </summary>
        [Display(Name = "Nội dung")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel cho khách hàng.
    /// </summary>
    public class CustomerViewModel : BaseCustomerViewModel
    {
    }

    /// <summary>
    /// ViewModel tuỳ chỉnh cho trang chủ khách hàng.
    /// </summary>
    public class CustomerCustomHomeViewModel
    {
        /// <summary>
        /// Sản phẩm bán chạy nhất.
        /// </summary>
        public Product BestSellingProduct { get; set; } = new();

        /// <summary>
        /// Danh sách 2 sản phẩm giảm giá nhiều nhất.
        /// </summary>
        public List<Product> Top2DiscountProducts { get; set; } = new();

        /// <summary>
        /// Danh sách đánh giá sản phẩm.
        /// </summary>
        public List<ProductReview> ProductReviews { get; set; } = new();

        /// <summary>
        /// Danh sách 10 sản phẩm bán chạy nhất.
        /// </summary>
        public List<Product> Top10HotSales { get; set; } = new();

        /// <summary>
        /// Danh sách 3 bài viết nổi bật.
        /// </summary>
        public List<Article> Top3Articles { get; set; } = new();
    }
}
