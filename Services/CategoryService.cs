using FastFood.DB.Entities;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Services
{
    public class CategoryService : CommonService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly string _categoryVirtualPath;

        public CategoryService(ICategoryRepository categoryRepository, IFileUploadService fileUploadService, IWebHostEnvironment webHostEnvironment)
        {
            _categoryRepository = categoryRepository;
            _fileUploadService = fileUploadService;
            _categoryVirtualPath = Path.Combine(webHostEnvironment.WebRootPath, "admin_page/uploads/categories/");
        }

        public async Task<IPagedList<Category>> GetCategoriesPagedList(int page, int size)
        {
            var categories = await this._categoryRepository.GetCategories();
            return categories.OrderBy(x => x.CategoryId).ToPagedList(page, size);
        }

        public async Task<(bool, string)> NewCategory(NewCategoryViewModel newCategoryViewModel)
        {
            (bool bgSuccess, string bgMessage, string newBackgroudFilePath) = await this._fileUploadService.ImageUpload(newCategoryViewModel.BackgroundImage, this._categoryVirtualPath);
            if (!bgSuccess)
            {
                return (false, bgMessage);
            }

            (bool thumbSuccess, string thumbMessage, string newThumbFilePath) = await this._fileUploadService.ImageUpload(newCategoryViewModel.ThumbnailImage, this._categoryVirtualPath);
            if (!thumbSuccess)
            {
                return (false, thumbMessage);
            }
            Category category = new()
            {
                CategoryName = newCategoryViewModel.CategoryName,
                Description = newCategoryViewModel.Description,
                CreatedAt = DateTime.Now,
                CreatedBy = newCategoryViewModel.CreatedBy,
                BackgroundImage = newBackgroudFilePath,
                ThumbnailImage = newThumbFilePath
            };

            await this._categoryRepository.AddCategory(category);

            return (true, "Danh mục mới đã được tạo thành công !");

        }

        public async Task<(bool, string)> DeleteCategory(int categoryId)
        {
            var category = await this._categoryRepository.GetCategoryById(categoryId);
            var hasProducts = category.Products.Any();

            if (category != null)
            {
                if (hasProducts)
                {
                    return (false, $"Danh mục {categoryId} đã có sản phẩm. Không thể xóa !");
                }

                CommonHelper.DeleteFile(category.BackgroundImage ?? string.Empty);
                CommonHelper.DeleteFile(category.ThumbnailImage ?? string.Empty);

                await this._categoryRepository.DeleteCategory(category);
                return (true, $"Đã xóa thành công danh mục {categoryId} !");
            }

            return (false, $"Không tìm thấy danh mục {categoryId} !");
        }

        public async Task<(bool, string)> DeleteCategories(int[] categoryIds)
        {
            List<int> successIds = new();
            List<int> failedIds = new();

            foreach (int categoryId in categoryIds)
            {
                (bool success, string message) = await this.DeleteCategory(categoryId);
                if (success)
                    successIds.Add(categoryId);
                else 
                    failedIds.Add(categoryId);
            }
            return (true, $"Đã xóa thành công {successIds.Count}/{failedIds.Count} danh mục. Các id danh mục lỗi gồm: {failedIds.ToArray()}");
        }

        public async Task<(bool, string)> EditCategory(EditCategoryViewModel editCategoryViewModel)
        {
            var category = await this._categoryRepository.GetCategoryById(editCategoryViewModel.CategoryId);
            if (category != null)
            {
                category.Description = editCategoryViewModel.Description;
                category.CategoryName = editCategoryViewModel.CategoryName;
                await this._categoryRepository.UpdateCategory(category);
                return (true, $"Sửa danh mục {editCategoryViewModel.CategoryId} thành công !");
            }
            return (false, $"Không tìm thấy danh mục {editCategoryViewModel.CategoryId} !");
        }

        public async Task<SelectList> GetCustomCategoriesSelectList()
        {
            var categories = await this._categoryRepository.GetCategories();
            return new SelectList(
                categories
                .Select(p => new SelectListItem
                {
                    Text = p.CategoryName,
                    Value = p.CategoryId.ToString()
                }).ToList(), "Value", "Text");

        }
    }
}
