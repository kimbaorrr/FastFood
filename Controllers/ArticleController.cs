using FastFood.DB;
using FastFood.Models;
using FastFood.Repositories.Interfaces;
using FastFood.Services;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Areas.Controllers
{
    [Route("articles")]
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly IArticleRepository _articleRepository;

        public ArticleController(IArticleService articleService, IArticleRepository articleRepository)
        {
            _articleService = articleService;
            _articleRepository = articleRepository;
        }

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
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> Detail([FromQuery] int articleId)
        {
            var article = await this._articleRepository.GetArticleById(articleId);

            ViewBag.Title = "Tin tức";
            ViewBag.Article = article;
            ViewBag.ReturnUrl = GetAbsoluteUri();
            return View();

        }
    }
}