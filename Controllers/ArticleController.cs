using FastFood.DB;
using FastFood.Models;
using FastFood.Repositories.Interfaces;
using FastFood.Services;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Controllers
{
    /// <summary>
    /// Controller xử lý các chức năng liên quan đến bài viết.
    /// </summary>
    [Route("articles")]
    public class ArticleController : BaseCustomerController
    {
        private readonly IArticleService _articleService;
        private readonly IArticleRepository _articleRepository;

        /// <summary>
        /// Khởi tạo controller bài viết với các dịch vụ cần thiết.
        /// </summary>
        /// <param name="articleService">Dịch vụ bài viết.</param>
        /// <param name="articleRepository">Kho lưu trữ bài viết.</param>
        public ArticleController(IArticleService articleService, IArticleRepository articleRepository)
        {
            _articleService = articleService;
            _articleRepository = articleRepository;
        }

        /// <summary>
        /// Hiển thị danh sách bài viết đã được duyệt, có phân trang.
        /// </summary>
        /// <param name="page">Trang hiện tại.</param>
        /// <param name="size">Số lượng bài viết trên mỗi trang.</param>
        /// <returns>Trang danh sách bài viết.</returns>
        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int size = 6)
        {
            var articles = await this._articleService.GetArticlesByApproveStatusPagedList(true, page, size);

            ViewBag.Articles = articles;
            ViewBag.CurrentPage = articles.PageNumber;
            ViewBag.TotalPages = articles.PageCount;
            ViewBag.Title = "Tin tức";
            return View();

        }

        /// <summary>
        /// Hiển thị chi tiết một bài viết.
        /// </summary>
        /// <param name="articleId">ID bài viết cần xem chi tiết.</param>
        /// <returns>Trang chi tiết bài viết.</returns>
        [HttpGet("detail/{articleId}")]
        public async Task<IActionResult> Detail([FromQuery] int articleId)
        {
            var customArticleViewModel = await this._articleService.GetCustomArticleViewModel(articleId);

            ViewBag.Title = "Tin tức";
            ViewBag.ReturnUrl = GetAbsoluteUri();

            return View(customArticleViewModel);

        }
    }
}