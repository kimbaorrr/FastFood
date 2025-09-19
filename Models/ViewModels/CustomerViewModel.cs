using FastFood.DB;
using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.ViewModels
{
    public abstract class BaseCustomerViewModel
    {
        public int CustomerId { get; set; } = -1;
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateOnly? Bod { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
    public class PotentialCustomersViewModel : BaseCustomerViewModel
    {
        public int NumOfPurchase { get; set; } = 0;
        public int? TotalInvoice { get; set; } = 0;
        public int? BiggestOrder { get; set; }
        public DateTime? RecentPurchase { get; set; }
        public DateTime? LastAccessedTime { get; set; }
        
    }
    public class CustomerDetailViewModel : PotentialCustomersViewModel
    {
        public int RecentActivities { get; set; } = 0;

        public List<Order> Orders { get; set; } = new List<Order>();
    }

    public class CustomerSendQuestionViewModel
    {
        [Display(Name = "Tên khách hàng")]
        [DataType(DataType.Text)]
        public string CustomerName { get; set; } = string.Empty;

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.Text)]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Nội dung")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; } = string.Empty;
    }
    public class CustomerViewModel : BaseCustomerViewModel
    {
        
    }


}
