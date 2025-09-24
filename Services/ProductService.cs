using FastFood.DB.Entities;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Services
{
    public class ProductService : CommonService, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductIngredientRepository _productIngredientRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;
        private readonly IFileUploadService _fileUploadService;
        private readonly string _productVirtualPath;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProductReviewRepository _productReviewRepository;
        private readonly IProductReviewService _productReviewService;

        public ProductService(ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment, IProductRepository productRepository, ICategoryService categoryService, IEmployeeRepository employeeRepository, IFileUploadService fileUploadService, IProductIngredientRepository productIngredientRepository, IProductReviewRepository productReviewRepository, IProductReviewService productReviewService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _employeeRepository = employeeRepository;
            _fileUploadService = fileUploadService;
            _categoryService = categoryService;
            _productIngredientRepository = productIngredientRepository;
            _productVirtualPath = Path.Combine(webHostEnvironment.WebRootPath, "admin_page/products/");
            _productReviewRepository = productReviewRepository;
            _productReviewService = productReviewService;
        }

        public async Task<IPagedList<Product>> GetProductsByApproveStatusPagedList(bool isApproved, int page, int size)
        {
            var productsByApproveStatus = await this._productRepository.GetProductsByApproveStatus(isApproved);
            return productsByApproveStatus.OrderBy(x => x.ProductId).ToPagedList(page, size);
        }

        public async Task<(bool, string)> AddProduct(NewProductPostViewModel newProductPostViewModel)
        {
            var product = await this._productRepository.GetProductByName(newProductPostViewModel.NewProduct.ProductName);
            if (product != null)
            {
                return (false, "Tên sản phẩm đã tồn tại trên hệ thống !");
            }

            var category = await this._categoryRepository.GetCategoryById(newProductPostViewModel.NewProduct.CategoryId);
            if (category == null)
            {
                return (false, "Vui lòng chọn danh mục cho sản phẩm !");
            }

            (bool success, string message, string productImages) = await this._fileUploadService.ImagesUpload(newProductPostViewModel.ProductImages, this._productVirtualPath);

            if (!success)
            {
                return (false, message);
            }

            Product newProduct = new()
            {
                ProductName = newProductPostViewModel.NewProduct.ProductName,
                CategoryId = newProductPostViewModel.NewProduct.CategoryId,
                OriginalPrice = newProductPostViewModel.NewProduct.OriginalPrice,
                Discount = newProductPostViewModel.NewProduct.Discount,
                Summary = newProductPostViewModel.NewProduct.Summary,
                Content = newProductPostViewModel.NewProduct.Content,
                Image = productImages,
                CreatedAt = DateTime.Now,
                CreatedBy = newProductPostViewModel.NewProduct.EmployeeId,
                IsApprove = false
            };

            await this._productRepository.AddProduct(newProduct);

            if (newProductPostViewModel.Ingredients == null || !newProductPostViewModel.Ingredients.Any())
            {
                return (false, "Hãy chọn ít nhất một nguyên liệu !");
            }

            foreach (var selectedIngredient in newProductPostViewModel.Ingredients)
            {
                ProductIngredient productIngredient = new()
                {
                    ProductId = product!.ProductId,
                    IngredientId = Convert.ToInt32(selectedIngredient.IngredientId),
                    QuantityNeeded = selectedIngredient.Quantity,
                    Unit = selectedIngredient.Unit
                };
                await this._productIngredientRepository.AddProductIngredient(productIngredient);
            }

            return (true, "Thêm sản phẩm thành công !");



        }

        public async Task<(bool, string, ProductDetailViewModel, string)> GetProductDetailViewModel(int productId, int? employeeId)
        {
            var product = await this._productRepository.GetProductById(productId);

            if (product == null)
            {
                return (false, "Id sản phẩm không hợp lệ !", null!, string.Empty);
            }

            ProductDetailViewModel productDetailViewModel = new()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                OriginalPrice = product.OriginalPrice,
                Discount = product.Discount ?? 0,
                FinalPrice = product.FinalPrice ?? 0,
                Summary = product.Summary ?? string.Empty,
                Content = product.Content ?? string.Empty,
                CreatedAt = product.CreatedAt,
                CategoryId = product.Category?.CategoryId ?? 0,
                CreatedBy = product.CreatedBy.HasValue ? this._employeeRepository.GetFullName(employeeId) : string.Empty,
                ApprovedStatusText = product.IsApprove ? "Đã duyệt" : "Chưa duyệt",
                ApprovedBy = product.Approver?.LastName + " " + product.Approver?.FirstName ?? string.Empty,
                ApprovedAt = product.ApprovedAt,
                Categories = await this._categoryService.GetCustomCategoriesSelectList()
            };

            return (true, string.Empty, productDetailViewModel, product.Image ?? string.Empty);
        }

        public async Task<(bool, string)> EditProductDetail(ProductDetailPostViewModel productDetailPostViewModel)
        {
            var product = await this._productRepository.GetProductById(productDetailPostViewModel.ProductDetail.ProductId);

            if (product == null)
            {
                return (false, "Id sản phẩm không hợp lệ !");
            }

            product.ProductName = productDetailPostViewModel.ProductDetail.ProductName;
            product.Summary = productDetailPostViewModel.ProductDetail.Summary;
            product.Content = productDetailPostViewModel.ProductDetail.Content;
            product.UpdatedAt = productDetailPostViewModel.ProductDetail.UpdatedAt;
            product.OriginalPrice = productDetailPostViewModel.ProductDetail.OriginalPrice;
            product.Discount = productDetailPostViewModel.ProductDetail.Discount;
            product.IsApprove = false;
            product.Approver = null;
            product.ApprovedAt = null;

            (bool success, string message, string newProductImagesPath) = await this._fileUploadService.ImagesUpload(productDetailPostViewModel.ProductImages, this._productVirtualPath);

            if (!success)
            {
                return (false, message);
            }

            product.Image = string.Concat(product.Image, newProductImagesPath);

            if (productDetailPostViewModel.Ingredients == null || !productDetailPostViewModel.Ingredients.Any())
                return (false, "Vui lòng chọn ít nhất 1 nguyên liệu !");

            return (true, $"Cập nhật sản phẩm {productDetailPostViewModel.ProductDetail.ProductId} thành công !");



        }

        public async Task<(bool, string)> DeleteProduct(int productId)
        {
            var product = await this._productRepository.GetProductById(productId);
            if (product == null)
                return (false, "Id sản phẩm không hợp lệ !");

            var isProductHasOrder = await this._productRepository.IsProductHasOrder(productId);
            if (isProductHasOrder)
            {
                return (false, "Sản phẩm đã có đơn hàng. Không thể xóa !");
            }

            var productIngredients = await this._productIngredientRepository.GetProductIngredientByProductId(productId);
            await this._productIngredientRepository.DeleteProductIngredient(productIngredients);

            var imagesSplitted = CommonHelper.ImageSplitter(product.Image ?? string.Empty);

            foreach (var image in imagesSplitted)
            {
                CommonHelper.DeleteFile(image);
            }

            await this._productRepository.DeleteProduct(product);

            return (true, $"Xóa sản phẩm {productId} thành công !");
        }

        public async Task<(bool, string)> DeleteProducts(int[] productIds)
        {
            List<int> successIds = new();
            List<int> failedIds = new();

            foreach (int productId in productIds)
            {
                (bool success, string message) = await this.DeleteProduct(productId);
                if (success)
                    successIds.Add(productId);
                else
                    failedIds.Add(productId);
            }
            return (true, $"Đã xóa thành công {successIds.Count}/{failedIds.Count} sản phẩm.\nCác Id xóa thất bại gồm {failedIds.ToArray()}");
        }

        public async Task<(bool, string)> ApproveProduct(int? approverId, int productId, bool isApproved)
        {
            var product = await this._productRepository.GetProductById(productId);

            if (product == null)
            {
                return (false, "Id sản phẩm không hợp lệ !");
            }

            if (isApproved)
            {
                product.IsApprove = true;
                product.ApproverId = approverId;
                product.ApprovedAt = DateTime.Now;

                await this._productRepository.UpdateProduct(product);
                return (true, $"Đã phê duyệt sản phẩm {productId} !");
            }

            product.IsApprove = false;
            product.ApprovedAt = null;
            product.ApproverId = null;
            return (true, $"Đã thu hồi sản phẩm {productId} !");

        }

        public async Task<(bool, string)> ApproveProducts(int? approverId, int[] productIds, bool isApproved)
        {
            List<int> successIds = new();
            List<int> failedIds = new();
            foreach (var productId in productIds)
            {
                (bool success, string message) = await this.ApproveProduct(approverId, productId, isApproved);

                if (success)
                    successIds.Add(productId);
                else
                    failedIds.Add(productId);
            }

            string isApprovedText = isApproved ? "phê duyệt" : "thu hồi";
            return (true, $"Đã {isApprovedText} thành công {successIds.Count}/{failedIds.Count} sản phẩm !");

        }

        public async Task<CustomProductDetailViewModel> GetCustomProductDetailViewModel(int productId)
        {
            var product = await this._productRepository.GetProductById(productId);
            var productReviews = await this._productReviewRepository.GetProductReviewsByProductId(productId);
            var topProductReview = productReviews.FirstOrDefault() ?? new ProductReview();
            var randomProducts = await this._productRepository.GetRandomProducts(true, productId, 4);

            CustomProductDetailViewModel customProductDetailViewModel = new()
            {
                Product = product,
                Image = CommonHelper.ImageSplitter(product.Image ?? string.Empty),
                ProductReviews = productReviews,
                TopProductReview = topProductReview,
                RandomProducts = randomProducts
            };
            return customProductDetailViewModel;
        }

        public async Task<Dictionary<string, int>> GetCustomProductsByTopSale(int take)
        {
            var products = await this._productRepository.GetProductsByApproveStatus(true);
            return products
                .OrderByDescending(x => x.OrderDetails.Sum(od => od.Quantity))
                .Take(take)
                .ToDictionary(
                    x => x.ProductName,
                    x => x.OrderDetails.Sum(od => od.Quantity)
                );
        }
    }
}
