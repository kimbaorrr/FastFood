using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Models.ViewComponents
{
    public class EmployeeChangePasswordViewComponent : ViewComponent
    {
        public EmployeeChangePasswordViewComponent()
        {
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            EmployeeChangePasswordViewModel employeeChangePasswordViewModel = new();
            return View(employeeChangePasswordViewModel);
        }
    }
}
