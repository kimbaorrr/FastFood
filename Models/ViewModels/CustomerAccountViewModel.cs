using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.ViewModels
{
    public abstract class BaseCustomerAccountViewModel
    {
        [Display(Name = "Tên đăng nhập")]
        [DataType(DataType.Text)]
        public string UserName { get; set; } = string.Empty;
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
    public class CustomerRegisterViewModel : BaseCustomerAccountViewModel
    {
        [Display(Name = "Họ đệm")]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = string.Empty;
        [Display(Name = "Tên khách hàng")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = string.Empty;
        [Display(Name = "Email")]
        [DataType(DataType.Text)]
        public string Email { get; set; } = string.Empty;
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.Text)]
        public string Phone { get; set; } = string.Empty;
        
    }

    public class CustomerLoginViewModel : BaseCustomerAccountViewModel
    {

    }
}
