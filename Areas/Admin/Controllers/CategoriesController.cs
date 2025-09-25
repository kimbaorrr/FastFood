using FastFood.DB;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{

    [Route("admin/categories")]
    [Authorize(AuthenticationSchemes = "EmployeeScheme")]
    public class CategoriesController : BaseEmployeeController
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var categories = await this._categoryService.GetCategoriesPagedList(page, size);
            var categoriesByProductCount = await this._categoryService.GetCategoriesOrderByProductCount(4);

            ViewBag.Categories = categories;
            ViewBag.CategoriesByProductCount = categoriesByProductCount;
            ViewBag.CurrentPage = categories.PageNumber;
            ViewBag.TotalPages = categories.PageCount;
            return View("List");
        }

        [HttpPost("create")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(NewCategoryViewModel newCategoryViewModel)
        {
            newCategoryViewModel.CreatedBy = this._employeeId;
            (bool success, string message) = await this._categoryService.NewCategory(newCategoryViewModel);
            return CreateJsonResult(success, message);
        }

        [HttpPost("delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete([FromForm] int categoryId)
        {
            (bool success, string message) = await this._categoryService.DeleteCategory(categoryId);
            return CreateJsonResult(success, message);
        }

        [HttpPost("multiple-delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> MultipleDelete([FromForm] int[] categoryIds)
        {
            (bool success, string message) = await this._categoryService.DeleteCategories(categoryIds);
            return CreateJsonResult(success, message);
        }

        [HttpPost("edit")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(EditCategoryViewModel editCategoryViewModel)
        {
            editCategoryViewModel.CategoryId = this._employeeId;
            (bool success, string message) = await this._categoryService.EditCategory(editCategoryViewModel);
            return CreateJsonResult(success, message);
        }
    }
}
