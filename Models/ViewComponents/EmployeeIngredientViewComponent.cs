using Microsoft.AspNetCore.Mvc;

namespace FastFood.Models.ViewComponents
{
    public class EmployeeIngredientViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string viewName, object? model)
        {
            return View(viewName, model);
        }
    }
}
