using FastFood.DB;
using FastFood.Models;
using FastFood.Models.ViewModels;
using FastFood.Repositories.Interfaces;
using FastFood.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using X.PagedList;
using X.PagedList.Extensions;
namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/products")]
    [Authorize(AuthenticationSchemes = "EmployeeScheme")]
    public class ProductController : BaseEmployeeController
    {
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryService _categoryService;
        private readonly IEmployeeRepository _employeeRepository;


        public ProductController(IProductService productService, IProductRepository productRepository, ICategoryService categoryService, IEmployeeRepository employeeRepository)
        {
            _productService = productService;
            _productRepository = productRepository;
            _categoryService = categoryService;
            _employeeRepository = employeeRepository;

        }

        [HttpPost("get")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await this._productRepository.GetProducts();
            return CreateJsonResult(true, string.Empty, products);
        }

        [HttpGet("get/approved")]
        public async Task<IActionResult> GetProductsApproved([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var productsByApproveStatus = await this._productService.GetProductsByApproveStatusPagedList(true, page, size);
            ViewBag.Producs = productsByApproveStatus;
            ViewBag.CurrentPage = productsByApproveStatus.PageNumber;
            ViewBag.TotalPages = productsByApproveStatus.PageCount;
            return View();
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var customCategoriesSelectList = await this._categoryService.GetCustomCategoriesSelectList();
            var employeeFullName = this._employeeRepository.GetFullName(this._employeeId);

            NewProductViewModel newProductViewModel = new()
            {
                CreatedBy = employeeFullName,
                Categories = customCategoriesSelectList
            };
            return View(newProductViewModel);
        }

        [HttpPost("create")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([FromForm] NewProductPostViewModel newProductPostViewModel)
        {
            (bool success, string messsage) = await this._productService.AddProduct(newProductPostViewModel);
            return CreateJsonResult(success, messsage);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> Detail([FromQuery] int productId)
        {
            (bool success, string message, ProductDetailViewModel productDetailViewModel, string images) = await this._productService.GetProductDetailViewModel(productId, this._employeeId);

            if (success)
            {
                ViewBag.ProductId = productDetailViewModel.ProductId;
                ViewBag.ReturnURL = GetAbsoluteUri();
                ViewBag.ProductImages = CommonHelper.ImageSplitter(images);
                return View(productDetailViewModel);
            }
            return View();
        }

        [HttpPost("detail")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Detail([FromForm] ProductDetailPostViewModel productDetailPostViewModel)
        {
            (bool success, string message) = await this._productService.EditProductDetail(productDetailPostViewModel);

            return CreateJsonResult(success, message);
        }

        [HttpPost("delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete([FromForm] int productId)
        {
            (bool success, string message) = await this._productService.DeleteProduct(productId);
            return CreateJsonResult(success, message);
        }

        [HttpPost("multiple-delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> MultipleDelete([FromForm] int[] productIds)
        {
            (bool success, string message) = await this._productService.DeleteProducts(productIds);
            return CreateJsonResult(success, message);
        }

        [HttpPost("approve")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve([FromForm] int productId, [FromForm] bool action)
        {
            (bool success, string message) = await this._productService.ApproveProduct(this._employeeId, productId, action);
            return CreateJsonResult(success, message);
        }

        [HttpPost("multiple-approve")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MultipleApprove([FromForm] int[] productIds, [FromForm] bool action)
        {
            (bool success, string message) = await this._productService.ApproveProducts(this._employeeId, productIds, action);
            return CreateJsonResult(success, message);
        }




    }
}