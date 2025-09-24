using Microsoft.AspNetCore.Mvc;

namespace FastFood.Models.ViewComponents
{
    public class EmployeeSidebarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string viewName)
        {
            return View(viewName);
        }
    }
}
