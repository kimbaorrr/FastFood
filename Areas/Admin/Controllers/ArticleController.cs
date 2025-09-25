using FastFood.Models.ViewModels;
using FastFood.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;
namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/articles")]
    [Authorize(AuthenticationSchemes = "EmployeeScheme")]
    public class ArticleController : BaseEmployeeController
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
                (bool success, string message) = await this._articleService.NewArticle(newArticleViewModel, this._employeeId);

                return CreateJsonResult(success, message);
            }
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return CreateJsonResult(false, string.Join(" | ", errors));
        }

        [HttpGet("get")]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            ViewBag.Title = "Tất cả bài viết";
            var articles = await this._articleService.GetArticlesPagedList(page, size);
            ViewBag.Articles = articles;
            ViewBag.CurrentPage = articles.PageNumber;
            ViewBag.TotalPages = articles.PageCount;
            return View();
        }

        [HttpGet("approve")]
        public async Task<IActionResult> Approve([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            ViewBag.Title = "Bài viết chờ phê duyệt";
            var articles = await this._articleService.GetArticlesByApproveStatusPagedList(false, page, size);

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

        [HttpGet("not-approve/get")]
        public async Task<IActionResult> GetNotApprove()
        {
            var notApprovedArticles = await this._articleService.GetArticlesNotApprove();
            var myArticlesNotApproved = notApprovedArticles.Where(x => x.AuthorId == this._employeeId);
            return CreateJsonResult(true, string.Empty, myArticlesNotApproved);
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