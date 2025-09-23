using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Models.ViewComponents
{
    /// <summary>
    /// ViewComponent dùng để hiển thị form đăng nhập của khách hàng.
    /// </summary>
    public class CustomerLoginViewComponent : ViewComponent
    {
        /// <summary>
        /// Hàm render giao diện đăng nhập khách hàng.
        /// </summary>
        /// <returns>
        /// </returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            CustomerLoginViewModel customerLoginViewModel = new();
            return View(customerLoginViewModel);
        }
    }

}
