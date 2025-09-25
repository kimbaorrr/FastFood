using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Models.ViewComponents
{
    public class EmployeeOrderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string viewName, object model = null, CustomOrderDetailViewModel customOrderDetailViewModel)
        {
            ViewBag.OrderDetail = customOrderDetailViewModel;
            return View(viewName, model);
        }
    }
}
