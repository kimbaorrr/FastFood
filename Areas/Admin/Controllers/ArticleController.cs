using FastFood.Models.ViewModels;
using FastFood.Services;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;
namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/articles")]
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(NewArticleViewModel newArticleViewModel)
        {
            if (ModelState.IsValid)
            {
                (bool success, string message) = await this._articleService.NewArticle(newArticleViewModel);

                return CreateJsonResult(success, message);
            }
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return CreateJsonResult(false, string.Join(" | ", errors));
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            ViewBag.Title = "Tất cả bài viết";
            var articles = await this._articleService.GetArticlesPagedList(page, size);
            ViewBag.Articles = articles;
            ViewBag.CurrentPage = articles.PageNumber;
            ViewBag.TotalPages = articles.PageCount;
            return View();
        }

        [HttpPost("delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete([FromForm] int articleId)
        {
            (bool success, string message) = await this._articleService.DeleteArticle(articleId);
            return CreateJsonResult(success, message);
        }

        [HttpPost("multiple-delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> MultipleDelete([FromForm] int[] articleId)
        {
            (bool success, string message) = await this._articleService.DeleteArticles(articleId);
            return CreateJsonResult(success, message);
        }

        [HttpPost("approve")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Approve([FromForm] int articleId, [FromForm] bool action)
        {
            (bool success, string message) = await this._articleService.ApproveArticle(articleId, this._employeeId, action);
            return CreateJsonResult(success, message);
        }

        [HttpPost("multiple-approve")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> MultipleApprove([FromForm] int[] articleIds, [FromForm] bool action)
        {
            (bool success, string message) = await this._articleService.ApproveArticles(articleIds, this._employeeId, action);
            return CreateJsonResult(success, message);
        }

        [HttpGet("not-approve")]
        public async Task<IActionResult> GetNotApprove()
        {
            var notApprovedArticles = await this._articleService.GetArticlesNotApprove();
            return CreateJsonResult(true, string.Empty, notApprovedArticles);
        }

        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Edit([FromForm] EditArticleViewModel editArticleViewModel)
        {
            (bool success, string message) = await this._articleService.EditArticle(editArticleViewModel);
            return CreateJsonResult(success, message);
        }


    }
}