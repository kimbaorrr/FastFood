using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.ViewModels
{
    /// <summary>
    /// ViewModel cơ sở cho tài khoản khách hàng.
    /// </summary>
    public abstract class BaseCustomerAccountViewModel
    {
        /// <summary>
        /// Tên đăng nhập của khách hàng.
        /// </summary>
        [Display(Name = "Tên đăng nhập")]
        [DataType(DataType.Text)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu của khách hàng.
        /// </summary>
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel cho việc đăng ký tài khoản khách hàng.
    /// </summary>
    public class CustomerRegisterViewModel : BaseCustomerAccountViewModel
    {
        /// <summary>
        /// Họ đệm của khách hàng.
        /// </summary>
        [Display(Name = "Họ đệm")]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Tên khách hàng.
        /// </summary>
        [Display(Name = "Tên khách hàng")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Email của khách hàng.
        /// </summary>
        [Display(Name = "Email")]
        [DataType(DataType.Text)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Số điện thoại của khách hàng.
        /// </summary>
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.Text)]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Địa chỉ giao hàng của khách hàng.
        /// </summary>
        [Display(Name = "Địa chỉ giao hàng")]
        [DataType(DataType.Text)]
        public string Address { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel cho việc đăng nhập tài khoản khách hàng.
    /// </summary>
    public class CustomerLoginViewModel : BaseCustomerAccountViewModel
    {

    }

    /// <summary>
    /// ViewModel chứa thông tin xác thực của khách hàng.
    /// </summary>
    public class CustomerClaimInfoViewModel
    {
        /// <summary>
        /// Mã khách hàng.
        /// </summary>
        public int CustomerId { get; set; } = -1;

        /// <summary>
        /// Ảnh đại diện của khách hàng.
        /// </summary>
        public string Avatar { get; set; } = string.Empty;

        /// <summary>
        /// Họ đệm của khách hàng.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Tên khách hàng.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Họ và tên đầy đủ của khách hàng.
        /// </summary>
        public string FullName => $"{LastName} {FirstName}";

        /// <summary>
        /// Email của khách hàng.
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }
}
