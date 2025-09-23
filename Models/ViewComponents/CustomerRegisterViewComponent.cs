using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Models.ViewComponents
{
    /// <summary>
    /// ViewComponent dùng để hiển thị form đăng ký tài khoản khách hàng.
    /// </summary>
    public class CustomerRegisterViewComponent : ViewComponent
    {
        /// <summary>
        /// Hàm render giao diện đăng ký khách hàng.
        /// </summary>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            CustomerRegisterViewModel customerRegisterViewModel = new();
            return View(customerRegisterViewModel);
        }
    }

}
