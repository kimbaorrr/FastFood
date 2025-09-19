using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.ViewModels
{
    public abstract class BaseEmployeeAccountViewModel
    {
        [Display(Name = "Tên đăng nhập")]
        [DataType(DataType.Text)]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.EmailAddress)]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
    }

    public class EmployeeLoginViewModel : BaseEmployeeAccountViewModel
    {
        [Required]
        public new string UserName { get; set; } = string.Empty;

        [Required]
        public new string Password { get; set; } = string.Empty;

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; } = false;
    }

    public class EmployeeChangePasswordViewModel
    {
        public int EmployeeId { get; set; } = -1;
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("MatKhauMoi", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không giống nhau !")]
        public string ReenterPassword { get; set; } = string.Empty;
    }

    public class EmployeeForgotPasswordViewModel : BaseEmployeeAccountViewModel
    {
        [Required]
        [MaxLength(100)]
        public new string Email { get; set; } = string.Empty;
    }

    public class EmployeeRegisterLoginAccountViewModel : BaseEmployeeAccountViewModel
    {
        public int EmployeeId { get; set; } = -1;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không giống nhau !")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "Vai trò")]
        public bool Role { get; set; } = false;
    }
}
