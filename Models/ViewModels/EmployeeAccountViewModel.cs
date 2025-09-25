using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.ViewModels
{
    /// <summary>
    /// Lớp cơ sở cho các ViewModel tài khoản nhân viên.
    /// </summary>
    public abstract class BaseEmployeeAccountViewModel
    {
        /// <summary>
        /// Tên đăng nhập của nhân viên.
        /// </summary>
        [Display(Name = "Tên đăng nhập")]
        [DataType(DataType.Text)]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu của nhân viên.
        /// </summary>
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Địa chỉ email của nhân viên.
        /// </summary>
        [DataType(DataType.EmailAddress)]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel cho chức năng đăng nhập của nhân viên.
    /// </summary>
    public class EmployeeLoginViewModel : BaseEmployeeAccountViewModel
    {
        /// <summary>
        /// Tên đăng nhập (bắt buộc).
        /// </summary>
        [Required]
        public new string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu (bắt buộc).
        /// </summary>
        [Required]
        public new string Password { get; set; } = string.Empty;

        /// <summary>
        /// Ghi nhớ đăng nhập.
        /// </summary>
        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; } = false;
    }

    /// <summary>
    /// ViewModel cho chức năng đổi mật khẩu của nhân viên.
    /// </summary>
    public class EmployeeChangePasswordViewModel
    {
        /// <summary>
        /// Mã nhân viên.
        /// </summary>
        public int EmployeeId { get; set; } = -1;

        /// <summary>
        /// Mật khẩu cũ (bắt buộc).
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; } = string.Empty;

        /// <summary>
        /// Mật khẩu mới (bắt buộc).
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Nhập lại mật khẩu mới (bắt buộc, phải giống mật khẩu mới).
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không giống nhau !")]
        public string ReenterPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel cho chức năng quên mật khẩu của nhân viên.
    /// </summary>
    public class EmployeeForgotPasswordViewModel : BaseEmployeeAccountViewModel
    {
        /// <summary>
        /// Địa chỉ email (bắt buộc).
        /// </summary>
        [Required]
        [MaxLength(100)]
        public new string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel cho chức năng đăng ký tài khoản đăng nhập cho nhân viên.
    /// </summary>
    public class EmployeeRegisterLoginAccountViewModel : BaseEmployeeAccountViewModel
    {
        /// <summary>
        /// Mã nhân viên.
        /// </summary>
        public int EmployeeId { get; set; } = -1;

        /// <summary>
        /// Xác nhận mật khẩu (bắt buộc, phải giống mật khẩu).
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không giống nhau !")]
        public string ConfirmPassword { get; set; } = string.Empty;

        /// <summary>
        /// Vai trò của nhân viên.
        /// </summary>
        [Display(Name = "Vai trò")]
        public bool Role { get; set; } = false;
    }

    /// <summary>
    /// ViewModel chứa thông tin xác thực của nhân viên.
    /// </summary>
    public class EmployeeClaimInfoViewModel
    {
        /// <summary>
        /// Mã nhân viên.
        /// </summary>
        public int EmployeeId { get; set; } = -1;

        /// <summary>
        /// Ảnh đại diện của nhân viên.
        /// </summary>
        public string Avatar { get; set; } = string.Empty;

        /// <summary>
        /// Họ đệm của nhân viên.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Tên nhân viên.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Họ và tên đầy đủ của nhân viên.
        /// </summary>
        public string FullName => $"{LastName} {FirstName}";

        /// <summary>
        /// Email của nhân viên.
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }

    public class EmployeeEditAccountViewModel : BaseEmployeeAccountViewModel
    {
        /// <summary>
        /// Mã nhân viên.
        /// </summary>
        public int EmployeeId { get; set; } = -1;
        /// <summary>
        /// Vai trò của nhân viên.
        /// </summary>
        [Display(Name = "Vai trò")]
        public bool Role { get; set; } = false;
    }
}
