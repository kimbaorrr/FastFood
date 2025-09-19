using FastFood.DB;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{

    [Route("admin/categories")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get(int page = 1, int size = 10)
        {
            var categories = await this._categoryService.GetCategoriesPagedList(page, size);
            ViewBag.Categories = categories;
            ViewBag.CurrentPage = categories.PageNumber;
            ViewBag.TotalPages = categories.PageCount;
            return View();
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
