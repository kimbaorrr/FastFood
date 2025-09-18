using FastFood.DB;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FastFood.Services
{
    public class EmployeeService : CommonService, IEmployeeService
    {
        private readonly IEmployeeAccountRepository _employeeAccountRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly PasswordHasher<string> _passwordHasher;
        private readonly IEmailService _emailService;
        public EmployeeService(IEmployeeAccountRepository employeeAccountRepository, IPermissionRepository permissionRepository, IEmailService emailService)
        {
            _employeeAccountRepository = employeeAccountRepository;
            _permissionRepository = permissionRepository;
            _passwordHasher = new PasswordHasher<string>();
            _emailService = emailService;
        }

        public async Task<bool> CheckPermission(string employeeId, int permissonId)
        {
            var employeeAccounts = await this._employeeAccountRepository.GetEmployeeAccounts();

            var permissionsOfEmployee = employeeAccounts
                                .Where(x => x.EmployeeId == Convert.ToInt32(employeeId))
                                .Select(x => x.Permission)
                                .FirstOrDefault()
                                ?? string.Empty;

            if (string.IsNullOrEmpty(permissionsOfEmployee))
                return false;

            bool isAccessed = permissionsOfEmployee.Split(',').Contains(permissonId.ToString());
            return isAccessed;
        }

        public async Task<bool> CheckRole(string employeeId)
        {
            var employeeAccounts = await this._employeeAccountRepository.GetEmployeeAccounts();

            return employeeAccounts
                .Where(x => x.EmployeeId == Convert.ToInt32(employeeId))
                .Select(x => x.Role)
                .FirstOrDefault();
        }

        public async Task<SortedList<string, string>> CustomListPermissions(string permissionIds)
        {
            var permissions = await this._permissionRepository.GetPermissions();

            string[] idsSplitted = permissionIds.Split(",", StringSplitOptions.RemoveEmptyEntries);
            SortedList<string, string> customListPermissions = new SortedList<string, string>();

            foreach (string permissonId in idsSplitted)
            {
                string description = permissions
                    .Where(x => x.PermissionId == Convert.ToInt32(permissonId))
                    .Select(x => x.Description)
                    .FirstOrDefault() ?? string.Empty;

                customListPermissions.Add(permissonId, description);
            }
            return customListPermissions;
        }

        public async Task<(bool, Employee?)> LoginChecker(EmployeeLoginViewModel employeeLoginViewModel)
        {
            employeeLoginViewModel.UserName = employeeLoginViewModel.UserName.Normalize().Trim().ToLower();

            var employeeInfo = await this._employeeAccountRepository.GetEmployeeAccountByUserName(employeeLoginViewModel.UserName);

            if (employeeInfo == null || employeeInfo.EmployeeId != -1)
            {
                return (false, null);
            }

            var verifyResult = this._passwordHasher.VerifyHashedPassword(
                employeeLoginViewModel.UserName,
                employeeInfo.Password,
                employeeLoginViewModel.Password
            );

            if (verifyResult != PasswordVerificationResult.Success ||
                   verifyResult != PasswordVerificationResult.SuccessRehashNeeded)
            {
                return (false, null);
            }

            return (true, employeeInfo.Employee);
        }

        public async Task<(bool, string)> ChangePassword(EmployeeChangePasswordViewModel employeeChangePasswordViewModel)
        {
            var employeeAccount = await this._employeeAccountRepository.GetEmployeeAccountByEmployeeId(employeeChangePasswordViewModel.EmployeeId);

            var verifyResult = this._passwordHasher.VerifyHashedPassword(
                string.Empty,
                employeeAccount.Password,
                employeeChangePasswordViewModel.OldPassword
            );

            if (verifyResult != PasswordVerificationResult.Success ||
                verifyResult != PasswordVerificationResult.SuccessRehashNeeded)
            {
                return (false, "Mật khẩu cũ không hợp lệ !");
            }
            if (employeeChangePasswordViewModel.OldPassword.Equals(employeeChangePasswordViewModel.NewPassword))
            {
                return (false, "Mật khẩu cũ & mật khẩu mới không được trùng nhau !");
            }

            if (!employeeChangePasswordViewModel.NewPassword.Equals(employeeChangePasswordViewModel.ReenterPassword))
            {
                return (false, "Mật khẩu mới & nhập lại mật khẩu không đúng !");
            }

            string newPassword = this._passwordHasher.HashPassword(string.Empty, employeeChangePasswordViewModel.NewPassword);

            await this._employeeAccountRepository.UpdateNewPassword(string.Empty, newPassword, false);

            return (true, "Đổi mật khẩu mới thành công !");

        }

        public async Task<(bool, string)> ForgotPassword(EmployeeForgotPasswordViewModel employeeForgotPasswordViewModel)
        {
            var employeeAccount = await this._employeeAccountRepository.GetEmployeeAccountByUserName(employeeForgotPasswordViewModel.UserName);

            if (employeeAccount == null)
            {
                return (false, "Thông tin đăng nhập không hợp lệ !");
            }

            string tempPassword = Guid.CreateVersion7().ToString().Replace("-", "");
            await this._employeeAccountRepository.UpdateNewPassword(
                employeeForgotPasswordViewModel.UserName,
                tempPassword,
                true
            );

            await this._emailService.SendEmail(
                employeeForgotPasswordViewModel.Email,
                "Khôi phục mật khẩu",
                $"Mật khẩu tạm thời của bạn là: {tempPassword}\nVui lòng thay đổi ngay sau khi đăng nhập."
            );

            return (true, "Đã gửi thông tin khôi phục đến email của bạn !");

        }
    }
}
