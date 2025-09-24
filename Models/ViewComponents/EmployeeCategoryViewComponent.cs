using Microsoft.AspNetCore.Mvc;

namespace FastFood.Models.ViewComponents
{
    public class EmployeeCategoryViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string viewName, object model = null)
        {
            return View(viewName, model);
        }
    }
}
